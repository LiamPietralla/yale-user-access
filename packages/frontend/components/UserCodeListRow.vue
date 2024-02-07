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
    (e: "show-code", id: number): void
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
    // If this code is the home code, confirm before allowing the user to edit it
    if (props.userCode.isHome) {
        if (!confirm("Are you sure you want to edit the home code?")) {
            return;
        }
    }

    editMode.value = !editMode.value;
};

const handleShowCodeClick = () => {
    // Emit the event to the parent component to handle
    emit("show-code", props.userCode.id);
}

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
    <tr scope="row">
        <td>
            <template v-if="userCode.isHome">
                {{ userCode.id }} (<IconHome />)
            </template>
            <template v-else>
                {{ userCode.id }}
            </template>
        </td>
        <td>{{ userCodeStatusDisplay(userCode.status) }}</td>
        <td class="flex">
            <YaleButton type="button">
                <IconEye @click="handleShowCodeClick" />
            </YaleButton>
            <YaleButton type="button" class="ml-2">
                <IconPencil />
            </YaleButton>
            <YaleButton type="button" class="ml-2">
                <IconTrash />
            </YaleButton>
        </td>
    </tr>
</template>