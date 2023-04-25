from fastapi import FastAPI

import services
from app import clients, schedules
from app.schemas import UserRead, UserUpdate, UserCreate
from app.users import fastapi_users, auth_backend

app = FastAPI()

app.include_router(clients.client_router)
app.include_router(schedules.schedule_router)
app.include_router(fastapi_users.get_auth_router(auth_backend), prefix="/auth/jwt", tags=["auth"])
app.include_router(fastapi_users.get_users_router(UserRead, UserUpdate), prefix="/users", tags=["users"])
app.include_router(fastapi_users.get_register_router(UserRead, UserCreate), prefix="/auth", tags=["auth"])


@app.on_event("startup")
async def on_startup():
    await services.create_db_and_tables()
