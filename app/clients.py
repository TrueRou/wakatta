from fastapi import APIRouter, Depends
from starlette import status

import services
from app import models, schemas
from app.users import current_privilege_user
from services import db_session

client_router = APIRouter(prefix='/client', tags=['client'])


@client_router.put('', response_model=schemas.Client)
async def put_client(hardware_id: str, identifier: str):
    async with db_session() as session:
        client = await services.select_model(session, models.Client, models.Client.hardware_id == hardware_id)
        if client is not None:
            client.identifier = identifier
        else:
            client = models.Client(hardware_id=hardware_id, identifier=identifier)
            await services.add_model(session, client)
        return client


@client_router.post('/class', response_model=schemas.Class, dependencies=[Depends(current_privilege_user)])
async def create_class(client_id: int, label: str, time_hour: int, time_minute: int):
    async with db_session() as session:
        await services.get_model(session, client_id, models.Client)
        clazz = models.Class(label=label, time_hour=time_hour, time_minute=time_minute, client_id=client_id)
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
