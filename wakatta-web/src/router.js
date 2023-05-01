import { createRouter, createWebHistory } from 'vue-router'
import { useUserStore } from "./stores/UserStore.js"
import DashboardView from './views/DashboardView.vue'
import LoginView from './views/LoginView.vue'
import RegisterView from './views/RegisterView.vue'
import DeniedView from './views/DeniedView.vue'
import ManageView from './views/ManageView.vue'
import ClientView from './views/ClientView.vue'
import ScheduleView from './views/ScheduleView.vue'
import SchedulesView from './views/SchedulesView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Dashboard',
      component: DashboardView
    },
    {
      path: '/login',
      name: 'Login',
      component: LoginView
    },
    {
      path: '/logout',
      name: 'Logout',
    },
    {
      path: '/register',
      name: 'Register',
      component: RegisterView
    },
    {
      path: '/denied',
      name: 'Denied',
      component: DeniedView
    },
    {
      path: '/manage',
      name: 'Manage',
      component: ManageView
    },
    {
      path: '/client/:id',
      name: 'Client',
      component: ClientView
    },
    {
      path: '/schedule/:id',
      name: 'Schedule',
      component: ScheduleView
    },
    {
      path: '/schedules/:id',
      name: 'Schedule',
      component: SchedulesView
    },
  ]
})

router.beforeEach(async (to, from, next) =>
{
  const userStore = useUserStore()
  await userStore.fetchUser()

  if (to.path == "/login" || to.path == "/register")
  {
    next()
    return
  }

  if (to.path == '/logout')
  {
    localStorage.removeItem('token')
    location.href = '/'
    return
  }

  if (!userStore.isLogged())
  {
    next("/login")
    return
  }

  if (userStore.userInfo.privilege != 1 && to.path != "/denied")
  {
    next("/denied")
    return
  }

  if (userStore.userInfo.privilege == 1 && to.path == "/denied")
  {
    next("/")
    return
  }

  next()
})

export default router
