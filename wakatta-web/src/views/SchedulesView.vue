<template>
    <div class="m-5">
        <el-page-header @back="clickBack">
            <template #content>
                <div class="flex items-center">
                    <el-icon class="mr-2">
                        <i class="fa fa-dashboard"></i>
                    </el-icon>
                    <span class="text-large font-600 mr-3 font-bold"> 日程管理 </span>
                    <span class="text-xs mr-2" style="color: var(--el-text-color-regular)">
                        管理日程表和订阅
                    </span>
                    <el-tag type="success">拥有权限</el-tag>
                </div>
            </template>
        </el-page-header>
    </div>
    <div class="m-5">
        <div class="flex mb-3">
            <el-button type="primary" @click="createSchedule">新建日程</el-button>
            <el-button @click="dataStore.refreshSchedules()">刷新</el-button>
        </div>
        <el-table class="w-full" border :data="dataStore.schedules">
            <el-table-column prop="id" label="ID" width="80" />
            <el-table-column prop="label" label="名称" width="300" />
            <el-table-column label="操作">
                <template #default="scope">
                    <el-button type="primary" @click="editSchedule(scope.row.id)">编辑</el-button>
                    <el-button type="info" @click="editSchedule(scope.row.id)">复制</el-button>
                    <el-button type="danger" @click="removeSchedule(scope.row.id)">删除</el-button>
                </template>
            </el-table-column>
        </el-table>
    </div>
    <el-dialog width="500" title="创建日程" v-model="dialogScheduleCreating">
        <el-form :model="dataScheduleCreating" :rules="formRules" label-width="100px" label-position="left"
            ref="formScheduleCreating">
            <el-form-item label="名称" prop="label">
                <el-input placeholder="三年A班课程表" v-model="dataScheduleCreating.label" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span>
                <el-button type="primary" @click="createScheduleSubmit()">
                    确定
                </el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script setup>
import { ref } from 'vue'
import { useDataStore } from '../stores/DataStore'
import { useRouter } from 'vue-router'
import axios from 'axios'
import config from '../config'
import { useUserStore } from '../stores/UserStore';

const dialogScheduleCreating = ref(false)
const dataScheduleCreating = ref(false)
const formScheduleCreating = ref()

const dataStore = useDataStore()
const userStore = useUserStore()
const router = useRouter()

const formRules = {
    label: [{ required: true, message: '请填写日程名称', trigger: 'blur' },],
}

const clickBack = () => router.back()

const editSchedule = (id) =>
{
    router.push(`/schedule/${id}`)
}

const removeSchedule = async (id) =>
{
    await axios.delete(config.API_SCHEDULE + `?schedule_id=${id}`, userStore.getAuthorizedHeader())
    dataStore.refreshSchedules()
}

const createSchedule = () =>
{
    dialogScheduleCreating.value = true;
    dataScheduleCreating.value = {}
}

const createScheduleSubmit = async () =>
{
    await formScheduleCreating.value.validate(async (valid) =>
    {
        if (valid)
        {
            await axios.post(config.API_SCHEDULE, dataScheduleCreating.value, userStore.getAuthorizedHeader())
            dialogScheduleCreating.value = false;
            await dataStore.refreshSchedules()
        }
    })
}
</script>