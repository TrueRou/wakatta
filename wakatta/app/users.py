import uuid

from fastapi import HTTPException, Depends
from fastapi_users import BaseUserManager, FastAPIUsers, UUIDIDMixin
from fastapi_users.authentication import (
    AuthenticationBackend,
    BearerTransport,
    JWTStrategy,
)
from fastapi_users.db import SQLAlchemyUserDatabase
from starlette import status

from app.models import User
from config import jwt_secret
from services import db_session


class UserManager(UUIDIDMixin, BaseUserManager[User, uuid.UUID]):
    pass


async def get_user_manager():
    async with db_session() as session:
        yield UserManager(SQLAlchemyUserDatabase(session, User))


bearer_transport = BearerTransport(tokenUrl="auth/jwt/login")


def get_jwt_strategy() -> JWTStrategy:
    return JWTStrategy(secret=jwt_secret, lifetime_seconds=3600)


auth_backend = AuthenticationBackend(
    name="jwt",
    transport=bearer_transport,
    get_strategy=get_jwt_strategy,
)

fastapi_users = FastAPIUsers[User, uuid.UUID](get_user_manager, [auth_backend])

current_active_user = fastapi_users.current_user(active=True)


async def current_privilege_user(user: User = Depends(current_active_user)) -> User:
    if user.privilege != 1:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN,
            detail='No permission.',
        )
    return user
