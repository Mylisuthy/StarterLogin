<template>
  <div class="auth-container gradient-bg d-flex align-items-center justify-content-center">
    <div class="glass-card auth-card p-5 animate-fade-in">
      <div class="text-center mb-4">
        <h1 class="h3 fw-bold text-white">Crear Cuenta</h1>
        <p class="text-white-50">Regístrate para empezar a usar la plataforma</p>
      </div>

      <form @submit.prevent="handleRegister">
        <div class="mb-3">
          <label class="form-label text-white-50">Nombre de Usuario</label>
          <input 
            v-model="username"
            type="text" 
            class="input-enterprise" 
            placeholder="usuario123"
            required
          >
        </div>

        <div class="mb-3">
          <label class="form-label text-white-50">Correo Electrónico</label>
          <input 
            v-model="email"
            type="email" 
            class="input-enterprise" 
            placeholder="ejemplo@correo.com"
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

        <div v-if="error" class="alert alert-danger py-2 mb-3 small">
          {{ error }}
        </div>

        <button 
          type="submit" 
          class="btn-enterprise w-100 mb-3"
          :disabled="loading"
        >
          <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
          Registrarse
        </button>

        <p class="text-center text-white-50 small mb-0">
          ¿Ya tienes una cuenta? 
          <router-link to="/login" class="text-white fw-bold text-decoration-none">Inicia Sesión</router-link>
        </p>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import api from '../../api/axios';

const username = ref('');
const email = ref('');
const password = ref('');
const loading = ref(false);
const error = ref<string | null>(null);
const router = useRouter();

const handleRegister = async () => {
  loading.value = true;
  error.value = null;
  try {
    await api.post('/auth/register', { 
      userName: username.value, 
      email: email.value, 
      password: password.value 
    });
    router.push('/login');
  } catch (err: any) {
    error.value = err.response?.data?.detail || 'Error al registrar usuario';
  } finally {
    loading.value = false;
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
</style>
