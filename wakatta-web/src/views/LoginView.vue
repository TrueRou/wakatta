<template>
  <div class="body bg">
    <el-form class="login-form" label-position="top" label-width="100px" :rules="rules" :model="formContent" status-icon
      ref="loginForm">
      <el-form-item class="form-item" label="邮箱" prop="email">
        <el-input class="input-box" type="text" v-model="formContent.email"></el-input>
      </el-form-item>
      <el-form-item class="form-item" label="密码" prop="password">
        <el-input class="input-box" type="password" v-model="formContent.password"></el-input>
      </el-form-item>
      <el-form-item>
        <el-button style="width: 100%" type="primary" @click="submitForm()">登录</el-button>
      </el-form-item>
      <RouterLink to="/register"><span class=" text-white text-sm hover:text-blue-400">没有账号？</span>
      </RouterLink>
      <el-tag v-if="error" class="ml-2" type="danger">登录失败, 可能是邮箱或密码错误</el-tag>
    </el-form>
  </div>
</template>
<script setup>
import config from '../config'
import Qs from 'qs';
import { useUserStore } from '../stores/UserStore';
import axios from 'axios';
import { ref } from "vue";

const userStore = useUserStore()
const loginForm = ref()
const formContent = ref({ email: '', password: '' })

const rules = {
  email: [{ required: true, message: '请填写邮箱', trigger: 'blur' },],
  password: [{ required: true, message: '请填写密码', trigger: 'blur' }, {
    min: 5,
    max: 12,
    message: '密码的长度范围为5-12',
    trigger: 'blur'
  }]
}
let error = ref(false)


const submitForm = async () =>
{
  var params = Qs.stringify({
    'username': formContent.value.email,
    'password': formContent.value.password
  })
  loginForm.value.validate(async (valid) =>
  {
    if (valid)
    {
      try
      {
        const response = await axios.post(config.API_USER_LOGIN, params)
        localStorage.setItem('token', response.data['access_token']);
        await userStore.fetchUser()
        location.href = '/'
      } catch (e)
      {
        console.log(e.response.data.error)
        error = true
      }
    }
  });
}
</script>
<style>
.login-form {
  background: rgb(0, 0, 0, 0.5);
  opacity: 1;
  padding: 20px;
  border-radius: 5px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
}

.form-item .el-form-item__label {
  color: white;
}

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  font-family: 'Poppins', sans-serif;
}

.bg {
  background-size: cover;
  /*也许之后会改成随机url？*/
  background-image: url(https://api.likepoems.com/img/pc);
}

.body {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  flex-direction: column;
  gap: 30px;
}

.input-box .el-input__wrapper {
  background: transparent;
  width: 250px;
  border: gray;
}

.input-box .el-input__inner {
  color: white;
}
</style>


