const APIEntry = 'http://localhost:8000/'
const API_CLIENT = APIEntry + 'client'
const API_CLIENT_ALL = APIEntry + 'client/all'
const API_CLIENT_CLASS = APIEntry + 'client/class'
const API_CLIENT_SUBSCRIPTION = APIEntry + "client/subscription"
const API_SCHEDULE = APIEntry + 'schedule'
const API_SCHEDULE_ALL = APIEntry + 'schedule/all'
const API_SCHEDULE_CLASS = APIEntry + 'schedule/class'
const API_STATISTICS = APIEntry + 'statistics'
const API_USER = APIEntry + 'users'
const API_USER_ALL = APIEntry + 'statistics/users'
const API_USER_LOGIN = APIEntry + 'auth/jwt/login'
const API_USER_REGISTER = APIEntry + 'auth/register'
const API_USER_ME = APIEntry + 'users/me'



export default { API_USER_LOGIN, API_CLIENT_SUBSCRIPTION, API_SCHEDULE_CLASS, API_SCHEDULE_ALL, API_CLIENT_CLASS, API_USER_REGISTER, API_CLIENT_ALL, API_CLIENT, API_SCHEDULE, API_STATISTICS, API_USER_ME, API_USER, API_USER_ALL }