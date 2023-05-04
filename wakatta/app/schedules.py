from datetime import datetime
from typing import List

from fastapi import APIRouter, Depends
from sqlalchemy import and_
from starlette import status

import services
from app import schemas, models
from app.schemas import ScheduleBase
from app.users import current_privilege_user
from services import db_session

schedule_router = APIRouter(prefix='/schedule', tags=['schedule'], dependencies=[Depends(current_privilege_user)])


def get_weekday():
    weekday = datetime.now().isoweekday()
    weekday = 0 if weekday == 7 else weekday
    return weekday


async def _broadcast_schedule(schedule_id: int):
    async with db_session() as session:
        clients = await services.select_models(session, models.Client,
                                               models.Client.subscribe_schedule_id == schedule_id)
        for client in clients:
            await apply_schedule(schedule_id, client.id)


@schedule_router.get('/all', response_model=List[schemas.Schedule])
async def get_schedules():
    async with db_session() as session:
        schedules = await services.select_models(session, models.Schedule, None)
        return schedules.all()


@schedule_router.post('', response_model=schemas.Schedule)
async def create_schedule(form: ScheduleBase):
    async with db_session() as session:
        schedule = models.Schedule(label=form.label)
        await services.add_model(session, schedule)
        return schedule


@schedule_router.get('', response_model=schemas.Schedule)
async def get_schedule(schedule_id: int):
    async with db_session() as session:
        schedule = await services.get_model(session, schedule_id, models.Schedule)
        return schedule


@schedule_router.delete('', status_code=status.HTTP_200_OK)
async def delete_schedule(schedule_id: int):
    async with db_session() as session:
        await services.delete_model(session, schedule_id, models.Schedule)


@schedule_router.get('/class', response_model=list[schemas.ScheduleClass])
async def get_schedule_classes(schedule_id: int, weekday: int):
    async with db_session() as session:
        condition = and_(models.ScheduleClass.schedule_id == schedule_id, models.ScheduleClass.weekday == weekday)
        result = await services.select_models(session, models.ScheduleClass, condition)
        return result.all()


@schedule_router.post('/class', response_model=schemas.ScheduleClass)
async def create_schedule_class(schedule_id: int, form: schemas.ScheduleClassBase):
    async with db_session() as session:
        await services.get_model(session, schedule_id, models.Schedule)
        clazz = models.ScheduleClass(label=form.label, time_hour=form.time_hour, time_minute=form.time_minute,
                                     schedule_id=schedule_id,
                                     time_duration=form.time_duration)
        await services.add_model(session, clazz)
        await _broadcast_schedule(schedule_id)
        return clazz


@schedule_router.patch('/class', response_model=schemas.ScheduleClass)
async def patch_schedule_class(class_id: int, form: schemas.ScheduleClassUpdate):
    async with db_session() as session:
        clazz = await services.get_model(session, class_id, models.ScheduleClass)
        await services.partial_update(session, clazz, form)
        await _broadcast_schedule(clazz.schedule_id)
        return clazz


@schedule_router.delete('/class', status_code=status.HTTP_200_OK)
async def delete_schedule_class(class_id: int):
    async with db_session() as session:
        clazz = await services.get_model(session, class_id, models.ScheduleClass)
        await services.delete_model(session, class_id, models.ScheduleClass)
        await _broadcast_schedule(clazz.schedule_id)


@schedule_router.post('/apply', response_model=schemas.Client)
async def apply_schedule(schedule_id: int, client_id: int):
    weekday = get_weekday()
    async with db_session() as session:
        schedule = await services.get_model(session, schedule_id, models.Schedule)
        client = await services.get_model(session, client_id, models.Client)
        await services.delete_models(session, models.Class, models.Class.client_id == client.id)
        sentence = schedule.classes.where(models.ScheduleClass.weekday == weekday)
        classes = await session.scalars(sentence)
        for clazz in classes:
            new_clazz = models.Class(label=clazz.label, time_hour=clazz.time_hour, time_minute=clazz.time_minute,
                                     client_id=client.id, time_duration=clazz.time_duration)
            session.add(new_clazz)
        await session.refresh(client)
        return client
