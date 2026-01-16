<template>
  <div class="profile-layout gradient-bg min-vh-100 pb-5">
    <Navbar />
    
    <div class="container-narrow mt-4">
      <nav class="d-flex justify-content-between align-items-center mb-4 px-3">
        <router-link to="/dashboard" class="btn-back">
          <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" viewBox="0 0 16 16">
            <path fill-rule="evenodd" d="M15 8a.5.5 0 0 0-.5-.5H2.707l3.147-3.146a.5.5 0 1 0-.708-.708l-4 4a.5.5 0 0 0 0 .708l4 4a.5.5 0 0 0 .708-.708L2.707 8.5H14.5A.5.5 0 0 0 15 8z"/>
          </svg>
          Regresar
        </router-link>
        <h2 class="h4 fw-bold text-white mb-0">Mi Perfil</h2>
        <div style="width: 80px"></div>
      </nav>

      <div class="glass-card p-5 animate-slide-up">
        <div class="text-center mb-5">
          <div class="avatar-large mx-auto mb-3">
            {{ userInitials }}
          </div>
          <h3 class="h5 fw-bold mb-1">{{ auth.user?.userName }}</h3>
          <span class="badge rounded-pill bg-accent px-3 py-1">{{ auth.user?.roles.join(', ') }}</span>
        </div>

        <div class="profile-grid">
          <div class="info-group">
            <label>Nombre de Usuario</label>
            <div class="info-value">{{ auth.user?.userName }}</div>
          </div>
          <div class="info-group">
            <label>Email Corporativo</label>
            <div class="info-value">{{ auth.user?.email }}</div>
          </div>
          <div class="info-group">
            <label>ID de Usuario</label>
            <div class="info-value small text-code">{{ auth.user?.id }}</div>
          </div>
        </div>

        <div class="mt-5 pt-4 border-top border-light border-opacity-10">
          <button class="btn-enterprise w-100 opacity-50" disabled>
            Editar Perfil (Próximamente)
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import Navbar from '../../components/Navbar.vue';
import { computed } from 'vue';
import { useAuthStore } from '../../stores/auth';

const auth = useAuthStore();

const userInitials = computed(() => {
  return auth.user?.userName.substring(0, 2).toUpperCase() || '??';
});
</script>

<style scoped>
.container-narrow {
  max-width: 600px;
  margin: 0 auto;
}

.btn-back {
  color: white;
  text-decoration: none;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 500;
  opacity: 0.8;
  transition: var(--transition-smooth);
}

.btn-back:hover {
  opacity: 1;
  transform: translateX(-5px);
}

.avatar-large {
  width: 100px;
  height: 100px;
  background: white;
  color: var(--primary);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2rem;
  font-weight: 800;
  border-radius: 2rem;
  box-shadow: var(--shadow-lg);
}

.profile-grid {
  display: grid;
  gap: 1.5rem;
}

.info-group label {
  display: block;
  font-size: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  color: var(--text-muted);
  margin-bottom: 0.5rem;
}

.info-value {
  background: rgba(0,0,0,0.05);
  padding: 0.75rem 1rem;
  border-radius: 0.75rem;
  font-weight: 500;
}

.text-code {
  font-family: 'JetBrains Mono', monospace;
  font-size: 0.8rem;
}

.bg-accent {
  background-color: var(--accent);
}

.animate-slide-up {
  animation: slideUp 0.6s cubic-bezier(0.16, 1, 0.3, 1);
}

@keyframes slideUp {
  from { opacity: 0; transform: translateY(30px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
