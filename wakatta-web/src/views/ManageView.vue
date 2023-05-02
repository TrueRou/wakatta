<template>
  <div class="m-5">
    <el-page-header @back="clickBack">
      <template #content>
        <div class="flex items-center">
          <el-icon class="mr-2">
            <i class="fa fa-dashboard"></i>
          </el-icon>
          <span class="text-large font-600 mr-3 font-bold"> 用户管理 </span>
          <span class="text-xs mr-2" style="color: var(--el-text-color-regular)">
            管理用户的访问权限和特殊权限
          </span>
          <el-tag type="success">拥有权限</el-tag>
        </div>
      </template>
    </el-page-header>
    <div class="justify-center flex mt-5">
      <el-container style="display: flex;">
        <el-table :data="userList" :border="true" :default-sort="{ prop: 'date', order: 'descending' }"
          style="width: 100%; display: flex; justify-content: center;">
          <el-table-column prop="id" label="id" width="310" />
          <el-table-column prop="nickname" label="用户名" width="150">
            <template #default="scope">
              <el-input v-model="scope.row.nickname"></el-input>
            </template>
          </el-table-column>
          <el-table-column prop="email" label="邮箱">
            <template #default="scope">
              <el-input v-model="scope.row.email"></el-input>
            </template>
          </el-table-column>
          <el-table-column width="110" prop="privilege" label="面板访问权限">
            <template #default="scope">
              <el-switch v-model="scope.row.privilege" />
            </template>
          </el-table-column>
          <el-table-column width="110" prop="is_superuser" label="用户管理权限">
            <template #default="scope">
              <el-switch v-model="scope.row.is_superuser" />
            </template>
          </el-table-column>
          <el-table-column label="操作" width="160">
            <template #default="scope">
              <el-button type="primary" @click="handleEdit(scope.row)">保存</el-button>
              <el-button type="danger" @click="handleDelete(scope.row)">删除</el-button>
            </template>
          </el-table-column>
        </el-table>
      </el-container>
    </div>
  </div>
</template>
<script setup>
import config from '../config'
import { onMounted, ref } from 'vue'
import axios from 'axios';
import { useRouter } from 'vue-router'

const router = useRouter()
const userList = ref([])
onMounted(() =>
{
  getAllUser()
})
const getAllUser = async () =>
{
  const token = localStorage.getItem('token')
  const response = await axios.get(config.API_USER_ALL, { headers: { "Authorization": `Bearer ${token}` } })
  userList.value = response.data
}
const handleEdit = async (object) =>
{
  const token = localStorage.getItem('token')
  const options = { headers: { "Authorization": `Bearer ${token}` } }
  const id = object.id
  await axios.patch(config.API_USER + `/${id}`, object, options)
  await getAllUser()
}
const handleDelete = async (object) =>
{
  const token = localStorage.getItem('token')
  const options = { headers: { "Authorization": `Bearer ${token}` } }
  const id = object.id
  await axios.delete(config.API_USER + `/${id}`, options)
  await getAllUser()
}
const clickBack = () =>
{
  router.back()
}
</script>
