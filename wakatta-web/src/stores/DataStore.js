import { defineStore } from 'pinia'
import { ref } from 'vue'
import { useUserStore } from './UserStore'
import axios from 'axios'
import config from '../config'

export const useDataStore = defineStore('data', () =>
{
    const userStore = useUserStore()

    const clients = ref([])
    const schedules = ref([])
    const online_users = ref([])
    const dashboard_statistics = ref([])


    async function fetchData()
    {
        if (localStorage.getItem('token') != null)
        {
            const response = await axios.get(config.API_STATISTICS, userStore.getAuthorizedHeader())
            const statistics = response.data
            clients.value = statistics.clients
            schedules.value = statistics.schedules
            online_users.value = statistics.online_users
            dashboard_statistics.value = statistics.dashboard_statistics
        }
    }

    async function refreshSchedules()
    {
        if (localStorage.getItem('token') != null)
        {
            const response = await axios.get(config.API_SCHEDULE_ALL, userStore.getAuthorizedHeader())
            schedules.value = response.data
        }
    }

    return { clients, schedules, online_users, dashboard_statistics, fetchData, refreshSchedules }
})