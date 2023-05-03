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


class ClassUpdate(ModelBase):
    __annotations__ = convert_to_optional(ClassBase)


class ScheduleBase(ModelBase):
    label: str


class Schedule(ScheduleBase):
    id: int
    classes: list[Class]


class Client(ModelBase):
    id: int
    identifier: str
    subscribe_schedule: Optional[Schedule]
    classes: list[Class]


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
