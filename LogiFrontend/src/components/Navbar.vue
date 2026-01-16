<template>
  <nav class="glass-card m-3 p-3 d-flex justify-content-between align-items-center navbar-enterprise">
    <router-link to="/dashboard" class="d-flex align-items-center gap-2 text-decoration-none text-current">
      <div class="logo-box gradient-bg"></div>
      <span class="fw-bold h5 mb-0 d-none d-sm-block">Portal Elite</span>
    </router-link>
    
    <div class="d-flex align-items-center gap-2 gap-md-4">
      <router-link to="/profile" class="user-pill d-flex align-items-center gap-2 text-decoration-none">
        <div class="user-avatar-sm">{{ userInitials }}</div>
        <div class="user-meta d-none d-md-block">
          <div class="fw-semibold small line-height-1 text-current">{{ auth.user?.userName }}</div>
          <div class="text-muted extra-small">{{ auth.user?.roles[0] }}</div>
        </div>
      </router-link>
      
      <div class="v-divider"></div>
      
      <button @click="handleLogout" class="btn-icon-danger" title="Cerrar Sesión">
        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" viewBox="0 0 16 16">
          <path fill-rule="evenodd" d="M10 12.5a.5.5 0 0 1-.5.5h-8a.5.5 0 0 1-.5-.5v-9a.5.5 0 0 1 .5-.5h8a.5.5 0 0 1 .5.5v2a.5.5 0 0 0 1 0v-2A1.5 1.5 0 0 0 9.5 2h-8A1.5 1.5 0 0 0 0 3.5v9A1.5 1.5 0 0 0 1.5 14h8a1.5 1.5 0 0 0 1.5-1.5v-2a.5.5 0 0 0-1 0v2z"/>
          <path fill-rule="evenodd" d="M15.854 8.354a.5.5 0 0 0 0-.708l-3-3a.5.5 0 0 0-.708.708L14.293 7.5H5.5a.5.5 0 0 0 0 1h8.793l-2.147 2.146a.5.5 0 0 0 .708.708l3-3z"/>
        </svg>
      </button>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useAuthStore } from '../stores/auth';
import { useRouter } from 'vue-router';

const auth = useAuthStore();
const router = useRouter();

const userInitials = computed(() => auth.user?.userName.substring(0, 2).toUpperCase() || '??');

const handleLogout = () => {
  auth.logout();
  router.push('/login');
};
</script>

<style scoped>
.navbar-enterprise {
  position: sticky;
  top: 1rem;
  z-index: 1000;
}

.text-current {
  color: var(--text-main);
}

.logo-box {
  width: 28px;
  height: 28px;
  border-radius: 6px;
}

.user-pill {
  padding: 0.25rem 0.75rem 0.25rem 0.25rem;
  border-radius: 2rem;
  background: rgba(0,0,0,0.03);
  transition: var(--transition-smooth);
}

.user-pill:hover {
  background: rgba(0,0,0,0.06);
}

.user-avatar-sm {
  width: 32px;
  height: 32px;
  background: var(--primary);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  font-size: 0.75rem;
  font-weight: bold;
}

.extra-small {
  font-size: 0.65rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.line-height-1 {
  line-height: 1;
}

.v-divider {
  width: 1px;
  height: 24px;
  background: var(--text-muted);
  opacity: 0.2;
}

.btn-icon-danger {
  background: none;
  border: none;
  color: var(--error);
  padding: 0.5rem;
  border-radius: 0.5rem;
  transition: var(--transition-smooth);
}

.btn-icon-danger:hover {
  background: rgba(239, 68, 68, 0.1);
  transform: scale(1.1);
}
</style>
