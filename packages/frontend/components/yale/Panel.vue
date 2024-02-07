<script setup lang="ts">
const props = defineProps<{
    heading?: string;
}>();
</script>

<template>
    <h2 v-if="props.heading" class="hr text-lg font-bold mb-2">{{ props.heading }}</h2>
    <div class="rounded-md bg-zinc-800 p-2">
        <slot></slot>
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