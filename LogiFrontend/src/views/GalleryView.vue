<template>
  <div class="gallery-layout min-vh-100 gradient-bg pb-5">
    <Navbar />
    
    <div class="container py-4">
      <header class="d-flex justify-content-between align-items-center mb-5 animate-fade-in px-3">
        <div>
          <h1 class="h2 fw-bold text-white mb-1">Galería de Cartas</h1>
          <p class="text-white-50">Explora la colección oficial de tarjetas de élite</p>
        </div>
        <div class="gallery-stats d-none d-md-flex gap-3">
          <div class="glass-pill px-3 py-2">
            <span class="fw-bold">{{ cards.length }}</span> Cartas
          </div>
        </div>
      </header>

      <!-- Loading State -->
      <div v-if="loading" class="d-flex justify-content-center align-items-center py-5">
        <div class="spinner-enterprise"></div>
      </div>

      <!-- Empty State -->
      <div v-else-if="cards.length === 0" class="text-center py-5 glass-card mx-3">
        <div class="mb-3 text-white-50">
          <svg xmlns="http://www.w3.org/2000/svg" width="60" height="60" fill="currentColor" viewBox="0 0 16 16">
            <path d="M14.5 3a.5.5 0 0 1 .5.5v9a.5.5 0 0 1-.5.5h-13a.5.5 0 0 1-.5-.5v-9a.5.5 0 0 1 .5-.5h13zm-13-1A1.5 1.5 0 0 0 0 3.5v9A1.5 1.5 0 0 0 1.5 14h13a1.5 1.5 0 0 0 1.5-1.5v-9A1.5 1.5 0 0 0 14.5 2h-13z"/>
            <path d="M10.648 7.646a.5.5 0 0 1 .708 0l2.5 2.5a.5.5 0 0 1 0 .708l-2.5 2.5a.5.5 0 0 1-.708-.708L12.793 10l-2.145-2.146a.5.5 0 0 1 0-.708zM5.352 7.646a.5.5 0 0 0-.708 0l-2.5 2.5a.5.5 0 0 0 0 .708l2.5 2.5a.5.5 0 0 0 .708-.708L3.207 10l2.145-2.146a.5.5 0 0 0 0-.708z"/>
          </svg>
        </div>
        <h3 class="h5 text-white">No hay cartas disponibles por ahora</h3>
        <p class="text-white-50">Vuelve más tarde para ver nuevas adiciones.</p>
      </div>

      <!-- Card Grid -->
      <div v-else class="card-grid animate-staggered">
        <PokemonCard 
          v-for="card in cards" 
          :key="card.id" 
          :card="card"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from '../api/axios';
import Navbar from '../components/Navbar.vue';
import PokemonCard from '../components/PokemonCard.vue';

interface Card {
  id: string;
  title: string;
  imageUrl: string;
  description: string;
  hp: number;
  attack: number;
  defense: number;
  isPublished: boolean;
}

const cards = ref<Card[]>([]);
const loading = ref(true);

onMounted(async () => {
  try {
    const response = await axios.get('/cards');
    cards.value = response.data;
  } catch (error) {
    console.error('Error fetching cards:', error);
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.card-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 2rem;
  justify-items: center;
  padding: 0 1rem;
}

.spinner-enterprise {
  width: 50px;
  height: 50px;
  border: 5px solid rgba(255,255,255,0.1);
  border-top-color: var(--accent);
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.glass-pill {
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(4px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 99px;
  color: white;
  font-size: 0.9rem;
}

.animate-fade-in {
  animation: fadeIn 0.8s ease-out;
}

.animate-staggered > * {
  animation: slideUp 0.6s cubic-bezier(0.16, 1, 0.3, 1) both;
}

.animate-staggered > *:nth-child(1) { animation-delay: 0.1s; }
.animate-staggered > *:nth-child(2) { animation-delay: 0.2s; }
.animate-staggered > *:nth-child(3) { animation-delay: 0.3s; }
.animate-staggered > *:nth-child(4) { animation-delay: 0.4s; }

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

@keyframes slideUp {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
