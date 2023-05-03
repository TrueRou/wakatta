<template>
    <div class="m-5">
        <el-page-header @back="clickBack">
            <template #content>
                <div class="flex items-center">
                    <el-icon class="mr-2">
                        <i class="fa fa-dashboard"></i>
                    </el-icon>
                    <span class="text-large font-600 mr-3 font-bold"> 日程管理 - {{ scheduleData.label }} </span>
                    <span class="text-xs mr-2" style="color: var(--el-text-color-regular)">
                        管理日程表和订阅
                    </span>
                    <el-tag type="success">拥有权限</el-tag>
                </div>
            </template>
        </el-page-header>
        <div class="m-5">
            <el-row :gutter="20">
                <el-col :span="12">
                    <el-descriptions border="true" direction="horizontal" column="1" title="日程信息">
                        <el-descriptions-item label="名称">{{ scheduleData.label }}</el-descriptions-item>
                        <el-descriptions-item label="ID">{{ scheduleData.id }}</el-descriptions-item>
                        <el-descriptions-item label="总课程数">{{ scheduleData.classes != null ? scheduleData.classes.length : 0
                        }}</el-descriptions-item>
                    </el-descriptions>
                </el-col>
                <el-col :span="6" :offset="6">
                    <el-descriptions direction="horizontal" title="日程操作" />
                    <div class="flex flex-col">
                        <div class="flex">
                            <el-button class="flex" type="primary">刷新</el-button>
                            <el-button class="flex" type="danger">删除</el-button>
                        </div>
                    </div>
                </el-col>
            </el-row>
            <div class="mt-2 w-full">
                <el-tabs>
                    <el-tab-pane label="课程">

                    </el-tab-pane>
                    <el-tab-pane label="订阅者">

                    </el-tab-pane>
                    <el-tab-pane label="设置">

                    </el-tab-pane>
                </el-tabs>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import config from '../config'
import axios from 'axios';
import { useUserStore } from '../stores/UserStore';

const scheduleData = ref({})

const router = useRouter()
const userStore = useUserStore()

const clickBack = () => router.back()

const refreshSchedule = async () =>
{
    let params = router.currentRoute.value.params
    const response = await axios.get(config.API_SCHEDULE + `?schedule_id=${params.id}`, userStore.getAuthorizedHeader())
    scheduleData.value = response.data
}

onMounted(async () => await refreshSchedule())

</script>