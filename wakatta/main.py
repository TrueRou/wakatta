from typing import List

from apscheduler.schedulers.asyncio import AsyncIOScheduler
from fastapi import FastAPI, Depends
from starlette.middleware.cors import CORSMiddleware

import services
from app.models import User
from services import db_session
from app import clients, schedules, session, models, schemas
from app.schemas import UserRead, UserUpdate, UserCreate
from app.users import fastapi_users, auth_backend, current_superuser, current_privilege_user

app = FastAPI()
scheduler = AsyncIOScheduler()

app.include_router(clients.client_router)
app.include_router(schedules.schedule_router)
app.include_router(fastapi_users.get_auth_router(auth_backend), prefix="/auth/jwt", tags=["auth"])
app.include_router(fastapi_users.get_users_router(UserRead, UserUpdate), prefix="/users", tags=["users"])
app.include_router(fastapi_users.get_register_router(UserRead, UserCreate), prefix="/auth", tags=["auth"])
origins = [
    "http://localhost:5173",
]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)


@app.on_event("startup")
async def on_startup():
    await services.create_db_and_tables()
    await session.update_subscription()  # Apply subscription when startup to ensure lessons of current weekday
    scheduler.add_job(session.check_online_clients, 'interval', seconds=5)
    scheduler.add_job(session.dequeue_messages, 'interval', seconds=1)
    scheduler.add_job(session.update_subscription, 'cron', hour=0)  # Apply subscription every single day
    scheduler.start()


@app.get("/statistics", tags=['statistics'], dependencies=[Depends(current_privilege_user)])
async def get_statistics():
    async with db_session() as the_session:
        return {
            'online_users': session.online_clients,
            'clients': await clients.get_clients(),
            'schedules': await schedules.get_schedules(),
            'dashboard_statistics': {
                'client_count': await services.select_models_count(the_session, models.Client, None),
                'schedule_count': await services.select_models_count(the_session, models.Schedule, None),
                'class_count': await services.select_models_count(the_session, models.Class, None),
                'schedule_class_count': await services.select_models_count(the_session, models.ScheduleClass, None),
                'user_count': await services.select_models_count(the_session, models.ScheduleClass, None),
                'online_users_count': len(session.online_clients),
            }
        }


@app.get("/statistics/users", response_model=List[schemas.UserRead], tags=['statistics'], dependencies=[Depends(current_superuser)])
async def get_all_users():
    async with db_session() as the_session:
        users = await services.select_models(the_session, User, None)
        return users.all()
