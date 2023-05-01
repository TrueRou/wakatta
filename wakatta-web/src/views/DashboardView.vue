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

const logged = computed(() =>
{
    return userStore.isLogged()
})

const clickBack = () =>
{
    router.back()
}

</script>