import contextlib
from typing import TypeVar, AsyncContextManager

from fastapi import HTTPException
from sqlalchemy import select, ScalarResult, delete
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession
from sqlalchemy.ext.asyncio import async_sessionmaker
from starlette import status

import config
from app.models import Base

engine = create_async_engine(config.mysql_url, echo=config.debug, future=True)
async_session_maker = async_sessionmaker(engine, class_=AsyncSession, expire_on_commit=False)
V = TypeVar("V")


async def create_db_and_tables():
    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)


@contextlib.asynccontextmanager
async def db_session() -> AsyncContextManager[AsyncSession]:
    async with async_session_maker() as session:
        yield session
        await session.commit()


async def add_model(session: AsyncSession, obj: V) -> V:
    session.add(obj)
    await session.commit()
    await session.refresh(obj)
    return obj


async def delete_model(session: AsyncSession, ident, model):
    target = await session.get(model, ident)
    await session.delete(target)


async def delete_models(session: AsyncSession, obj, condition):
    sentence = delete(obj).where(condition)
    await session.execute(sentence)


async def get_model(session: AsyncSession, ident, model: V) -> V:
    model = await session.get(model, ident)
    if not model:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail='There is no model on the provided ident.',
        )
    return model


def _build_select_sentence(obj, condition, offset=-1, limit=-1, order_by=None):
    base = select(obj).where(condition)
    if order_by is not None:
        base = base.order_by(order_by)
    if offset != -1:
        base = base.offset(offset)
    if limit != -1:
        base = base.limit(limit)
    return base


async def select_model(session: AsyncSession, obj: V, condition, offset=-1, limit=-1, order_by=None) -> V:
    sentence = _build_select_sentence(obj, condition, offset, limit, order_by)
    model = await session.scalar(sentence)
    return model


async def select_models(session: AsyncSession, obj: V, condition, offset=-1, limit=-1, order_by=None) -> ScalarResult[V]:
    sentence = _build_select_sentence(obj, condition, offset, limit, order_by)
    model = await session.scalars(sentence)
    return model


async def partial_update(session: AsyncSession, item: V, updates) -> V:
    update_data = updates.dict(exclude_unset=True)
    for key, value in update_data.items():
        setattr(item, key, value)
    await session.commit()
    await session.refresh(item)
    return item

