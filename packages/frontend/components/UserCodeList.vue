<script setup lang='ts'>
import type { ApiResponse } from '@/types/api-response';
import UserCodeListRow from './UserCodeListRow.vue';
import { onMounted, ref } from 'vue';
import { UserCodeStatus, type YaleUserCode } from '@/types/yale';

const userCodes = ref([] as YaleUserCode[]);
const userCode = ref({} as YaleUserCode);
const loading = ref(true);
const runtimeConfig = useRuntimeConfig();

// Modals
const showModal = ref(false);
const editModal = ref(false);
const clearModal = ref(false);

// When the component is mounted, load the user codes
onMounted(async () => {
    // Load the user codes and set the loading status when complete
    await loadUserCodes();
    loading.value = false;
});

const loadUserCodes = async () => {
    // Send a request to the api to get the user codes
    const response = await fetch(`${runtimeConfig.public.apiBaseUrl}/api/Yale/codes`, { credentials: 'include' });

    // Parse the response
    const apiResponse = await response.json() as ApiResponse<YaleUserCode[]>;

    // Set the user codes by id
    userCodes.value = apiResponse.data?.sort((a, b) => a.id - b.id) || [];
}

const handleUpdateCode = async (body: { id: number, code: string }) => {
    // Set the loading status
    loading.value = true;

    // Send a request to the api, the body will just be the new code
    const response = await fetch(`${runtimeConfig.public.apiBaseUrl}/api/Yale/code/${body.id}`, {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        },
        body: `"${body.code}"`
    });

    // Parse the result
    const apiResponse = await response.json() as ApiResponse<boolean>;

    if (apiResponse.success) {
        // If the response was successful update the code in the list
        const index = userCodes.value.findIndex(x => x.id === body.id);
        userCodes.value[index].code = body.code;
        userCodes.value[index].status = UserCodeStatus.ENABLED;
    } else {
        // Otherwise display an error to the user.
        alert(apiResponse.error ?? 'Unknown error');
    }

    // Set the loading status
    loading.value = false;
}

const handleClearCode = async (id: number) => {
    // Set the loading status
    loading.value = true;

    // Send a request to the api
    const response = await fetch(`${runtimeConfig.public.apiBaseUrl}/api/Yale/code/${id}/status`, {
        method: 'POST',
        credentials: 'include'
    });

    // Parse the response
    const apiResponse = await response.json() as ApiResponse<boolean>;

    if (apiResponse.success) {
        // If the response was successful set the code to available in the list
        const index = userCodes.value.findIndex(x => x.id === id);
        userCodes.value[index].code = '';
        userCodes.value[index].status = UserCodeStatus.AVAILABLE;
    } else {
        // Otherwise display an error to the user.
        alert(apiResponse.error ?? 'Unknown error');
    }

    // Set the loading status
    loading.value = false;
} 

const handleShowCode = (id: number) => {
    // Set the user code to show
    const code = userCodes.value.find(x => x.id === id);

    // If the code is not found then show an error to the user
    if (!code) {
        alert('Code not found');
        return;
    }

    // Set the user code to show
    userCode.value = code;

    // Show the show-modal
    showModal.value = true;
}
</script>

<template>
    <!-- Handle loading state -->
    <div v-if="loading">Loading...</div>
    <table v-else class="w-full text-left">
        <tr>
            <th scope="col">Code ID</th>
            <th scope="col">Status</th>
            <th scope="col">Actions</th>
        </tr>
        <UserCodeListRow v-for="userCode in userCodes" :key="userCode.id" :user-code="userCode"
            @update-code="handleUpdateCode" @clear-code="handleClearCode" @show-code="handleShowCode" />
    </table>

    <!-- Show modal -->
    <Modal v-if="showModal" @close="showModal = false">
        <template #title>
            User Code # {{ userCode.id }}
        </template>
        <template #default>
            Code: {{ userCode.code }}
        </template>
    </Modal>

    <!-- Edit modal -->
    <Modal v-if="editModal" @close="editModal = false">
        <template #title>
            Edit User Code # {{ userCode.id }}
        </template>
        <template #default>
            TODO
        </template>
        <template #footer>
            TODO
        </template>
    </Modal>

    <!-- Clear modal -->
    <Modal v-if="clearModal" @close="clearModal = false">
        <template #title>
            Delete User Code # {{ userCode.id }}
        </template>
        <template #default>
            TODO
        </template>
        <template #footer>
            TODO
        </template>
    </Modal>
</template>