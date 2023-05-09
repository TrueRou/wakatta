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
                    <el-descriptions :border="true" direction="horizontal" :column="1" title="日程信息">
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
                            <el-select class=" w-28" v-model="filterDay" @change="refreshClasses">
                                <el-option v-for="weekday in weekdayList" :label="weekday.label" :value="weekday.value">
                                </el-option>
                            </el-select>
                            <el-button type="primary" class="ml-2" @click="createClass()">新建</el-button>
                            <el-button type="danger" class="ml-2">全部删除</el-button>
                        </div>
                        <el-table class="w-full" :border="true" :data="currentClasses">
                            <el-table-column type="index" label="序号" width="80" />
                            <el-table-column prop="label" label="课程" width="100" />
                            <el-table-column prop="name" label="时间">
                                <template #default="scope">
                                    {{ getClassTime(scope.row.time_hour, scope.row.time_minute, scope.row.time_duration) }}
                                </template>
                            </el-table-column>
                            <el-table-column label="操作" width="240">
                                <template #default="scope">
                                    <el-button type="primary" @click="editClass(scope.row)">编辑</el-button>
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
    <el-dialog width="500" title="创建课程" v-model="dialogClassCreating">
        <el-form :model="dataClassCreating" :rules="formRules" label-width="100px" label-position="left"
            ref="formClassCreating">
            <el-form-item label="课程名称" prop="label">
                <el-input placeholder="数学" v-model="dataClassCreating.label" />
            </el-form-item>
            <el-form-item label="时间" prop="time">
                <el-input-number placeholder="6" v-model="dataClassCreating.time_hour" :min="0" :max="24" />
                <span class="text-lg ml-2 mr-2"> : </span>
                <el-input-number placeholder="40" v-model="dataClassCreating.time_minute" :min="0" :max="60" />
            </el-form-item>
            <el-form-item label="时长" prop="duration">
                <el-input-number placeholder="60" v-model="dataClassCreating.time_duration" :min="1" />
            </el-form-item>
            <el-form-item label="星期" prop="weekday">
                <el-select class=" w-28" v-model="dataClassCreating.weekday">
                    <el-option v-for="weekday in weekdayList" :label="weekday.label" :value="weekday.value">
                    </el-option>
                </el-select>
            </el-form-item>
        </el-form>
        <template #footer>
            <span>
                <el-button type="primary" @click="createClassSubmit()">
                    确定
                </el-button>
            </span>
        </template>
    </el-dialog>
    <el-dialog width="500" title="编辑课程" v-model="dialogClassEditing">
        <el-form :model="dataClassEditing" :rules="formRules" label-width="100px" label-position="left"
            ref="formClassEditing">
            <el-form-item label="课程名称" prop="label">
                <el-input placeholder="数学" v-model="dataClassEditing.label" />
            </el-form-item>
            <el-form-item label="时间" prop="time">
                <el-input-number placeholder="6" v-model="dataClassEditing.time_hour" :min="0" :max="24" />
                <span class="text-lg ml-2 mr-2"> : </span>
                <el-input-number placeholder="40" v-model="dataClassEditing.time_minute" :min="0" :max="60" />
            </el-form-item>
            <el-form-item label="时长" prop="duration">
                <el-input-number placeholder="60" v-model="dataClassEditing.time_duration" :min="1" />
            </el-form-item>
            <el-form-item label="星期" prop="weekday">
                <el-select class=" w-28" v-model="dataClassEditing.weekday">
                    <el-option v-for="weekday in weekdayList" :label="weekday.label" :value="weekday.value">
                    </el-option>
                </el-select>
            </el-form-item>
        </el-form>
        <template #footer>
            <span>
                <el-button type="primary" @click="editClassSubmit()">
                    确定
                </el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import config from '../config'
import axios from 'axios';
import { useUserStore } from '../stores/UserStore';

const scheduleData = ref({})
const weekdayList = [
    {
        "label": "星期一",
        "value": "1",
    },
    {
        "label": "星期二",
        "value": "2",
    },
    {
        "label": "星期三",
        "value": "3",
    },
    {
        "label": "星期四",
        "value": "4",
    },
    {
        "label": "星期五",
        "value": "5",
    },
    {
        "label": "星期六",
        "value": "6",
    },
    {
        "label": "星期日",
        "value": "0",
    },
]
const filterDay = ref(new Date().getDay().toString())

const formRules = {
    label: [{ required: true, message: '请填写课程名称', trigger: 'blur' },],
}

const currentClasses = ref()

const dialogClassCreating = ref(false)
const dataClassCreating = ref({})
const formClassCreating = ref()
const dialogClassEditing = ref(false)
const dataClassEditing = ref({})
const formClassEditing = ref()

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

const createClass = () =>
{
    dialogClassCreating.value = true;
    dataClassCreating.value = { weekday: filterDay.value }
}

function getClassTime(hour, minute, duration)
{
    const endMinute = (minute + duration) % 60;
    const endHour = hour + Math.floor((minute + duration) / 60);
    const start = `${hour.toString().padStart(2, '0')}:${minute.toString().padStart(2, '0')}`;
    const end = `${endHour.toString().padStart(2, '0')}:${endMinute.toString().padStart(2, '0')}`;
    return `${start} - ${end}`;
}

const createClassSubmit = async () =>
{
    await formClassCreating.value.validate(async (valid) =>
    {
        if (valid)
        {
            await axios.post(config.API_SCHEDULE_CLASS + `?schedule_id=${scheduleData.value.id}`, dataClassCreating.value, userStore.getAuthorizedHeader())
            dialogClassCreating.value = false;
            await refreshSchedule()
        }
    })
}

const editClassSubmit = async () =>
{
    await formClassCreating.value.validate(async (valid, fields) =>
    {
        if (valid)
        {
            await axios.patch(config.API_SCHEDULE_CLASS + `?schedule_id=${scheduleData.value.id}`, dataClassEditing.value, userStore.getAuthorizedHeader())
            dialogClassEditing.value = false;
        }
    })
}

const editClass = (clazz) =>
{
    dialogClassEditing.value = true
    dataClassEditing.value = clazz
}

const refreshClasses = async () =>
{
    const response = await axios.get(config.API_SCHEDULE_CLASS + `?schedule_id=${scheduleData.value.id}&weekday=${filterDay.value}`, userStore.getAuthorizedHeader())
    currentClasses.value = response.data
}

const removeClass = async (id) =>
{
    await axios.delete(config.API_SCHEDULE_CLASS + `?class_id=${id}`, userStore.getAuthorizedHeader())
    await refreshSchedule()
}

onMounted(async () => await refreshSchedule())

</script>