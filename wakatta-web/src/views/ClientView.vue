<template>
    <div class="m-5">
        <el-page-header @back="clickBack">
            <template #content>
                <div class="flex items-center">
                    <el-icon class="mr-2">
                        <i class="fa fa-dashboard"></i>
                    </el-icon>
                    <span class="text-large font-600 mr-3 font-bold"> 会议管理 - {{ clientData.identifier }} </span>
                    <span class="text-xs mr-2" style="color: var(--el-text-color-regular)">
                        管理会议的课程和订阅
                    </span>
                    <el-tag type="success">拥有权限</el-tag>
                </div>
            </template>
        </el-page-header>
        <div class="m-5">
            <el-row :gutter="20">
                <el-col :span="12">
                    <el-descriptions :border="true" direction="horizontal" :column="1" title="客户端信息">
                        <el-descriptions-item label="名称">{{ clientData.identifier }}</el-descriptions-item>
                        <el-descriptions-item label="ID">{{ clientData.id }}</el-descriptions-item>
                        <el-descriptions-item label="订阅日程表">{{ subscriptionLabel }}</el-descriptions-item>
                        <el-descriptions-item label="状态">
                            <el-tag size="small" class="mr-2" type="success">在线</el-tag>
                            <el-tag size="small">打铃中</el-tag>
                        </el-descriptions-item>
                    </el-descriptions>
                </el-col>
                <el-col :span="6" :offset="6">
                    <el-descriptions direction="horizontal" title="客户端操作" />
                    <div class="flex flex-col">
                        <div class="flex">
                            <el-button class="flex" type="primary">刷新</el-button>
                            <el-button class="flex" type="danger">删除</el-button>
                        </div>
                    </div>
                </el-col>
            </el-row>
        </div>
        <div class="m-5 w-full">
            <el-tabs>
                <el-tab-pane label="总览">
                    <el-descriptions class="w-1/2" direction="horizontal" :column="1">
                        <el-descriptions-item label="名称">{{ clientData.identifier }}</el-descriptions-item>
                        <el-descriptions-item label="ID">{{ clientData.id }}</el-descriptions-item>
                        <el-descriptions-item label="Hardware ID">{{ clientData.hardware_id }}</el-descriptions-item>
                        <el-descriptions-item label="客户端版本">
                            {{ clientData.version }}
                        </el-descriptions-item>
                        <el-descriptions-item label="上课铃声">
                            {{ clientData.class_begin_ringtone_filename }}
                        </el-descriptions-item>
                        <el-descriptions-item label="下课铃声">
                            {{ clientData.class_over_ringtone_filename }}
                        </el-descriptions-item>
                        <el-descriptions-item label="订阅日程表">
                            {{ clientData.subscribe_schedule_id == null ? "未订阅" : subscriptionLabel +
                                `(ID: ${clientData.subscribe_schedule_id})` }}
                        </el-descriptions-item>
                    </el-descriptions>
                </el-tab-pane>
                <el-tab-pane label="课程" class="mr-8">
                    <div class="flex mb-3">
                        <el-button type="primary" @click="createClass()">新建</el-button>
                        <el-button type="danger" class="ml-2">全部删除</el-button>
                    </div>
                    <el-table class="w-full" :border="true" :data="clientData.classes">
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
                <el-tab-pane label="订阅">
                    <div class="flex mb-3">
                        <el-button type="primary" @click="editSubscription()">修改订阅</el-button>
                        <el-button @click="jumpSubscription()">管理订阅</el-button>
                        <el-button type="danger" @click="removeSubscription()">取消订阅</el-button>
                    </div>
                    <el-table class="w-full" :border="true" :data="clientData.subscribe_schedule?.classes">
                        <el-table-column type="index" label="序号" width="80" />
                        <el-table-column prop="label" label="课程" width="100" />
                        <el-table-column label="星期" width="100">
                            <template #default="scope">
                                {{ getChineseWeekday(scope.row.weekday) }}
                            </template>
                        </el-table-column>
                        <el-table-column prop="name" label="时间" />
                    </el-table>
                </el-tab-pane>
                <el-tab-pane label="提醒"></el-tab-pane>
                <el-tab-pane label="设置">
                    <div class=" mr-10">
                        <div class="flex mb-3">
                            <el-button type="primary" @click="editClientSubmit()">保存修改</el-button>
                        </div>
                        <el-form :model="clientDataCopy" label-width="100px" label-position="left">
                            <el-form-item label="客户端名称" prop="label">
                                <el-input v-model="clientDataCopy.identifier" />
                            </el-form-item>
                            <el-form-item label="上课铃声" prop="time">
                                <el-select class="w-full" v-model="clientDataCopy.class_begin_ringtone_filename">
                                    <el-option v-for="value in config.AVAILABLE_RINGTONES" :value="value"></el-option>
                                </el-select>
                            </el-form-item>
                            <el-form-item label="下课铃声" prop="duration">
                                <el-select class="w-full" v-model="clientDataCopy.class_over_ringtone_filename">
                                    <el-option v-for="value in config.AVAILABLE_RINGTONES" :value="value"></el-option>
                                </el-select>
                            </el-form-item>
                            <el-form-item label="VITS语音模型" prop="vits">
                                <el-select filterable class="w-full" v-model="clientData.vits_id">
                                    <el-option v-for="vits in dataStore.vits_characters" :value="vits.id"
                                        :label="vits.name"></el-option>
                                </el-select>
                            </el-form-item>
                        </el-form>
                    </div>
                </el-tab-pane>
            </el-tabs>
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
        </el-form>
        <template #footer>
            <span>
                <el-button type="primary" @click="editClassSubmit()">
                    确定
                </el-button>
            </span>
        </template>
    </el-dialog>
    <el-dialog width="500" title="修改订阅" v-model="dialogSubscription">
        <el-form :model="clientData" label-width="100px" label-position="left" ref="formSubscription">
            <el-form-item label="日程表" prop="schedule">
                <el-select v-model="clientData.subscribe_schedule_id">
                    <el-option v-for="schedule in dataStore.schedules" :value="schedule.id"
                        :label="schedule.label"></el-option>
                </el-select>
            </el-form-item>
        </el-form>
        <template #footer>
            <span>
                <el-button type="primary" @click="editClientSubmit()">
                    确定
                </el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script setup>
