<template>
    <div class="body bg">
        <div class="login-form h-96 w-80 flex justify-center items-center" style="flex-direction: column;">
            <span class="flex h-6"></span>
            <i class="fa-solid fa-triangle-exclamation fa-bounce text-9xl text-white flex"></i>
            <span class="flex h-4"></span>
            <span class="text-white flex text-lg">您的账号尚未拥有访问权限</span>
            <span class="flex h-4"></span>
            <RouterLink to="/logout" class=" w-4/5">
                <el-button class="w-full">登出</el-button>
            </RouterLink>
        </div>
    </div>
</template>

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