<template>
  <div class="dashboard-layout">
    <Navbar />

    <main class="container py-4">
      <div class="row g-4">
        <div class="col-md-4" v-for="n in 3" :key="n">
          <div class="glass-card p-4 h-100 card-hover">
            <h5 class="fw-bold mb-3">Recurso Corporativo {{ n }}</h5>
            <p class="text-muted small">Información protegida accesible gracias a tu sesión segura por JWT.</p>
            <div class="mt-4 pt-3 border-top">
              <span class="badge bg-primary-subtle text-primary border border-primary-subtle px-3 py-2 rounded-pill">
                Nivel de Acceso: {{ auth.user?.roles.join(', ') }}
              </span>
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import Navbar from '../../components/Navbar.vue';
import { useAuthStore } from '../../stores/auth';
import { useRouter } from 'vue-router';

const auth = useAuthStore();
const router = useRouter();

const handleLogout = () => {
  auth.logout();
  router.push('/login');
};
</script>

<style scoped>
.dashboard-layout {
  min-height: 100vh;
  background-color: var(--background);
}

.logo-box {
  width: 32px;
  height: 32px;
  border-radius: 8px;
}

.card-hover {
  transition: var(--transition-smooth);
  cursor: pointer;
}

.card-hover:hover {
  transform: translateY(-5px);
  box-shadow: 0 12px 40px rgba(0,0,0,0.1);
}

.bg-primary-subtle {
  background-color: #e2e8f0;
}
</style>
