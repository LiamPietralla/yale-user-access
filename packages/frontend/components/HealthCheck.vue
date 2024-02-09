<script setup lang="ts">
import { onMounted, ref } from 'vue';

const runtimeConfig = useRuntimeConfig();
const healthResult = ref<string>("Fetching health check...");

onMounted(async () => {
    // Send a request to the API Health Check endpoint
    const response = await fetch(`${runtimeConfig.public.apiBaseUrl}/api/Health`);

    // Extract the response payload (string result)
    const result = await response.text();

    // Set the result
    healthResult.value = result;
})


</script>

<template>
    <div class="text-slate-300">
    <h1>Health Check</h1>
    <p>{{ healthResult }}</p>
</div>
</template>