from enum import IntEnum


class Packets(IntEnum):
    MESSAGE = 1
    REFRESH_SCHEDULE = 2
    RECONNECT = 3
    RING_CLASS_BEGIN = 4
    RING_CLASS_OVER = 5
