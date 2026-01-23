<template>
  <div class="admin-layout min-vh-100 gradient-bg pb-5">
    <Navbar />
    
    <div class="container py-4">
      <header class="d-flex justify-content-between align-items-center mb-4 px-3">
        <div>
          <h1 class="h2 fw-bold text-white mb-1">Administración de Cartas</h1>
          <p class="text-white-50">Gestiona la colección y visibilidad de las tarjetas</p>
        </div>
        <button @click="openCreateModal" class="btn-enterprise d-flex align-items-center gap-2">
          <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" viewBox="0 0 16 16">
            <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z"/>
          </svg>
          Nueva Carta
        </button>
      </header>

      <!-- Loading State -->
      <div v-if="loading" class="d-flex justify-content-center align-items-center py-5">
        <div class="spinner-enterprise"></div>
      </div>

      <!-- Card Grid -->
      <div v-else class="card-grid">
        <PokemonCard 
          v-for="card in cards" 
          :key="card.id" 
          :card="card"
          :isAdmin="true"
          @edit="openEditModal"
          @delete="handleDelete"
        />
      </div>
    </div>

    <!-- Edit/Create Modal -->
    <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
      <div class="glass-card modal-content p-4 animate-scale-up">
        <h3 class="h4 fw-bold mb-4">{{ isEditing ? 'Editar Carta' : 'Crear Nueva Carta' }}</h3>
        
        <form @submit.prevent="saveCard" class="form-grid">
          <div class="form-group span-2">
            <label>Título de la Carta</label>
            <input v-model="form.title" type="text" class="input-enterprise" required placeholder="Ej: Dragonite GX">
          </div>

          <div class="form-group span-2">
            <label>Imagen (Cloudinary)</label>
            <div class="d-flex gap-2">
              <input v-model="form.imageUrl" type="text" class="input-enterprise" required placeholder="URL de la imagen o Sube una">
              <button type="button" @click="triggerFileUpload" class="btn-enterprise-outline" :disabled="uploading">
                {{ uploading ? 'Subiendo...' : 'Subir' }}
              </button>
            </div>
            <input type="file" ref="fileInput" @change="handleFileUpload" class="d-none" accept="image/*">
          </div>

          <div class="form-group span-2">
            <label>Descripción</label>
            <textarea v-model="form.description" class="input-enterprise" rows="3" placeholder="Describe los poderes de esta carta..."></textarea>
          </div>

          <div class="form-group">
            <label>HP</label>
            <input v-model.number="form.hp" type="number" class="input-enterprise" required>
          </div>
          <div class="form-group">
            <label>Ataque</label>
            <input v-model.number="form.attack" type="number" class="input-enterprise" required>
          </div>
          <div class="form-group">
            <label>Defensa</label>
            <input v-model.number="form.defense" type="number" class="input-enterprise" required>
          </div>

          <div class="form-group d-flex align-items-center gap-2">
            <input v-model="form.isPublished" type="checkbox" id="isPublished" class="form-check-input">
            <label for="isPublished" class="mb-0">Publicar Carta</label>
          </div>

          <div class="form-actions span-2 mt-4 d-flex gap-2">
            <button type="button" @click="closeModal" class="btn-enterprise-outline flex-grow-1">Cancelar</button>
            <button type="submit" class="btn-enterprise flex-grow-1" :disabled="saving">
              {{ saving ? 'Guardando...' : 'Guardar Carta' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from '../../api/axios';
import Navbar from '../../components/Navbar.vue';
import PokemonCard from '../../components/PokemonCard.vue';

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
const showModal = ref(false);
const isEditing = ref(false);
const uploading = ref(false);
const saving = ref(false);
const fileInput = ref<HTMLInputElement | null>(null);

const form = ref({
  id: '',
  title: '',
  imageUrl: '',
  description: '',
  hp: 100,
  attack: 50,
  defense: 50,
  isPublished: false
});

const fetchCards = async () => {
  loading.value = true;
  try {
    const response = await axios.get('/cards/all');
    cards.value = response.data;
  } catch (error) {
    console.error('Error fetching cards:', error);
  } finally {
    loading.value = false;
  }
};

onMounted(fetchCards);

const openCreateModal = () => {
  isEditing.value = false;
  form.value = {
    id: '', title: '', imageUrl: '', description: '',
    hp: 100, attack: 50, defense: 50, isPublished: false
  };
  showModal.value = true;
};

const openEditModal = (card: Card) => {
  isEditing.value = true;
  form.value = { ...card };
  showModal.value = true;
};

const closeModal = () => {
  showModal.value = false;
};

const triggerFileUpload = () => {
  fileInput.value?.click();
};

const handleFileUpload = async (event: Event) => {
  const target = event.target as HTMLInputElement;
  const file = target.files?.[0];
  if (!file) return;

  uploading.value = true;
  const formData = new FormData();
  formData.append('file', file);
  formData.append('upload_preset', 'ml_default'); // Default preset

  try {
    // Note: In a real app, use the Cloud Name from env
    const response = await fetch('https://api.cloudinary.com/v1_1/demo/image/upload', {
      method: 'POST',
      body: formData
    });
    const data = await response.json();
    form.value.imageUrl = data.secure_url;
  } catch (error) {
    console.error('Error uploading to Cloudinary:', error);
    alert('Error al subir la imagen. Verifica tu conexión.');
  } finally {
    uploading.value = false;
  }
};

const saveCard = async () => {
  saving.value = true;
  try {
    if (isEditing.value) {
      await axios.put(`/cards/${form.value.id}`, form.value);
    } else {
      await axios.post('/cards', form.value);
    }
    await fetchCards();
    closeModal();
  } catch (error) {
    console.error('Error saving card:', error);
  } finally {
    saving.value = false;
  }
};

const handleDelete = async (id: string) => {
  if (!confirm('¿Estás seguro de que quieres eliminar esta carta?')) return;
  try {
    await axios.delete(`/cards/${id}`);
    await fetchCards();
  } catch (error) {
    console.error('Error deleting card:', error);
  }
};
</script>

<style scoped>
.card-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 2rem;
  justify-items: center;
}

.modal-overlay {
  position: fixed;
  top: 0; left: 0; width: 100%; height: 100%;
  background: rgba(0,0,0,0.8);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 2000;
  backdrop-filter: blur(8px);
}

.modal-content {
  width: 100%;
  max-width: 600px;
  max-height: 90vh;
  overflow-y: auto;
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  gap: 1.25rem;
}

.span-2 { grid-column: span 3; }

.form-group label {
  display: block;
  font-size: 0.8rem;
  font-weight: 600;
  text-transform: uppercase;
  color: var(--text-muted);
  margin-bottom: 0.5rem;
}

.btn-enterprise-outline {
  border: 2px solid var(--accent);
  color: var(--accent);
  background: transparent;
  padding: 0.75rem 1.5rem;
  border-radius: 0.5rem;
  font-weight: 600;
  transition: var(--transition-smooth);
}

.btn-enterprise-outline:hover {
  background: var(--accent);
  color: white;
}

.animate-scale-up {
  animation: scaleUp 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

@keyframes scaleUp {
  from { transform: scale(0.9); opacity: 0; }
  to { transform: scale(1); opacity: 1; }
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
</style>
