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
    <div class="bg-zinc-800 text-slate-300 text-center w-full md:w-96 rounded-md">
        <h1 class="text-7xl mt-5">Yale</h1>
        <div>
            <h4 class="hr text-2xl mt-3">User Access</h4>
        </div>

        <input class="block mt-10 w-5/6 mx-auto bg-zinc-900 p-2 rounded-md" type="password" v-model="password" placeholder="Password">
        <p class="mt-3 w-5/6 mx-auto text-red-600" v-if="passwordError">{{ passwordError }}</p>
        <button class="mb-8 mt-3 w-5/6 mx-auto bg-stone-950 hover:bg-stone-900 p-2 rounded-md" @click='handleLogin'>Login</button>
    </div>
</template>

<style>
.hr {
    display: inline-block;
}

.hr::after {
    content: '';
    display: block;
    border-top: 2px solid theme('colors.slate.300');
    margin-top: .1rem;
}
</style>