import { onMounted, ref, computed } from 'vue'
import { useUserStore } from '../stores/UserStore'
import { useRouter } from 'vue-router'
import axios from 'axios'
import config from '../config'
import { useDataStore } from '../stores/DataStore'
import { ElNotification } from 'element-plus'

const clientData = ref({})
const clientDataCopy = ref({})
const dialogClassEditing = ref(false)
const dataClassEditing = ref({})
const dialogClassCreating = ref(false)
const dataClassCreating = ref({})
const formClassCreating = ref()
const formClassEditing = ref()
const dialogSubscription = ref(false)
const formSubscription = ref()

const userStore = useUserStore()
const dataStore = useDataStore()
const router = useRouter()

const formRules = {
    label: [{ required: true, message: '请填写课程名称', trigger: 'blur' },],
}

const clickBack = () => router.back()

const refreshClient = async () =>
{
    let params = router.currentRoute.value.params
    const response = await axios.get(config.API_CLIENT + `?client_id=${params.id}`, userStore.getAuthorizedHeader())
    clientData.value = response.data
    clientDataCopy.value = {
        "identifier": clientData.value.identifier,
        "class_begin_ringtone_filename": clientData.value.class_begin_ringtone_filename,
        "class_over_ringtone_filename": clientData.value.class_over_ringtone_filename
    }
}

const editClass = (clazz) =>
{
    dialogClassEditing.value = true
    dataClassEditing.value = clazz
}

const editSubscription = () =>
{
    dialogSubscription.value = true
}

const createClass = () =>
{
    dialogClassCreating.value = true;
    dataClassCreating.value = {}
}

const createClassSubmit = async () =>
{
    await formClassCreating.value.validate(async (valid) =>
    {
        if (valid)
        {
            await axios.post(config.API_CLIENT_CLASS + `?client_id=${clientData.value.id}`, dataClassCreating.value, userStore.getAuthorizedHeader())
            dialogClassCreating.value = false;
            await refreshClient()
            ElNotification({
                title: '提示',
                message: '创建成功',
                type: 'success',
            })
        }
    })
}

const editClientSubmit = async () =>
{
    const data = {
        "subscribe_schedule_id": clientData.value.subscribe_schedule_id,
        "identifier": clientDataCopy.value.identifier,
        "class_begin_ringtone_filename": clientDataCopy.value.class_begin_ringtone_filename,
        "class_over_ringtone_filename": clientDataCopy.value.class_over_ringtone_filename,
        "vits_id": clientData.value.vits_id
    }
    await axios.patch(config.API_CLIENT + `?client_id=${clientData.value.id}`, data, userStore.getAuthorizedHeader())
    dialogSubscription.value = false;
    await refreshClient()
    await dataStore.fetchData()
    ElNotification({
        title: '提示',
        message: '修改成功',
        type: 'success',
    })

}

function getClassTime(hour, minute, duration)
{
    const endMinute = (minute + duration) % 60;
    const endHour = hour + Math.floor((minute + duration) / 60);
    const start = `${hour.toString().padStart(2, '0')}:${minute.toString().padStart(2, '0')}`;
    const end = `${endHour.toString().padStart(2, '0')}:${endMinute.toString().padStart(2, '0')}`;
    return `${start} - ${end}`;
}

function getChineseWeekday(weekday)
{
    const weekdays = ['日', '一', '二', '三', '四', '五', '六'];
    return "星期" + weekdays[weekday];
}

const editClassSubmit = async () =>
{
    await formClassEditing.value.validate(async (valid, fields) =>
    {
        if (valid)
        {
            await axios.patch(config.API_CLIENT_CLASS + `?class_id=${dataClassEditing.value.id}`, dataClassEditing.value, userStore.getAuthorizedHeader())
            dialogClassEditing.value = false;
            await refreshClient()
            ElNotification({
                title: '提示',
                message: '修改成功',
                type: 'success',
            })
        }
    })
}

const removeClass = async (id) =>
{
    await axios.delete(config.API_CLIENT_CLASS + `?class_id=${id}`, userStore.getAuthorizedHeader())
    await refreshClient()
    ElNotification({
        title: '提示',
        message: '删除成功',
        type: 'success',
    })
}

const removeSubscription = async () =>
{
    await axios.delete(config.API_CLIENT_SUBSCRIPTION + `?client_id=${clientData.value.id}`, userStore.getAuthorizedHeader())
    await refreshClient()
    ElNotification({
        title: '提示',
        message: '取消成功',
        type: 'success',
    })
}

const jumpSubscription = async () =>
{
    if (clientData.value.subscribe_schedule != null)
    {
        router.push(`/schedule/${clientData.value.subscribe_schedule.id}`)
    }
}

const subscriptionLabel = computed(() =>
{
    if (clientData.value.subscribe_schedule != null)
    {
        return clientData.value.subscribe_schedule.label
    } else
    {
        return "未订阅"
    }
})

onMounted(async () => await refreshClient())

</script>