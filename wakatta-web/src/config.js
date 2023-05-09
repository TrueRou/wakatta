const APIEntry = 'http://localhost:8000/'
const API_CLIENT = APIEntry + 'client'
const API_CLIENT_ALL = APIEntry + 'client/all'
const API_CLIENT_CLASS = APIEntry + 'client/class'
const API_CLIENT_SUBSCRIPTION = APIEntry + "client/subscription"
const API_SCHEDULE = APIEntry + 'schedule'
const API_SCHEDULE_ALL = APIEntry + 'schedule/all'
const API_SCHEDULE_CLASS = APIEntry + 'schedule/class'
const API_SCHEDULE_SUBSCRIBER = APIEntry + 'schedule/subscriber'
const API_STATISTICS = APIEntry + 'statistics'
const API_USER = APIEntry + 'users'
const API_USER_ALL = APIEntry + 'statistics/users'
const API_USER_LOGIN = APIEntry + 'auth/jwt/login'
const API_USER_REGISTER = APIEntry + 'auth/register'
const API_USER_ME = APIEntry + 'users/me'

const AVAILABLE_RINGTONES = [
    'default_class_begin.wav',
    'default_class_over.wav',
    'Cancan.wav',
    'Inazuma 稻妻.wav',
    'Mountain Journey.wav',
    'SpongeBob Theme Song.wav',
    'Time to Say Farewell.wav',
    'To Victory.wav',
    '原始の森.wav',
    '孤独月.wav',
    '暖かな日々の中で.wav',
    '洄映的漩流.wav',
]



export default { AVAILABLE_RINGTONES, API_SCHEDULE_SUBSCRIBER, API_USER_LOGIN, API_CLIENT_SUBSCRIPTION, API_SCHEDULE_CLASS, API_SCHEDULE_ALL, API_CLIENT_CLASS, API_USER_REGISTER, API_CLIENT_ALL, API_CLIENT, API_SCHEDULE, API_STATISTICS, API_USER_ME, API_USER, API_USER_ALL }