from enum import IntEnum


class Packets(IntEnum):
    MESSAGE = 1
    REFRESH_SCHEDULE = 2
    RECONNECT = 3
