<script setup lang='ts'>
import { ref } from 'vue';
import { type ApiResponse } from '~/types/api-response';

const password = ref('');
const passwordError = ref('');
const authenticated = useCookie<boolean>('authenticated');
const runtimeConfig = useRuntimeConfig();

const handleLogin = async () => {
    // Reset the error
    passwordError.value = '';

    // Send a request to the api to login
    const response = await fetch(`${runtimeConfig.public.apiBaseUrl}/api/Authentication/login`, {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        },
        body: `"${password.value}"`
    });

    // Parse the response
    const apiResponse = await response.json() as ApiResponse<boolean>;

    if (apiResponse.success) {
        // If the response was successful, login and redirect to the home page
        authenticated.value = true;
        navigateTo('/');
    } else {
        // Otherwise, set the error
        passwordError.value = apiResponse.error ?? 'Unknown error';
    }
}
</script>

<template>
    <h1>Login</h1>

    <input type="password" v-model="password" placeholder="Password">
    <p v-if="passwordError">{{ passwordError }}</p>
    <button @click='handleLogin'>Login</button>
</template>