<template>
  <nav class="glass-card m-3 p-3 d-flex justify-content-between align-items-center navbar-enterprise">
    <div class="d-flex align-items-center gap-3">
      <router-link to="/dashboard" class="d-flex align-items-center gap-2 text-decoration-none text-current">
        <div class="logo-box gradient-bg"></div>
        <span class="fw-bold h5 mb-0 d-none d-sm-block">Portal Elite</span>
      </router-link>
      <div class="v-divider"></div>
      <router-link to="/gallery" class="nav-link-enterprise">Galería</router-link>
      <router-link v-if="isAdmin" to="/admin/cards" class="nav-link-enterprise">Admin Cartas</router-link>
    </div>
    
    <div class="d-flex align-items-center gap-2 gap-md-4">
      <router-link to="/profile" class="user-pill d-flex align-items-center gap-2 text-decoration-none">
        <div class="user-avatar-sm">{{ userInitials }}</div>
        <div class="user-meta d-none d-md-block">
          <div class="fw-semibold small line-height-1 text-current">{{ auth.user?.userName }}</div>
          <div class="text-muted extra-small">{{ auth.user?.roles[0] }}</div>
        </div>
      </router-link>
      
      <button @click="toggleDarkMode" class="btn-theme-toggle" :title="isDark ? 'Modo Claro' : 'Modo Noche'">
        <svg v-if="!isDark" xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" viewBox="0 0 16 16">
          <path d="M6 .278a.768.768 0 0 1 .08.858 7.208 7.208 0 0 0-.878 3.46c0 4.021 3.278 7.277 7.318 7.277.527 0 1.04-.055 1.533-.16a.787.787 0 0 1 .81.316.733.733 0 0 1-.031.893A8.349 8.349 0 0 1 8.344 16C3.734 16 0 12.286 0 7.71 0 4.266 2.114 1.312 5.124.06A.752.752 0 0 1 6 .278z"/>
        </svg>
        <svg v-else xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" viewBox="0 0 16 16">
          <path d="M8 11a3 3 0 1 1 0-6 3 3 0 0 1 0 6zm0 1a4 4 0 1 0 0-8 4 4 0 0 0 0 8zM8 0a.5.5 0 0 1 .5.5v2a.5.5 0 0 1-1 0v-2A.5.5 0 0 1 8 0zm0 13a.5.5 0 0 1 .5.5v2a.5.5 0 0 1-1 0v-2a.5.5 0 0 1 .5-.5zM0 8a.5.5 0 0 1 .5-.5h2a.5.5 0 0 1 0 1h-2A.5.5 0 0 1 0 8zm13 0a.5.5 0 0 1 .5-.5h2a.5.5 0 0 1 0 1h-2a.5.5 0 0 1-.5-.5zM2.343 2.343a.5.5 0 0 1 .707 0l1.414 1.414a.5.5 0 0 1-.707.707L2.343 3.05a.5.5 0 0 1 0-.707zm9.193 9.193a.5.5 0 0 1 .707 0l1.414 1.414a.5.5 0 0 1-.707.707l-1.414-1.414a.5.5 0 0 1 0-.707zm0-9.193a.5.5 0 0 1 0 .707L10.122 4.464a.5.5 0 0 1-.707-.707L10.829 2.343a.5.5 0 0 1 .707 0zm-9.193 9.193a.5.5 0 0 1 0 .707L3.05 13.657a.5.5 0 0 1-.707-.707l1.414-1.414a.5.5 0 0 1 .707 0z"/>
        </svg>
      </button>
      
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

const isAdmin = computed(() => auth.user?.roles.includes('Admin'));
const userInitials = computed(() => auth.user?.userName.substring(0, 2).toUpperCase() || '??');

const isDark = computed(() => {
  return document.documentElement.classList.contains('dark');
});

const toggleDarkMode = () => {
  if (document.documentElement.classList.contains('dark')) {
    document.documentElement.classList.remove('dark');
    document.documentElement.classList.add('light');
    localStorage.setItem('theme', 'light');
  } else {
    document.documentElement.classList.remove('light');
    document.documentElement.classList.add('dark');
    localStorage.setItem('theme', 'dark');
  }
};

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

.btn-theme-toggle {
  background: none;
  border: none;
  color: var(--text-main);
  padding: 0.5rem;
  border-radius: 0.5rem;
  transition: var(--transition-smooth);
  cursor: pointer;
}

.btn-theme-toggle:hover {
  background: rgba(0, 0, 0, 0.05);
  transform: rotate(15deg);
}

.nav-link-enterprise {
  color: var(--text-main);
  text-decoration: none;
  font-weight: 500;
  font-size: 0.9rem;
  padding: 0.5rem 0.75rem;
  border-radius: 0.5rem;
  transition: var(--transition-smooth);
}

.nav-link-enterprise:hover, .router-link-active.nav-link-enterprise {
  background: rgba(59, 130, 246, 0.1);
  color: var(--accent);
}
</style>
