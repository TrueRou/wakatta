import requests
from fastapi import APIRouter

import config

vits_router = APIRouter(prefix='/vits', tags=['vits'])
vits_characters = []

if config.vits_enabled:
    try:
        request = requests.get(config.vits_entrypoint + "voice/speakers")
        request.encoding = 'utf-8'
        vits_characters = request.json()['VITS']
    except requests.exceptions.ConnectionError:
        config.vits_enabled = False


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
