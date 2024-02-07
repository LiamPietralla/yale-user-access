<script setup lang="ts">
import type { HTMLInputTypes } from '~/types/html-input-types';

const props = defineProps<{
    type: HTMLInputTypes
    class: string | undefined
    placeholder: string
    modelValue: string
}>();

const emit = defineEmits([
    'update:modelValue'
])

const handleInput = (event: Event) => {
    // If input is a text input, emit the value
    if (props.type === 'text' || props.type === 'password') {
        emit('update:modelValue', (event.target as HTMLInputElement).value);
    }
}
</script>

<template>
    <input :class="`bg-zinc-900 p-2 rounded-md ${props.class}`" :type="props.type"
        :value="props.modelValue" :placeholder="props.placeholder" @input="handleInput">
</template>~/types/html-input-types