<script setup lang="ts">
import type { HTMLButtonTypes } from '~/types/html-input-types';

interface ButtonProps {
    type: HTMLButtonTypes | undefined
    class: string | undefined
    text: string
}

const props = withDefaults(defineProps<ButtonProps>(), {
    type: 'button'
})

const emit = defineEmits([
    'click'
])

const handleClick = () => {
    emit('click');
}
</script>

<template>
    <button :type="props.type" :class="`bg-stone-950 hover:bg-stone-900 p-2 rounded-md ${props.class}`"
        @click="handleClick">{{ props.text }}</button>
</template>