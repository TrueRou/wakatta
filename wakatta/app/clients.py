from typing import List

from fastapi import APIRouter, Depends
from random_words import RandomNicknames
from starlette import status

import services
from app import models, schemas
from app.session import online_clients, client_messages, refresh_client, client_need_refresh, enqueue_message, \
    enqueue_message_client
from app.users import current_privilege_user
from services import db_session

client_router = APIRouter(prefix='/client', tags=['client'])
random = RandomNicknames()


@client_router.put('', response_model=schemas.Client)
async def put_client(hardware_id: str):
    async with db_session() as session:
        client = await services.select_model(session, models.Client, models.Client.hardware_id == hardware_id)
        if client is None:
            identifier = random.random_nick(gender='f')
            client = models.Client(hardware_id=hardware_id, identifier=identifier)
            await services.add_model(session, client)
        refresh_client(client.id)
        return client


@client_router.get('/class', response_model=List[schemas.Class])
async def get_classes(client_id: int):
    async with db_session() as session:
        client = await services.get_model(session, client_id, models.Client)
        return client.classes


@client_router.get('/heartbeat')
async def heartbeat_client(client_id: int):
    if client_id not in online_clients:
        online_clients.append(client_id)
    response = {
        'messages': client_messages[client_id],
        'need_refresh': client_need_refresh[client_id]
    }
    refresh_client(client_id)
    return response


@client_router.get('', response_model=schemas.Client, dependencies=[Depends(current_privilege_user)])
async def get_client(client_id: int):
    async with db_session() as session:
        return await services.get_model(session, client_id, models.Client)


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
        clazz = models.Class(label=form.label, time_hour=form.time_hour, time_minute=form.time_minute, client_id=client_id)
        await services.add_model(session, clazz)
        return clazz


@client_router.patch('/class', response_model=schemas.Class, dependencies=[Depends(current_privilege_user)])
async def patch_class(class_id: int, form: schemas.ClassUpdate):
    async with db_session() as session:
        clazz = await services.get_model(session, class_id, models.Class)
        await services.partial_update(session, clazz, form)
        return clazz


@client_router.delete('/class', status_code=status.HTTP_200_OK, dependencies=[Depends(current_privilege_user)])
async def delete_class(class_id: int):
    async with db_session() as session:
        await services.delete_model(session, class_id, models.Class)


@client_router.post('/message', status_code=status.HTTP_200_OK, dependencies=[Depends(current_privilege_user)])
async def create_message(message: str, client_id: int = -1):
    if client_id == -1:
        enqueue_message(message)
    else:
        enqueue_message_client(message, client_id)
