<template>
  <ToastContainer />
  <router-view v-slot="{ Component }">
    <transition name="fade" mode="out-in">
      <component :is="Component" />
    </transition>
  </router-view>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted } from 'vue';
import ToastContainer from './components/ToastContainer.vue';
import { IdleService } from './services/idle';

onMounted(() => {
  IdleService.start();
});

onUnmounted(() => {
  IdleService.stop();
});
</script>

<style>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
