<script setup lang='ts'>
import { UserCodeStatus, type YaleUserCode } from '~/types/yale';
import { ref, type PropType } from 'vue';

const props = defineProps({
    userCode: {
        type: Object as PropType<YaleUserCode>,
        required: true
    }
});

const emit = defineEmits<{
    (e: "update-code", body: { id: number, code: string }): void
    (e: "clear-code", id: number): void 
}>();

const showCode = ref(false);
const editMode = ref(false);
const newCode = ref("");

// Toggle the show code bool to show or hide the code in the UI
const toggleShowCode = () => {
    showCode.value = !showCode.value;
};

// Toggle edit mode to allow the user to edit the code
const toggleEditMode = () => {
    editMode.value = !editMode.value;
};

// Handle the submit code button click
const handleSubmitCode = () => {
    const body = {
        id: props.userCode.id,
        code: newCode.value,
    };

    // Emit the event to the parent component
    emit("update-code", body);
}

// Handle the clear code button click
const handleClearCode = () => {
    // Emit the event to the parent component
    emit("clear-code", props.userCode.id);
}

const userCodeStatusDisplay = (status: UserCodeStatus): string => {
    switch (status) {
        case UserCodeStatus.AVAILABLE:
            return 'Available';
        case UserCodeStatus.ENABLED:
            return 'Enabled';
        case UserCodeStatus.DISABLED:
            return 'Disabled';
        default:
            return 'Unknown';
    }
}
</script>

<template>
    <tr>
        <td>{{ userCode.id }}</td>
        <td v-if="userCode.status === UserCodeStatus.ENABLED && !editMode">{{ showCode ? userCode.code : "***" }}</td>
        <td v-if="editMode"><input type="text" v-model="newCode" /></td>
        <td>{{ userCodeStatusDisplay(userCode.status) }}</td>
        <td>{{ userCode.isHome }}</td>
        <td>
            <button v-if="userCode.status === UserCodeStatus.ENABLED" @click="toggleShowCode">{{ showCode ? "Hide" : "Show" }} Code</button>
            <button v-if="!editMode" @click="toggleEditMode">{{ userCode.status === UserCodeStatus.ENABLED ? "Update Code" : "Set Code" }}</button>
            <button v-if="editMode" @click="handleSubmitCode">Submit Code</button>
            <button v-if="userCode.status === UserCodeStatus.ENABLED" @click="handleClearCode">Clear Code</button>
        </td>
    </tr>
</template>