<script setup>
import { RouterView } from 'vue-router'
import { onMounted, ref, computed } from 'vue'
import { useUserStore } from './stores/UserStore'
import { useDataStore } from './stores/DataStore'
import { useRouter } from 'vue-router'
const clients = ref([])
const userStore = useUserStore()
const dataStore = useDataStore()
const router = useRouter()

onMounted(async () =>
{
  await dataStore.fetchData()
})

const clickDashboard = () =>
{
  router.push('/')
}

const clickClient = (id) =>
{
  router.push(`/client/${id}`)
}

const clickManage = () =>
{
  router.push('/manage')
}

const clickSchedules = () =>
{
  router.push(`/schedules`)
}

const sideShown = computed(() =>
{
  if (!userStore.isLogged()) return false;
  if (router.currentRoute.value.href == "/denied") return false;
  return true;
})

</script>

<template>
  <el-container class="h-full">
    <el-aside v-if="sideShown" class="h-full">
      <el-scrollbar class="h-full">
        <el-menu class="h-full" default-active="@dashboard" active-text-color="#303133" :default-openeds="['@clients']">
          <el-menu-item index="@dashboard" @click="clickDashboard">
            <el-icon>
              <i class="fa fa-dashboard"></i>
            </el-icon>
            仪表盘
          </el-menu-item>
          <el-sub-menu index="@clients">
            <template #title>
              <el-icon>
                <i class="fa fa-users"></i>
              </el-icon>
              会议管理
            </template>
            <el-menu-item v-for="client in dataStore.clients" :index="'c' + client.id" @click="clickClient(client.id)">
              {{ client.identifier }}
            </el-menu-item>
          </el-sub-menu>
          <el-menu-item index="@schedules" @click="clickSchedules">
            <el-icon>
              <i class="fa fa-calendar"></i>
            </el-icon>
            日程管理
          </el-menu-item>
          <el-menu-item index="@manage" @click="clickManage">
            <el-icon>
              <i class="fa fa-user"></i>
            </el-icon>
            用户管理
          </el-menu-item>
        </el-menu>
      </el-scrollbar>
    </el-aside>
    <el-main style="padding: 0;">
      <RouterView :key="router.currentRoute.value.fullPath" />
    </el-main>
  </el-container>
</template>

<style>
.el-scrollbar__view {
  height: 100%;
}
</style>