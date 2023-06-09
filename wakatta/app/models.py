from fastapi_users.db import SQLAlchemyBaseUserTableUUID
from sqlalchemy import Column, String, Integer, ForeignKey
from sqlalchemy import and_
from sqlalchemy.orm import declarative_base, relationship, backref

Base = declarative_base()


class User(SQLAlchemyBaseUserTableUUID, Base):
    nickname = Column(String(length=32), index=True, nullable=False)
    privilege = Column(Integer, nullable=False, default=0)


class Class(Base):
    __tablename__ = "class"
    id = Column(Integer, primary_key=True, autoincrement=True)
    label = Column(String(length=32), nullable=False)
    time_hour = Column(Integer)
    time_minute = Column(Integer)
    time_duration = Column(Integer, default=40)
    client_id = Column(Integer, ForeignKey('client.id'), index=True)


class Client(Base):
    __tablename__ = "client"
    id = Column(Integer, primary_key=True, autoincrement=True)
    hardware_id = Column(String(length=128), index=True, nullable=False)
    identifier = Column(String(length=32), index=True, nullable=False, default='Client')
    class_begin_ringtone_filename = Column(String(length=32), nullable=False, default='default_class_begin.wav')
    class_over_ringtone_filename = Column(String(length=32), nullable=False, default='default_class_over.wav')
    version = Column(String(length=32), nullable=False)
    subscribe_schedule_id = Column(Integer, ForeignKey('schedule.id'))
    vits_id = Column(Integer, default=133)
    classes = relationship('Class', backref=backref('client', lazy=False), lazy='selectin', uselist=True,
                           order_by=and_(Class.time_hour, Class.time_minute))
    subscribe_schedule = relationship('Schedule', backref=backref('client', lazy='dynamic'), lazy=False)


class ScheduleClass(Base):
    __tablename__ = "schedule_class"
    id = Column(Integer, primary_key=True, autoincrement=True)
    label = Column(String(length=32), nullable=False)
    time_hour = Column(Integer)
    time_minute = Column(Integer)
    time_duration = Column(Integer, default=40)
    weekday = Column(Integer)
    schedule_id = Column(Integer, ForeignKey('schedule.id'), index=True)


class Schedule(Base):
    __tablename__ = "schedule"
    id = Column(Integer, primary_key=True, autoincrement=True)
    label = Column(String(length=32), nullable=False)
    classes = relationship('ScheduleClass', backref=backref('schedule', lazy=False), lazy='selectin', uselist=True,
                           order_by=and_(ScheduleClass.weekday, ScheduleClass.time_hour, ScheduleClass.time_minute))
