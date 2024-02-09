<script setup lang="ts">
import type { HTMLButtonTypes } from '~/types/html-input-types';

interface ButtonProps {
    type?: HTMLButtonTypes
    disabled?: boolean
}

const props = withDefaults(defineProps<ButtonProps>(), {
    type: 'button',
    disabled: false
})

const emit = defineEmits([
    'click'
])

const handleClick = () => {
    emit('click');
}
</script>

<template>
    <button :type="props.type" :disabled="props.disabled" class="bg-stone-950 hover:bg-stone-900 p-2 rounded-md disabled:opacity-50 disabled:cursor-not-allowed"
        @click="handleClick">
        <slot></slot>
    </button>
</template>