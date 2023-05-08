import uuid
from typing import List, Optional

from fastapi_users import schemas
from pydantic import BaseModel


def convert_to_optional(schema):
    return {k: Optional[v] for k, v in schema.__annotations__.items()}


class ModelBase(BaseModel):
    class Config:
        orm_mode = True


class ClassBase(ModelBase):
    label: str
    time_hour: int
    time_minute: int
    time_duration: int


class Class(ClassBase):
    id: int


class ScheduleClassBase(ClassBase):
    weekday: int


class ScheduleClass(ScheduleClassBase):
    id: int


class ClassUpdate(ModelBase):
    __annotations__ = convert_to_optional(ClassBase)


class ScheduleClassUpdate(ModelBase):
    __annotations__ = convert_to_optional(ClassBase)
    weekday: Optional[int]


class ScheduleBase(ModelBase):
    label: str


class Schedule(ScheduleBase):
    id: int
    classes: list[ScheduleClass]


class ClientBase(ModelBase):
    id: int
    identifier: str
    version: str
    hardware_id: str
    class_begin_ringtone_filename: str
    class_over_ringtone_filename: str
    subscribe_schedule_id: Optional[int]


class Client(ClientBase):
    subscribe_schedule: Optional[Schedule]
    classes: list[Class]


class ClientUpdate(ClientBase):
    __annotations__ = convert_to_optional(ClientBase)


class Statistics(ModelBase):
    online_users: list[int]
    clients: list[Client]
    schedules: list[Schedule]
    dashboard_statistics: dict


class UserRead(schemas.BaseUser[uuid.UUID]):
    nickname: str
    privilege: bool


class UserCreate(schemas.BaseUserCreate):
    nickname: str


class UserUpdate(schemas.BaseUserUpdate):
    nickname: str
    privilege: bool
