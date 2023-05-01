import { defineStore } from 'pinia'
import { ref } from 'vue'
import axios from 'axios'
import config from '../config'
import { ElNotification } from 'element-plus'

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
      try
      {
        const response = await axios.get(config.API_USER_ME, getAuthorizedHeader())
        this.userInfo = response.data
      } catch {
        localStorage.removeItem('token')
        ElNotification({
          title: '登录已失效',
          message: '因为Token过期, 你的登录已失效, 请重新登录',
          type: 'warning',
        })
      }
    }
  }

  function getAuthorizedHeader()
  {
    const token = localStorage.getItem('token')
    return { headers: { "Authorization": `Bearer ${token}` } }
  }

  return { userInfo, fetchUser, isLogged, getAuthorizedHeader }
})