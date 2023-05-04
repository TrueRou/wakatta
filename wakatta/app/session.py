import queue
from datetime import timedelta, datetime
from typing import List
from sqlalchemy import null

import services
from app import schedules, models
from app.packets import Packets

online_clients: List[int] = []
client_data: dict[int, dict] = {}
client_packets: dict[int, queue.Queue[dict]] = {}


def tick_session():
    for client in online_clients:
        # remove inactive clients
        last_activity = client_data[client]['last_activity']
        if last_activity is None or last_activity + timedelta(seconds=5) < datetime.now():
            online_clients.remove(client)
            client_data[client] = dict()
            client_packets[client] = queue.Queue()
            continue  # pass other actions


def tick_client(client_id: int):
    if client_id not in online_clients:
        client_data[client_id] = dict()
        client_packets[client_id] = queue.Queue()
    client_data[client_id]['last_activity'] = datetime.now()


def send_packet(packet_id: int, payload: str, client_id: int):
    client_packets[client_id].put({
        'packet_id': packet_id,
        'payload': payload
    })


def send_packets(packet_id: int, payload: str):
    for client in online_clients:
        send_packet(packet_id, payload, client)


async def update_subscription():
    async with services.db_session() as session:
        clients = await services.select_models(session, models.Client, models.Client.subscribe_schedule_id != null(), )
        for client in clients:
            client_id = client.id
            await schedules.apply_schedule(client.subscribe_schedule_id, client_id)
            if client_id in online_clients:
                send_packet(Packets.REFRESH_SCHEDULE, '', client_id)
