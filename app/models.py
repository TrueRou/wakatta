from fastapi_users.db import SQLAlchemyBaseUserTableUUID
from sqlalchemy import Column, String, Integer, ForeignKey
from sqlalchemy.orm import declarative_base, relationship, backref

Base = declarative_base()


class User(SQLAlchemyBaseUserTableUUID, Base):
    nickname = Column(String(length=32), index=True, nullable=False)
    privilege = Column(Integer, nullable=False, default=0)


class Client(Base):
    __tablename__ = "client"
    id = Column(Integer, primary_key=True, autoincrement=True)
    hardware_id = Column(String(length=128), index=True, nullable=False)
    identifier = Column(String(length=32), index=True, nullable=False, default='Client')
    subscribe_schedule_id = Column(Integer, ForeignKey('schedule.id'))
    classes = relationship('Class', backref=backref('client', lazy=False), lazy='selectin', uselist=True)
    subscribe_schedule = relationship('Schedule', backref=backref('client', lazy='dynamic'), lazy=False)


class Class(Base):
    __tablename__ = "class"
    id = Column(Integer, primary_key=True, autoincrement=True)
    label = Column(String(length=32), nullable=False)
    time_hour = Column(Integer)
    time_minute = Column(Integer)
    client_id = Column(Integer, ForeignKey('client.id'), index=True)


class Schedule(Base):
    __tablename__ = "schedule"
    id = Column(Integer, primary_key=True, autoincrement=True)
    label = Column(String(length=32), nullable=False)
    classes = relationship('ScheduleClass', backref=backref('schedule', lazy=False), lazy='dynamic')


class ScheduleClass(Base):
    __tablename__ = "schedule_class"
    id = Column(Integer, primary_key=True, autoincrement=True)
    label = Column(String(length=32), nullable=False)
    time_hour = Column(Integer)
    time_minute = Column(Integer)
    weekday = Column(Integer)
    schedule_id = Column(Integer, ForeignKey('schedule.id'), index=True)
