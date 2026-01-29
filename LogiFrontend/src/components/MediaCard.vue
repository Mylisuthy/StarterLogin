<template>
  <div class="media-card glass-card h-100 card-hover overflow-hidden">
    <div class="image-container">
      <img :src="media.imageUrl || 'https://via.placeholder.com/300x200?text=No+Image'" :alt="media.title" class="media-image" />
      <div class="media-type-badge">{{ media.contentType === 'Movie' ? 'Película' : 'Serie' }}</div>
    </div>
    <div class="p-4 content">
      <div class="d-flex justify-content-between align-items-start mb-2">
        <h5 class="fw-bold mb-0 text-truncate" :title="media.title">{{ media.title }}</h5>
        <span class="badge rating-badge ms-2">{{ media.rating || 'N/A' }}</span>
      </div>
      <p class="text-muted small description mb-3">{{ media.description }}</p>
      <div class="mt-auto pt-3 border-top d-flex justify-content-between align-items-center">
        <span class="badge genre-badge">
          {{ media.genreName }}
        </span>
        <span class="text-muted tiny" v-if="media.releaseDate">
          {{ new Date(media.releaseDate).getFullYear() }}
        </span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { defineProps } from 'vue';
import type { MediaResponse } from '../services/mediaService';

defineProps<{
  media: MediaResponse;
}>();
</script>

<style scoped>
.media-card {
  display: flex;
  flex-direction: column;
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.image-container {
  position: relative;
  height: 200px;
  overflow: hidden;
}

.media-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s ease;
}

.card-hover:hover .media-image {
  transform: scale(1.1);
}

.media-type-badge {
  position: absolute;
  top: 10px;
  left: 10px;
  background: rgba(0, 0, 0, 0.6);
  backdrop-filter: blur(4px);
  color: white;
  padding: 4px 12px;
  border-radius: 20px;
  font-size: 0.75rem;
  font-weight: 600;
}

.rating-badge {
  background: var(--primary-color, #6366f1);
  color: white;
  padding: 4px 8px;
  border-radius: 6px;
  font-size: 0.7rem;
}

.genre-badge {
  background-color: #e2e8f0;
  color: #475569;
  border: 1px solid #cbd5e1;
  border-radius: 20px;
  padding: 4px 12px;
}

.description {
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  height: 3.6rem;
}

.tiny {
  font-size: 0.75rem;
}

.card-hover {
  transition: var(--transition-smooth);
  cursor: pointer;
}

.card-hover:hover {
  transform: translateY(-5px);
  box-shadow: 0 12px 40px rgba(0,0,0,0.15);
}
</style>
