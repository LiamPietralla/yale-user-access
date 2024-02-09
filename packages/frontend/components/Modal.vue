<script setup lang="ts">
const emit = defineEmits<{
    (e: "close"): void
}>();

const handleClick = () => {
    emit("close");
}

// If clicking outside the modal, close it
document.addEventListener("click", (event) => {
    if (event.target === document.querySelector(".fixed")) {
        handleClick();
    }
});
</script>

<template>
    <div class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center">
        <div class="bg-zinc-800 rounded-md min-w-96">
            <div class="p-6 border-b border-slate-300">
                <h3 class="text-md font-bold flex justify-between items-center">
                    <slot name="title"></slot>
                    <YaleButton type="button" @click="handleClick">
                        <IconXMark />
                    </YaleButton>
                </h3>
            </div>
            <div class="p-6 border-b border-slate-300">
                <slot></slot>
            </div>
            <div class="p-6">
                <slot name="footer"></slot>
            </div>
        </div>
    </div>
</template>