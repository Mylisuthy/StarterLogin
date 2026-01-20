# 🎨 Frontend - Guía de Desarrollo e Interfaz Moderna

El frontend de **StarterLogin** es una Single Page Application (SPA) ultra-rápida construida con **Vue 3** y **Vite**.

## 🏗️ Conceptos Clave para Aprender

1.  **Composition API (Script Setup)**: Es la forma moderna de Vue para organizar lógica. Más legible y eficiente.
2.  **Estado Global (Pinia)**: Usamos "Stores" para que la información del usuario esté disponible en cualquier página sin pasar "props" infinitas.
3.  **Guardias de Navegación**: El router decide si puedes ver una página basándose en si tienes un token guardado.

---

## 🗺️ Tour Guiado del Código

### 1. El Punto de Inicio: `src/main.ts`
- **Qué hace**: Carga Vue, el Router (Navegación) y Pinia (Estado). Es el pegamento de todo.

### 2. La Central de Datos: `src/stores/auth.ts`
- **Lógica**: Aquí se procesa el login. Cuando el backend responde con un token, este store lo guarda en `localStorage` para que no se pierda al refrescar.

### 3. Las Páginas: `src/views/`
- **Estructura**: `LoginView.vue` maneja el formulario. `DashboardView.vue` muestra el contenido protegido.
- **Diseño**: Usamos variables de CSS en `src/style.css` para mantener colores consistentes y un look "premium".

### 4. Comunicación: `src/api/axios.ts`
- **Interceptor**: Verás un código que "inyecta" automáticamente el Token en cada petición al backend. ¡Tú no tienes que hacerlo a mano!

---

## 🚀 Cómo personalizar la App

- **¿Cambiar Colores?**: Edita `:root` en `src/style.css`. Todo el sistema de diseño se actualizará solo.
- **¿Añadir una Página?**: 
  1. Crea un `.vue` en `views/`.
  2. Añade la ruta en `src/router/index.ts`.
- **¿Nuevas alertas?**: Usa `useToastStore` desde cualquier componente para mostrar mensajes burbuja.

---

## 🛠️ Comandos Útiles (Frontend)
- `npm install`: Instala librerías.
- `npm run dev`: Inicia el servidor de desarrollo.
- `npm run build`: Prepara la app para producción (genera la carpeta `dist`).
