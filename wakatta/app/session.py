import queue
from datetime import timedelta, datetime
from typing import List

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
        online_clients.append(client_id)
    client_data[client_id]['last_activity'] = datetime.now()


def send_packet(packet_id: int, payload: str, client_id: int):
    if client_id in client_packets:
        client_packets[client_id].put({
            'packet_id': int(packet_id),
            'payload': payload
        })


def send_packets(packet_id: int, payload: str):
    for client in online_clients:
        send_packet(packet_id, payload, client)
