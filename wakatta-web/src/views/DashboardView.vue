<template>
    <div class="m-5">
        <el-page-header @back="clickBack">
            <template #content>
                <div class="flex items-center">
                    <el-icon class="mr-2">
                        <i class="fa fa-dashboard"></i>
                    </el-icon>
                    <span class="text-large font-600 mr-3 font-bold"> 仪表盘 </span>
                    <span class="text-xs mr-2" style="color: var(--el-text-color-regular)">
                        展示客户端、日程表和用户的相关信息和状态
                    </span>
                    <el-tag type="success">拥有权限</el-tag>
                </div>
            </template>
        </el-page-header>
        <el-row class="m-20 mt-8" justify="space-evenly">
            <el-col :span="4">
                <el-statistic title="总会议数" :value="dashboard_statistics.client_count" />
            </el-col>
            <el-col :span="4">
                <el-statistic title="总课程数" :value="dashboard_statistics.class_count" />
            </el-col>
            <el-col :span="4">
                <el-statistic title="总用户数" :value="dashboard_statistics.user_count" />
            </el-col>
        </el-row>
        <el-row class="m-20 mt-8" justify="space-evenly">
            <el-col :span="4">
                <el-statistic title="在线会议数" :value="dashboard_statistics.online_users_count" />
            </el-col>
            <el-col :span="4">
                <el-statistic title="总日程表数" :value="dashboard_statistics.schedule_count" />
            </el-col>
            <el-col :span="4">
                <el-statistic title="日程表课程数" :value="dashboard_statistics.schedule_class_count" />
            </el-col>
        </el-row>
    </div>
</template>

<script setup>
import { onMounted, ref, computed } from 'vue'
import { useUserStore } from '../stores/UserStore'
import { useDataStore } from '../stores/DataStore'
import { useRouter } from 'vue-router'

const userStore = useUserStore()
const dataStore = useDataStore()
const router = useRouter()

const dashboard_statistics = ref({})

const clickBack = () =>
{
    router.back()
}

onMounted(async () =>
{
    dataStore.fetchData()
    dashboard_statistics.value = dataStore.dashboard_statistics
    console.log(dataStore.dashboard_statistics)
})

</script>