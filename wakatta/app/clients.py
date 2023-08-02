from typing import List

from fastapi import APIRouter, Depends
from random_words import RandomNicknames

from starlette import status

import services
from app import models, schemas, schedules
from app.packets import Packets
from app.session import tick_client, client_packets, send_packet, send_packets
from app.users import current_privilege_user
from services import db_session

client_router = APIRouter(prefix='/client', tags=['client'])
random = RandomNicknames()


def _notify_client(client_id: int):
    send_packet(Packets.REFRESH_SCHEDULE, '', client_id)


def _notify_client_hard(client_id: int):
    send_packet(Packets.RECONNECT, '', client_id)


@client_router.get('/create', response_model=schemas.Client)
async def create_client(hardware_id: str, version: str):
    async with db_session() as session:
        client = await services.select_model(session, models.Client, models.Client.hardware_id == hardware_id)
        if client is None:
            identifier = random.random_nick(gender='f')
            client = models.Client(hardware_id=hardware_id, identifier=identifier, version=version)
            await services.add_model(session, client)
        client.version = version
        tick_client(client.id)
        return client


@client_router.get('/class', response_model=List[schemas.Class])
async def get_classes(client_id: int):
    async with db_session() as session:
        client = await services.get_model(session, client_id, models.Client)
        return client.classes


@client_router.get('/heartbeat')
async def heartbeat_client(client_id: int):
    tick_client(client_id)
    queue = client_packets[client_id]
    result = []
    while not queue.empty():
        result.append(queue.get())
    return result


@client_router.get('', response_model=schemas.Client)
async def get_client(client_id: int):
    async with db_session() as session:
        return await services.get_model(session, client_id, models.Client)


@client_router.patch('', response_model=schemas.Client, dependencies=[Depends(current_privilege_user)])
async def patch_client(client_id: int, form: schemas.ClientUpdate):
    async with db_session() as session:
        client = await services.get_model(session, client_id, models.Client)
        if form.subscribe_schedule_id != client.subscribe_schedule_id:
            await schedules.apply_schedule(form.subscribe_schedule_id, client_id)
        await services.partial_update(session, client, form)
        _notify_client_hard(client_id)
        return client


@client_router.delete('/subscription', response_model=schemas.Client, dependencies=[Depends(current_privilege_user)])
async def remove_subscription(client_id: int):
    async with db_session() as session:
        client = await services.get_model(session, client_id, models.Client)
        client.subscribe_schedule_id = None
        _notify_client(client_id)
        return client


@client_router.get('/all', response_model=List[schemas.Client], dependencies=[Depends(current_privilege_user)])
async def get_clients():
    async with db_session() as session:
        clients = await services.select_models(session, models.Client, None)
        return clients.all()


@client_router.get('/class', response_model=schemas.Class, dependencies=[Depends(current_privilege_user)])
async def get_class(client_id: int):
    async with db_session() as session:
        return await services.get_model(session, client_id, models.Client)


@client_router.post('/class', response_model=schemas.Class, dependencies=[Depends(current_privilege_user)])
async def create_class(client_id: int, form: schemas.ClassBase):
    async with db_session() as session:
        await services.get_model(session, client_id, models.Client)
        clazz = models.Class(label=form.label, time_hour=form.time_hour, time_minute=form.time_minute,
                             client_id=client_id, time_duration=form.time_duration)
        await services.add_model(session, clazz)
        _notify_client(client_id)
        return clazz


@client_router.patch('/class', response_model=schemas.Class, dependencies=[Depends(current_privilege_user)])
async def patch_class(class_id: int, form: schemas.ClassUpdate):
    async with db_session() as session:
        clazz = await services.get_model(session, class_id, models.Class)
        await services.partial_update(session, clazz, form)
        _notify_client(clazz.client_id)
        return clazz


@client_router.delete('/class', status_code=status.HTTP_200_OK, dependencies=[Depends(current_privilege_user)])
async def delete_class(class_id: int):
    async with db_session() as session:
        clazz = await services.get_model(session, class_id, models.Class)
        _notify_client(clazz.client_id)
        await services.delete_model(session, class_id, models.Class)


@client_router.post('/message', status_code=status.HTTP_200_OK, dependencies=[Depends(current_privilege_user)])
async def create_message(message: str, client_id: int = -1):
    send_packet(Packets.MESSAGE, message, client_id) if client_id != -1 else send_packets(Packets.MESSAGE, message)


@client_router.post('/ring', status_code=status.HTTP_200_OK, dependencies=[Depends(current_privilege_user)])
async def ring_client(begin: bool, client_id: int = -1):
    packet = Packets.RING_CLASS_BEGIN if begin else Packets.RING_CLASS_OVER
    send_packet(packet, "", client_id) if client_id != -1 else send_packets(packet, "")
