<template>
  <div class="body bg">
    <el-form class="register-form" label-position="top" label-width="100px" :rules="rules" :model="formContent"
      status-icon ref="registerForm">
      <el-form-item class="form-item" label="用户名" prop="name">
        <el-input class="input-box" type="name" v-model="formContent.name"></el-input>
      </el-form-item>
      <el-form-item class="form-item" label="邮箱" prop="email">
        <el-input class="input-box" type="text" v-model="formContent.email"></el-input>
      </el-form-item>
      <el-form-item class="form-item" label="密码" prop="password">
        <el-input class="input-box" type="password" v-model="formContent.password"></el-input>
      </el-form-item>
      <el-form-item>
        <el-button style="width: 100%" type="primary" @click="submitForm()">注册</el-button>
      </el-form-item>
      <el-tag v-if="error" class="ml-2" type="danger">注册失败, 可能是已经有相同邮箱的用户了</el-tag>
    </el-form>
  </div>
</template>
<script setup>
import config from '../config'
import { ref } from "vue";
import axios from "axios";
const registerForm = ref()
const formContent = ref({ email: '', password: '', name: '' })

const rules = {
  email: [{ required: true, message: '请填写邮箱', trigger: 'blur' },],
  password: [{ required: true, message: '请填写密码', trigger: 'blur' }, {
    min: 5,
    max: 12,
    message: '密码的长度范围为5-12',
    trigger: 'blur'
  }],
  name: [{ required: true, message: '请填写用户名', trigger: 'blur' },]
}
let error = ref(false)
const submitForm = async () =>
{
  var params = {
    "email": formContent.value.email,
    "password": formContent.value.password,
    "is_active": true,
    "is_superuser": false,
    "is_verified": false,
    "nickname": formContent.value.name
  }
  registerForm.value.validate(async (valid) =>
  {
    if (valid)
    {
      axios.post(config.API_USER_REGISTER, params).then((response) =>
      {
        location.href = '/'
      }).catch((response) =>
      {
        console.log(response)
        error = true
      })
    }
  });
}
</script>


<style>
.register-form {
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
