<template>
  <div class="pokemon-card-container" :class="{ 'is-published': card.isPublished, 'is-admin': isAdmin }">
    <div class="pokemon-card" :style="cardStyles">
      <div class="card-glass-shine"></div>
      
      <!-- Card Image Section -->
      <div class="card-image-wrapper">
        <img :src="card.imageUrl" :alt="card.title" class="card-image" loading="lazy">
        <div class="hp-badge">{{ card.hp }} HP</div>
      </div>

      <!-- Card Content Section -->
      <div class="card-info">
        <h3 class="card-title">{{ card.title }}</h3>
        <p class="card-description">{{ card.description }}</p>
        
        <div class="card-stats">
          <div class="stat-item">
            <span class="stat-label">ATK</span>
            <span class="stat-value">{{ card.attack }}</span>
          </div>
          <div class="stat-divider"></div>
          <div class="stat-item">
            <span class="stat-label">DEF</span>
            <span class="stat-value">{{ card.defense }}</span>
          </div>
        </div>
      </div>

      <!-- Admin Actions Overlay -->
      <div v-if="isAdmin" class="admin-actions">
        <button @click.stop="$emit('edit', card)" class="btn-action edit" title="Editar">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
            <path d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z"/>
          </svg>
        </button>
        <button @click.stop="$emit('delete', card.id)" class="btn-action delete" title="Eliminar">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
            <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z"/>
            <path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"/>
          </svg>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  card: {
    id: string;
    title: string;
    imageUrl: string;
    description: string;
    hp: number;
    attack: number;
    defense: number;
    isPublished: boolean;
  },
  isAdmin?: boolean
}>();

defineEmits(['edit', 'delete']);

const cardStyles = computed(() => {
  // Generate a unique tint based on the HP or other stats
  const hue = (props.card.hp * 2) % 360;
  return {
    '--card-theme-color': `hsl(${hue}, 70%, 50%)`,
    '--card-theme-glow': `hsla(${hue}, 70%, 50%, 0.3)`
  };
});
</script>

<style scoped>
.pokemon-card-container {
  width: 250px; /* Base width */
  height: 350px; /* 2.5:3.5 Ratio approx */
  perspective: 1000px;
  margin: 1rem;
}

.pokemon-card {
  position: relative;
  width: 100%;
  height: 100%;
  background: var(--surface);
  border-radius: 12px;
  border: 8px solid #fbd743; /* Classic Pokemon Gold border */
  box-shadow: 0 10px 20px rgba(0,0,0,0.2);
  overflow: hidden;
  transition: transform 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275), box-shadow 0.3s ease;
  display: flex;
  flex-direction: column;
}

.pokemon-card:hover {
  transform: translateY(-10px) rotateY(5deg);
  box-shadow: 0 15px 30px var(--card-theme-glow);
}

.card-glass-shine {
  position: absolute;
  top: 0; left: 0; right: 0; bottom: 0;
  background: linear-gradient(135deg, rgba(255,255,255,0.3) 0%, rgba(255,255,255,0) 50%);
  pointer-events: none;
  z-index: 2;
}

.card-image-wrapper {
  position: relative;
  height: 180px;
  background: #2d3748;
  margin: 4px;
  border: 4px solid #374151;
  overflow: hidden;
}

.card-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s ease;
}

.pokemon-card:hover .card-image {
  transform: scale(1.1);
}

.hp-badge {
  position: absolute;
  top: 8px;
  right: 8px;
  background: gold;
  color: #1a202c;
  padding: 2px 8px;
  border-radius: 99px;
  font-weight: 800;
  font-size: 0.75rem;
  box-shadow: 0 2px 4px rgba(0,0,0,0.2);
}

.card-info {
  padding: 12px;
  flex-grow: 1;
  display: flex;
  flex-direction: column;
  background: linear-gradient(to bottom, transparent, rgba(0,0,0,0.02));
}

.card-title {
  font-size: 1.1rem;
  font-weight: 800;
  margin-bottom: 4px;
  color: var(--text-main);
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.card-description {
  font-size: 0.75rem;
  color: var(--text-muted);
  line-height: 1.3;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  margin-bottom: auto;
}

.card-stats {
  display: flex;
  justify-content: space-around;
  align-items: center;
  margin-top: 8px;
  padding-top: 8px;
  border-top: 1px dashed rgba(0,0,0,0.1);
}

.stat-item {
  text-align: center;
}

.stat-label {
  display: block;
  font-size: 0.6rem;
  font-weight: 700;
  color: var(--text-muted);
}

.stat-value {
  font-size: 0.9rem;
  font-weight: 900;
  color: var(--card-theme-color);
}

.stat-divider {
  width: 1px;
  height: 20px;
  background: rgba(0,0,0,0.1);
}

.admin-actions {
  position: absolute;
  top: 0; left: 0; width: 100%; height: 100%;
  background: rgba(0,0,0,0.6);
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  opacity: 0;
  transition: opacity 0.3s ease;
  backdrop-filter: blur(2px);
  z-index: 3;
}

.pokemon-card:hover .admin-actions {
  opacity: 1;
}

.btn-action {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  border: none;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: transform 0.2s ease;
}

.btn-action:hover {
  transform: scale(1.1);
}

.btn-action.edit { background: var(--accent); color: white; }
.btn-action.delete { background: var(--error); color: white; }

/* Dark Mode Adjustments */
:root.dark .pokemon-card {
  background: var(--surface);
  border-color: #ffd700;
}

:root.dark .card-image-wrapper {
  background: #1a202c;
}
</style>
