<template>
    <div class="m-5">
        <el-page-header @back="clickBack">
            <template #content>
                <div class="flex items-center">
                    <el-icon class="mr-2">
                        <i class="fa fa-dashboard"></i>
                    </el-icon>
                    <span class="text-large font-600 mr-3 font-bold"> 客户端管理 - {{ client.identifier }} </span>
                    <span class="text-xs mr-2" style="color: var(--el-text-color-regular)">
                        编辑客户端信息和课程表
                    </span>
                    <el-tag type="success">拥有权限</el-tag>
                </div>
            </template>
        </el-page-header>
    </div>
</template>

<script setup>
import { onMounted, ref, computed } from 'vue'
import { useUserStore } from '../stores/UserStore'
import { useDataStore } from '../stores/DataStore'
import { useRouter } from 'vue-router'
import axios from 'axios'
import config from '../config'

const client = ref({})

const userStore = useUserStore()
const dataStore = useDataStore()
const router = useRouter()

const logged = computed(() =>
{
    return userStore.isLogged()
})

const clickBack = () =>
{
    router.back()
}

onMounted(async () =>
{
    let params = router.currentRoute.value.params
    const response = await axios.get(config.API_CLIENT + `?client_id=${params.id}`, userStore.getAuthorizedHeader())
    client.value = response.data
})

</script>