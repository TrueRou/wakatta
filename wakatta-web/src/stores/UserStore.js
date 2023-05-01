import { defineStore } from 'pinia'
import { ref } from 'vue'
import axios from 'axios'
import config from '../config'

export const useUserStore = defineStore('user', () =>
{
  const userInfo = ref({})

  function isLogged()
  {
    return localStorage.getItem('token') != null
  }

  async function fetchUser()
  {
    if (isLogged())
    {

      const response = await axios.get(config.API_USER_ME, getAuthorizedHeader())
      this.userInfo = response.data
    }
  }

  function getAuthorizedHeader()
  {
    const token = localStorage.getItem('token')
    return { headers: { "Authorization": `Bearer ${token}` } }
  }

  return { userInfo, fetchUser, isLogged, getAuthorizedHeader }
})