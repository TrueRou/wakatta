import requests
from fastapi import APIRouter

import config
import services
from app import models
from services import db_session

vits_router = APIRouter(prefix='/vits', tags=['vits'])
vits_characters = []

if config.vits_enabled:
    request = requests.get(config.vits_entrypoint + "voice/speakers")
    request.encoding = 'utf-8'
    vits_characters = request.json()['VITS']


@vits_router.get('/characters')
async def get_characters():
    return {
        'enabled': config.vits_enabled,
        'characters': vits_characters
    }


@vits_router.get('/entrypoint')
async def get_entrypoint():
    return {
        'enabled': config.vits_enabled,
        'entrypoint': config.vits_entrypoint
    }


@vits_router.get('/id')
async def get_character_id(client_id: int):
    async with db_session() as session:
        client = await services.get_model(session, client_id, models.Client)
        return {
            'enabled': config.vits_enabled,
            'id': client.vits_id
        }