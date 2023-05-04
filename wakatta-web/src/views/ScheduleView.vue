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
                        <div class="flex mb-3">
                            <el-select v-model="filterDay" size="large" @change="refreshClasses">
                                <el-option label="星期一" value="1"></el-option>
                                <el-option label="星期二" value="2"></el-option>
                                <el-option label="星期三" value="3"></el-option>
                                <el-option label="星期四" value="4"></el-option>
                                <el-option label="星期五" value="5"></el-option>
                                <el-option label="星期六" value="6"></el-option>
                                <el-option label="星期日" value="0"></el-option>
                            </el-select>
                            <el-button type="primary" @click="createClass()">新建</el-button>
                            <el-button type="danger" class="ml-2">全部删除</el-button>
                        </div>
                        <el-table class="w-full" border :data="clientData.classes">
                            <el-table-column prop="id" label="ID" width="80" />
                            <el-table-column prop="label" label="课程" width="100" />
                            <el-table-column prop="name" label="时间" />
                            <el-table-column label="操作" width="240">
                                <template #default="scope">
                                    <el-button type="primary" @click="editClass(scope.row)">编辑</el-button>
                                    <el-button type="info" @click="editClass(scope.row)">复制</el-button>
                                    <el-button type="danger" @click="removeClass(scope.row.id)">删除</el-button>
                                </template>
                            </el-table-column>
                        </el-table>
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
const currentClasses = ref({})
const filterDay = ref(new Date().getDay())

const router = useRouter()
const userStore = useUserStore()

const clickBack = () => router.back()

const refreshSchedule = async () =>
{
    let params = router.currentRoute.value.params
    const response = await axios.get(config.API_SCHEDULE + `?schedule_id=${params.id}`, userStore.getAuthorizedHeader())
    scheduleData.value = response.data
    await refreshClasses()
}

const refreshClasses = async () =>
{
    const response = await axios.get(config.API_SCHEDULE_CLASS + `?schedule_id=${scheduleData.value.id}&weekday=${filterDay}`, userStore.getAuthorizedHeader())
    currentClasses.value = response.data
}

const createClass = async () =>
{

}

const editClass = async () =>
{

}

const removeClass = async () =>
{

}

onMounted(async () => await refreshSchedule())

</script>