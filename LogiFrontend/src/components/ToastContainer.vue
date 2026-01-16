<template>
  <div class="toast-container position-fixed top-0 end-0 p-3" style="z-index: 2000">
    <transition-group name="toast">
      <div 
        v-for="toast in store.toasts" 
        :key="toast.id"
        class="glass-card toast-item mb-2 p-3 d-flex align-items-center gap-3"
        :class="`toast-${toast.type}`"
      >
        <div class="toast-icon">
          <svg v-if="toast.type === 'success'" width="20" height="20" fill="currentColor" viewBox="0 0 16 16"><path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/></svg>
          <svg v-if="toast.type === 'error'" width="20" height="20" fill="currentColor" viewBox="0 0 16 16"><path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293 5.354 4.646z"/></svg>
        </div>
        <div class="toast-message small fw-medium">{{ toast.message }}</div>
        <button @click="store.remove(toast.id)" class="btn-close-sm ms-auto">×</button>
      </div>
    </transition-group>
  </div>
</template>

<script setup lang="ts">
import { useToastStore } from '../stores/toast';
const store = useToastStore();
</script>

<style scoped>
.toast-item {
  min-width: 300px;
  max-width: 400px;
  border-left: 4px solid var(--accent);
  background: var(--glass-bg);
  backdrop-filter: blur(12px);
  box-shadow: var(--shadow-lg);
}

.toast-success { border-left-color: var(--success); }
.toast-error { border-left-color: var(--error); }

.toast-icon {
  flex-shrink: 0;
}

.toast-success .toast-icon { color: var(--success); }
.toast-error .toast-icon { color: var(--error); }

.btn-close-sm {
  background: none;
  border: none;
  font-size: 1.5rem;
  line-height: 1;
  color: var(--text-muted);
  cursor: pointer;
}

.toast-enter-active, .toast-leave-active {
  transition: all 0.5s cubic-bezier(0.68, -0.55, 0.265, 1.55);
}
.toast-enter-from { opacity: 0; transform: translateX(100px); }
.toast-leave-to { opacity: 0; transform: scale(0.9); }
</style>
