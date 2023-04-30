import asyncio
import queue
from datetime import timedelta, datetime
from typing import List

import sqlalchemy
from sqlalchemy import null

import services
from app import schedules, models

online_clients = []
message_queue = queue.Queue()
client_messages = {}
client_need_refresh = {}


def check_online_clients():
    for index, client in enumerate(online_clients):
        last_activity = client['last_activity']
        if last_activity + timedelta(seconds=5) < datetime.now():
            last_activity.pop(index)


def refresh_client(client_id: int):
    client_messages[client_id] = []
    client_need_refresh[client_id] = False


def enqueue_message(message: str):
    message_queue.put(message)


def enqueue_message_client(message: str, client_id: int):
    if isinstance(client_messages[client_id], List):
        client_messages[client_id].append(message)


def dequeue_messages():
    while not message_queue.empty():
        message = message_queue.get()
        for client_id in online_clients:
            enqueue_message_client(message, client_id)


async def update_subscription():
    async with services.db_session() as session:
        clients = await services.select_models(session, models.Client, models.Client.subscribe_schedule_id != null(), )
        for client in clients:
            client_id = client.id
            await schedules.apply_schedule(client.subscribe_schedule_id, client_id)
            if client_id in online_clients:
                client_need_refresh[client_id] = True
