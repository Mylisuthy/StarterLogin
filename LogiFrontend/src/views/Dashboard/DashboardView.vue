<template>
  <div class="dashboard-layout">
    <Navbar />

    <main class="container py-4">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2 class="text-white fw-bold">Panel de Administración</h2>
        <button class="btn btn-primary btn-lg" @click="openModal()">
          <i class="bi bi-plus-lg"></i> Nueva Carta
        </button>
      </div>

      <!-- Loading State -->
      <div v-if="loading" class="text-center text-white py-5">
        <div class="spinner-border" role="status"></div>
      </div>

      <!-- Empty State -->
      <div v-else-if="cards.length === 0" class="text-center text-muted py-5">
        <h4>No hay cartas creadas. ¡Crea la primera!</h4>
      </div>

      <!-- Cards Grid -->
      <div v-else class="row g-4">
        <div class="col-auto" v-for="card in cards" :key="card.id">
          <MagicCard 
            :title="card.title" 
            :description="card.description" 
            :image-url="card.imageUrl"
            :is-admin="true"
          >
            <template #actions>
              <button class="btn btn-sm btn-warning" @click.stop="openModal(card)">Editar</button>
              <button 
                class="btn btn-sm" 
                :class="card.isPublished ? 'btn-secondary' : 'btn-success'"
                @click.stop="togglePublish(card)"
              >
                {{ card.isPublished ? 'Despublicar' : 'Publicar' }}
              </button>
              <button class="btn btn-sm btn-danger" @click.stop="deleteCard(card.id)">Eliminar</button>
            </template>
          </MagicCard>
          
          <!-- Status Badge Overlay (Optional enhancement) -->
          <div class="text-center mt-2">
            <span class="badge" :class="card.isPublished ? 'bg-success' : 'bg-secondary'">
                {{ card.isPublished ? 'PÚBLICA' : 'BORRADOR' }}
            </span>
          </div>
        </div>
      </div>
    </main>

    <!-- Create/Edit Modal (Custom Overlay) -->
    <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content glass-modal">
          <div class="modal-header border-0">
            <h5 class="modal-title text-white">{{ editingId ? 'Editar Carta' : 'Nueva Carta Mágica' }}</h5>
            <button type="button" class="btn-close btn-close-white" @click="closeModal"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="saveCard">
              <div class="mb-3">
                <label class="form-label text-white-50">Título</label>
                <input v-model="form.title" type="text" class="form-control bg-dark text-white border-secondary" required>
              </div>
              <div class="mb-3">
                <label class="form-label text-white-50">Descripción</label>
                <textarea v-model="form.description" class="form-control bg-dark text-white border-secondary" rows="3" required></textarea>
              </div>
              <div class="mb-3">
                <label class="form-label text-white-50">Imagen</label>
                <input type="file" ref="fileInput" class="form-control bg-dark text-white border-secondary" accept="image/*" @change="handleFileChange">
                <div v-if="form.imageUrl && !selectedFile" class="mt-2 text-center">
                    <img :src="form.imageUrl" class="img-thumbnail bg-dark border-secondary" style="max-height: 100px;">
                    <small class="d-block text-muted">Imagen actual</small>
                </div>
              </div>
              <div class="d-grid gap-2">
                <button type="submit" class="btn btn-primary" :disabled="saving">
                    <span v-if="saving" class="spinner-border spinner-border-sm me-2"></span>
                    {{ editingId ? 'Guardar Cambios' : 'Crear Carta' }}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import Navbar from '../../components/Navbar.vue';
import MagicCard from '../../components/MagicCard.vue';
import { useToastStore } from '../../stores/toast';
import { magicCardsApi, type MagicCard as MagicCardType } from '../../services/magicCards';

const toast = useToastStore();

const cards = ref<MagicCardType[]>([]);
const loading = ref(true);
const saving = ref(false);

// Modal state
const showModal = ref(false);
const editingId = ref<string | null>(null);
const fileInput = ref<HTMLInputElement | null>(null);
const selectedFile = ref<File | null>(null);

const form = reactive({
    title: '',
    description: '',
    imageUrl: ''
});

const loadCards = async () => {
    loading.value = true;
    try {
        // Fetch all (including drafts) since we are admin
        cards.value = await magicCardsApi.getAll(true);
    } catch (error) {
        toast.show('Error al cargar cartas', 'error');
    } finally {
        loading.value = false;
    }
};

onMounted(() => {
    loadCards();
});

// Modal Actions
const openModal = (card?: MagicCardType) => {
    if (card) {
        editingId.value = card.id;
        form.title = card.title;
        form.description = card.description;
        form.imageUrl = card.imageUrl;
    } else {
        editingId.value = null;
        form.title = '';
        form.description = '';
        form.imageUrl = '';
    }
    selectedFile.value = null;
    if (fileInput.value) fileInput.value.value = '';
    showModal.value = true;
};

const closeModal = () => {
    showModal.value = false;
};

const handleFileChange = (event: Event) => {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
        const file = target.files[0];
        if (file) {
             selectedFile.value = file;
        }
    }
};

const saveCard = async () => {
    if (!form.title || !form.description) return;
    if (!editingId.value && !selectedFile.value) {
        toast.show('La imagen es obligatoria para nuevas cartas', 'info');
        return;
    }

    saving.value = true;
    const formData = new FormData();
    formData.append('title', form.title);
    formData.append('description', form.description);
    if (selectedFile.value) {
        formData.append('image', selectedFile.value);
    }

    try {
        if (editingId.value) {
            await magicCardsApi.update(editingId.value, formData);
            toast.show('Carta actualizada', 'success');
        } else {
            await magicCardsApi.create(formData);
            toast.show('Carta creada mágicamente', 'success');
        }
        closeModal();
        loadCards();
    } catch (error) {
        toast.show('Error al guardar la carta', 'error');
    } finally {
        saving.value = false;
    }
};

const togglePublish = async (card: MagicCardType) => {
    try {
        const newState = !card.isPublished;
        await magicCardsApi.publish(card.id, newState);
        card.isPublished = newState;
        toast.show(newState ? 'Carta publicada' : 'Carta ocultada', 'info');
    } catch (error) {
        toast.show('Error al cambiar estado', 'error');
    }
};

const deleteCard = async (id: string) => {
    if (!confirm('¿Estás seguro de eliminar esta carta?')) return;
    try {
        await magicCardsApi.delete(id);
        cards.value = cards.value.filter(c => c.id !== id);
        toast.show('Carta eliminada', 'success');
    } catch (error) {
        toast.show('Error al eliminar', 'error');
    }
};
</script>

<style scoped>
.dashboard-layout {
  min-height: 100vh;
  background-color: var(--background);
}

.modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.7);
    z-index: 1050;
    display: flex;
    align-items: center;
    justify-content: center;
}

.glass-modal {
    background: rgba(30, 30, 30, 0.95);
    backdrop-filter: blur(10px);
    border: 1px solid rgba(255, 255, 255, 0.1);
}
</style>
