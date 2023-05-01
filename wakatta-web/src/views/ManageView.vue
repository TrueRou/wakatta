<template>
  <div style="justify-content: center; display: flex;">
    <el-container style="display: flex;">
      <el-table :data="userList" :border="true" :default-sort="{ prop: 'date', order: 'descending' }"
        style="width: 100%; display: flex; justify-content: center;">
        <el-table-column prop="id" label="id" width="400" />
        <el-table-column prop="username" label="用户名" width="200">
          <template #default="scope">
            <el-input v-model="scope.row.username"></el-input>
          </template>
        </el-table-column>
        <el-table-column prop="email" label="邮箱">
          <template #default="scope">
            <el-input v-model="scope.row.email"></el-input>
          </template>
        </el-table-column>
        <el-table-column width="120" prop="is_superuser" label="超级用户">
          <template #default="scope">
            <el-switch v-model="scope.row.is_superuser" />
          </template>
        </el-table-column>
        <el-table-column width="120" prop="manage_available" label="管理会议">
          <template #default="scope">
            <el-switch v-model="scope.row.privilege" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="180">
          <template #default="scope">
            <el-button type="primary" @click="handleEdit(scope.row)">保存</el-button>
            <el-button type="danger" @click="handleDelete(scope.row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-container>
  </div>
</template>
<script setup>
import config from '../config'
import { onMounted, ref } from 'vue'
import axios from 'axios';
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
</script>
