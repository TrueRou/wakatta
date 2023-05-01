<template>
    <el-page-header icon="" title="">
        <template #content>
            <div class="flex items-center">
                <el-avatar class="mr-3" :size="32"
                    src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
                <span class="text-large font-600 mr-3"> Title </span>
                <span class="text-sm mr-2" style="color: var(--el-text-color-regular)">
                    Sub title
                </span>
                <el-tag>Default</el-tag>
            </div>
        </template>
        <template #extra>
            <div class="flex items-center">
                <el-button type="primary">登录</el-button>
            </div>
        </template>
    </el-page-header>
</template>