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


class Schedule(ModelBase):
    id: int
    label: str


class Client(ModelBase):
    id: int
    identifier: str
    subscribe_schedule: Optional[Schedule]
    classes: List[Class]


class UserRead(schemas.BaseUser[uuid.UUID]):
    nickname: str
    privilege: int


class UserCreate(schemas.BaseUserCreate):
    nickname: str


class UserUpdate(schemas.BaseUserUpdate):
    nickname: str
