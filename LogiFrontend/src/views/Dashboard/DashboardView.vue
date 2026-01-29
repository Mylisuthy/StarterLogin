<template>
  <div class="dashboard-layout">
    <Navbar />

    <main class="container py-5">
      <header class="mb-5">
        <h2 class="fw-bold mb-2">Explorar Contenido</h2>
        <p class="text-muted">Descubre las mejores películas, series y documentales.</p>
      </header>

      <!-- Filters -->
      <div class="mb-4 d-flex flex-wrap gap-2">
        <button 
          @click="selectedGenre = null" 
          class="btn filter-btn" 
          :class="selectedGenre === null ? 'btn-primary' : 'btn-outline-secondary'"
        >
          Todos
        </button>
        <button 
          v-for="genre in genres" 
          :key="genre.id" 
          @click="selectedGenre = genre.id"
          class="btn filter-btn"
          :class="selectedGenre === genre.id ? 'btn-primary' : 'btn-outline-secondary'"
        >
          {{ genre.name }}
        </button>
      </div>

      <!-- Media Grid -->
      <div v-if="loading" class="text-center py-5">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Cargando...</span>
        </div>
      </div>

      <div v-else-if="filteredMedia.length > 0" class="row g-4">
        <div class="col-12 col-sm-6 col-lg-4 col-xl-3" v-for="item in filteredMedia" :key="item.id">
          <MediaCard :media="item" />
        </div>
      </div>

      <div v-else class="text-center py-5 glass-card">
        <h4 class="text-muted">No se encontró contenido en esta categoría.</h4>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import Navbar from '../../components/Navbar.vue';
import MediaCard from '../../components/MediaCard.vue';
import { mediaService, type MediaResponse, type GenreResponse } from '../../services/mediaService';

const media = ref<MediaResponse[]>([]);
const genres = ref<GenreResponse[]>([]);
const loading = ref(true);
const selectedGenre = ref<string | null>(null);

const filteredMedia = computed(() => {
  if (!selectedGenre.value) return media.value;
  return media.value.filter((m: MediaResponse) => m.genreId === selectedGenre.value);
});

onMounted(async () => {
  try {
    const [mediaData, genresData] = await Promise.all([
      mediaService.getAllMedia(),
      mediaService.getAllGenres()
    ]);
    media.value = mediaData;
    genres.value = genresData;
  } catch (error) {
    console.error('Error loading dashboard data:', error);
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.dashboard-layout {
  min-height: 100vh;
  background-color: var(--background);
}

.filter-btn {
  border-radius: 20px;
  padding: 6px 20px;
  font-weight: 600;
  transition: all 0.3s ease;
}

.btn-primary {
  background-color: var(--primary-color, #6366f1);
  border-color: var(--primary-color, #6366f1);
}

.glass-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 16px;
}
</style>
