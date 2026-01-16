<template>
  <div class="auth-container gradient-bg d-flex align-items-center justify-content-center">
    <div class="glass-card auth-card p-5 animate-fade-in">
      <div class="text-center mb-4">
        <h1 class="h3 fw-bold text-white">Bienvenido de Nuevo</h1>
        <p class="text-white-50">Ingresa tus credenciales para continuar</p>
      </div>

      <form @submit.prevent="handleLogin">
        <div class="mb-3">
          <label class="form-label text-white-50">Correo Electrónico</label>
          <input 
            v-model="email"
            type="email" 
            class="input-enterprise" 
            placeholder="admin@starterlogin.com"
            required
          >
        </div>
        
        <div class="mb-4">
          <label class="form-label text-white-50">Contraseña</label>
          <input 
            v-model="password"
            type="password" 
            class="input-enterprise" 
            placeholder="••••••••"
            required
          >
        </div>

        <div v-if="auth.error" class="alert alert-danger py-2 mb-3 small">
          {{ auth.error }}
        </div>

        <button 
          type="submit" 
          class="btn-enterprise w-100 mb-3"
          :disabled="auth.loading"
        >
          <span v-if="auth.loading" class="spinner-border spinner-border-sm me-2"></span>
          Iniciar Sesión
        </button>

        <p class="text-center text-white-50 small mb-0">
          ¿No tienes una cuenta? 
          <router-link to="/register" class="text-white fw-bold text-decoration-none">Regístrate</router-link>
        </p>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useAuthStore } from '../../stores/auth';
import { useRouter } from 'vue-router';

const email = ref('');
const password = ref('');
const auth = useAuthStore();
const router = useRouter();

const handleLogin = async () => {
  const success = await auth.login({ email: email.value, password: password.value });
  if (success) {
    router.push('/dashboard');
  }
};
</script>

<style scoped>
.auth-container {
  min-height: 100vh;
  width: 100vw;
}

.auth-card {
  width: 100%;
  max-width: 450px;
}

.animate-fade-in {
  animation: fadeIn 0.6s ease-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}

input::placeholder {
  color: #94a3b8;
}
</style>
