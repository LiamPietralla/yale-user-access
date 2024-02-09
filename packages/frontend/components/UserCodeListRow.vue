<script setup lang='ts'>
import { UserCodeStatus, type YaleUserCode } from '~/types/yale';
import { type PropType } from 'vue';

const props = defineProps({
    userCode: {
        type: Object as PropType<YaleUserCode>,
        required: true
    }
});

const emit = defineEmits<{
    (e: "show-code", id: number): void
    (e: "update-code", id: number): void
    (e: "clear-code", id: number): void
}>();

const handleShowCodeClick = () => {
    // Emit the event to the parent component to handle
    emit("show-code", props.userCode.id);
}

// Handle the update code button click
const handleUpdateCodeClick = () => {
    // Emit the event to the parent component
    emit("update-code", props.userCode.id);
}

// Handle the clear code button click
const handleClearCodeClick = () => {
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
            <YaleButton type="button" @click="handleShowCodeClick" :disabled="props.userCode.status !== UserCodeStatus.ENABLED">
                <IconEye />
            </YaleButton>
            <YaleButton type="button" class="ml-2" @click="handleUpdateCodeClick">
                <IconPencil />
            </YaleButton>
            <YaleButton type="button" class="ml-2" @click="handleClearCodeClick" :disabled="props.userCode.status !== UserCodeStatus.ENABLED" v-if="!props.userCode.isHome">
                <IconTrash />
            </YaleButton>
        </td>
    </tr>
</template>