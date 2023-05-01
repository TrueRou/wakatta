<script setup>
import { RouterView } from 'vue-router'

onMounted(() => {
  
}),
</script>

<template>
  <el-container class="h-full">
    <el-aside class="h-full">
      <el-scrollbar class="h-full">
        <el-menu class="h-full" default-active="@dashboard" :default-openeds="['@clients']">
          <el-menu-item index="dashboard" route="/dashboard">
            <el-icon>
              <i class="fa fa-dashboard"></i>
            </el-icon>
            仪表盘
          </el-menu-item>
          <el-sub-menu index="client">
            <template #title>
              <el-icon>
                <i class="fa fa-users"></i>
              </el-icon>
              客户端管理
            </template>
            
            <el-menu-item v-for="client in " index="dfdsf">Client1</el-menu-item>
            <el-menu-item index="2ff">Client2</el-menu-item>
          </el-sub-menu>
          <el-sub-menu index="@schedules">
            <template #title>
              <el-icon>
                <i class="fa fa-calendar"></i>
              </el-icon>
              日程表管理
            </template>
            <el-menu-item index="22ff">Schedule1</el-menu-item>
            <el-menu-item index="22fff">Schedule2</el-menu-item>
            <el-menu-item index="22fffdf">Schedule3</el-menu-item>
            <el-menu-item index="22ffsdf">Schedule4</el-menu-item>
          </el-sub-menu>
        </el-menu>
      </el-scrollbar>
    </el-aside>
    <el-main class="m-5">
      <RouterView />
    </el-main>
  </el-container>
</template>

<style>
.el-scrollbar__view {
  height: 100%;
}
</style>


