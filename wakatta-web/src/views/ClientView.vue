<template>
    <div class="m-5">
        <el-page-header @back="clickBack">
            <template #content>
                <div class="flex items-center">
                    <el-icon class="mr-2">
                        <i class="fa fa-dashboard"></i>
                    </el-icon>
                    <span class="text-large font-600 mr-3 font-bold"> 客户端管理 - {{ clientData.identifier }} </span>
                    <span class="text-xs mr-2" style="color: var(--el-text-color-regular)">
                        编辑客户端信息和课程表
                    </span>
                    <el-tag type="success">拥有权限</el-tag>
                </div>
            </template>
        </el-page-header>
        <div class="m-5">
            <el-row :gutter="20">
                <el-col :span="12">
                    <el-descriptions border="true" direction="horizontal" column="1" title="客户端信息">
                        <el-descriptions-item label="名称">{{ clientData.identifier }}</el-descriptions-item>
                        <el-descriptions-item label="ID">{{ clientData.id }}</el-descriptions-item>
                        <el-descriptions-item
                            label="Hardware ID">f6d9b713-31a0-41bb-9360-927e11a81786</el-descriptions-item>
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
                        <div class="flex mt-5">
                            <el-button class="flex" type="success">手动打铃</el-button>
                        </div>
                        <div class="flex mt-5">
                            <el-button class="flex" type="warn">暂停打铃</el-button>
                        </div>
                    </div>
                </el-col>
            </el-row>
        </div>
        <div class="m-5 w-full">
            <el-tabs>
                <el-tab-pane label="总览">
                    <el-descriptions class="w-1/2" direction="horizontal" column="1">
                        <el-descriptions-item label="名称">{{ clientData.identifier }}</el-descriptions-item>
                        <el-descriptions-item label="ID">{{ clientData.id }}</el-descriptions-item>
                        <el-descriptions-item
                            label="Hardware ID">f6d9b713-31a0-41bb-9360-927e11a81786</el-descriptions-item>
                        <el-descriptions-item label="客户端版本">
                            Beta 1.0
                        </el-descriptions-item>
                        <el-descriptions-item label="会议号">
                            123456789
                        </el-descriptions-item>
                        <el-descriptions-item label="参会人数">
                            123
                        </el-descriptions-item>
                        <el-descriptions-item label="订阅日程表">
                            Schedule (123456789)
                        </el-descriptions-item>
                    </el-descriptions>
                </el-tab-pane>
                <el-tab-pane label="课程" class="mr-8">
                    <div class="flex mb-3">
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
                <el-tab-pane label="订阅">
                    <div class="flex mb-3">
                        <el-button type="primary">修改订阅</el-button>
                        <el-button>转到</el-button>
                    </div>
                    <el-table border :data="tableData" style="width: 100%">
                        <el-table-column prop="date" label="Date" width="180" />
                        <el-table-column prop="name" label="Name" width="180" />
                        <el-table-column prop="address" label="Address" />
                    </el-table>
                </el-tab-pane>
                <el-tab-pane label="提醒"></el-tab-pane>
                <el-tab-pane label="设置"></el-tab-pane>
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
</template>

<script setup>
import { onMounted, ref, computed } from 'vue'
import { useUserStore } from '../stores/UserStore'
import { useDataStore } from '../stores/DataStore'
import { useRouter } from 'vue-router'
import axios from 'axios'
import Qs from 'qs';
import config from '../config'

const clientData = ref({})
const dialogClassEditing = ref(false)
const dataClassEditing = ref(false)
const dialogClassCreating = ref(false)
const dataClassCreating = ref(false)
const formClassCreating = ref()
const formClassEditing = ref()

const userStore = useUserStore()
const dataStore = useDataStore()
const router = useRouter()

const formRules = {
    label: [{ required: true, message: '请填写课程名称', trigger: 'blur' },],
}

const logged = computed(() => userStore.isLogged())
const clickBack = () => router.back()

const refreshClient = async () =>
{
    let params = router.currentRoute.value.params
    const response = await axios.get(config.API_CLIENT + `?client_id=${params.id}`, userStore.getAuthorizedHeader())
    clientData.value = response.data
}

const editClass = (clazz) =>
{
    dialogClassEditing.value = true
    dataClassEditing.value = clazz
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
            const body = Qs.stringify(dataClassCreating)
            await axios.post(config.API_CLIENT_CLASS + `?client_id=${clientData.value.id}`, dataClassCreating.value, userStore.getAuthorizedHeader())
            dialogClassCreating.value = false;
            await refreshClient()
        }
    })
}

const editClassSubmit = async () =>
{
    await formClassCreating.value.validate((valid, fields) =>
    {
        if (valid)
        {
            dialogClassEditing.value = false;
        }
    })
}

const removeClass = (id) => dataStore.removeClass(id)

onMounted(async () => refreshClient())

</script>