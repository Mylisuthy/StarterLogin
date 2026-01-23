<template>
  <div class="magic-card-container">
    <div class="magic-card">
      <div class="card-front">
        <img :src="imageUrl" :alt="title" class="card-image" />
        <h3 class="card-title">{{ title }}</h3>
      </div>
      <div class="card-back">
        <h3 class="back-title">{{ title }}</h3>
        <p class="card-description">{{ description }}</p>
        <div class="card-actions" v-if="isAdmin">
            <slot name="actions"></slot>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  title: string;
  description: string;
  imageUrl: string;
  isAdmin?: boolean;
}>();
</script>

<style scoped>
.magic-card-container {
  perspective: 1000px;
  width: 300px;
  height: 420px;
  cursor: pointer;
}

.magic-card {
  position: relative;
  width: 100%;
  height: 100%;
  text-align: center;
  transition: transform 0.8s;
  transform-style: preserve-3d;
  box-shadow: 0 4px 8px rgba(0,0,0,0.2);
  border-radius: 15px;
}

.magic-card-container:hover .magic-card {
  transform: rotateY(180deg);
}

.card-front, .card-back {
  position: absolute;
  width: 100%;
  height: 100%;
  -webkit-backface-visibility: hidden;
  backface-visibility: hidden;
  border-radius: 15px;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

.card-front {
  background-color: #1a1a1a;
  color: white;
}

.card-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.card-title {
  position: absolute;
  bottom: 20px;
  background: rgba(0, 0, 0, 0.7);
  width: 100%;
  padding: 10px;
  margin: 0;
  font-size: 1.2rem;
}

.card-back {
  background-color: black;
  color: white;
  transform: rotateY(180deg);
  padding: 20px;
  border: 1px solid #333;
}

.back-title {
  margin-bottom: 15px;
  font-size: 1.5rem;
  color: #fff;
}

.card-description {
  font-size: 1rem;
  line-height: 1.5;
  color: #ccc;
}

.card-actions {
    margin-top: 20px;
    display: flex;
    gap: 10px;
}
</style>
