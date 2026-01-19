<template>
  <div class="gallery-layout">
    <Navbar />
    
    <main class="container py-5">
      <h1 class="text-white mb-5 text-center fw-bold display-4">Galería Mágica</h1>
      
      <div v-if="loading" class="text-center text-white">
        <div class="spinner-border" role="status">
          <span class="visually-hidden">Cargando...</span>
        </div>
      </div>

      <div v-else-if="cards.length === 0" class="text-center text-muted">
        <h3>No hay cartas publicadas aún.</h3>
      </div>

      <div v-else class="row g-4 justify-content-center">
        <div class="col-auto" v-for="card in cards" :key="card.id">
          <MagicCard 
            :title="card.title" 
            :description="card.description" 
            :image-url="card.imageUrl"
          />
        </div>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import Navbar from '../../components/Navbar.vue';
import MagicCard from '../../components/MagicCard.vue';
import { magicCardsApi, type MagicCard as MagicCardType } from '../../services/magicCards';
import { useToastStore } from '../../stores/toast';

const cards = ref<MagicCardType[]>([]);
const loading = ref(true);
const toast = useToastStore();

onMounted(async () => {
    try {
        cards.value = await magicCardsApi.getAll(false); // Only published
    } catch (error) {
        toast.show('Error al cargar la galería', 'error');
    } finally {
        loading.value = false;
    }
});
</script>

<style scoped>
.gallery-layout {
  min-height: 100vh;
  background-color: var(--background);
}
</style>
