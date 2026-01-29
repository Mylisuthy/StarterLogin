# LogiFrontend: Guia de Arquitectura y Desarrollo de Interfaz

Este directorio contiene la aplicación cliente de StarterLogin, una interfaz reactiva de alto rendimiento construida con Vue 3 y Vite para garantizar una experiencia de usuario fluida y eficiente.

## Parametros de Ejecucion y Acceso

El frontend está configurado para operar en sincronía con el ecosistema de microservicios y contenedores:

*   **Puerto de Desarrollo (Local)**: 5173
*   **Puerto Expuesto (Docker)**: 5900
*   **URL de Acceso Local**: http://localhost:5900
*   **Comunicacion Base**: http://localhost:5901/api (Configurable vía variable de entorno)

## Arquitectura del Proyecto (Modular & Reactiva)

La estructura del código sigue las mejores prácticas de Vue 3, centrada en la composición y la gestión de estado atómica.

### 1. Nucleo de Comunicacion (src/api)
Gestiona todas las peticiones salientes hacia el backend.
*   **Instancia de Axios**: Configurada con tiempos de espera y parámetros base.
*   **Interceptores**: Implementación automática de la inyección del token JWT en las cabeceras de cada petición y captura centralizada de errores 401 para redirección al login.

### 2. Gestion de Estado Global (src/stores)
Utiliza Pinia para mantener la consistencia del estado en toda la aplicación.
*   **Auth Store**: Gestiona la identidad del usuario, los tokens de sesión y los permisos de acceso.
*   **UI Store**: Controla estados visuales efímeros como notificaciones (toasts) y estados de carga globales.

### 3. Capa de Servicios (src/services)
Abstrae la lógica de comunicación para desacoplarla de los componentes de la interfaz.
*   **Media Service**: Métodos para recuperación de catálogos, filtrado por géneros y búsqueda multimedia.
*   **Auth Service**: Centraliza las operaciones de registro, inicio de sesión y gestión de perfiles.

---

## Herramientas de Desarrollo y Optimizacion (Expert Mode)

El flujo de trabajo en el frontend requiere un manejo preciso de las dependencias y la compilación.

### Comandos de Mantenimiento de Dependencias
*   **Limpieza de Cache de NPM**: `npm cache clean --force`
*   **Auditoría de Seguridad**: `npm audit fix`
*   **Actualizacion Segura**: `npm update`
*   **Remover e Instalar desde Cero**:
    ```bash
    rm -rf node_modules package-lock.json && npm install
    ```

### Depuracion y Diagnostico (Pro Tips)
*   **Vue Devtools**: Es imprescindible tener instalada la extensión oficial para inspeccionar Props, Events y el estado de Pinia.
*   **Inspeccion de Red**: Usa la pestaña "Network" del navegador para verificar que el header `Authorization` se está enviando como `Bearer <token>`.
*   **Debug de Interceptores**: Puedes añadir un `console.log` en `src/api/axios.ts` para ver todas las respuestas fallidas del servidor y debuggear errores 500 o 400 de forma centralizada.

---

## Despliegue y Pruebas de Produccion

Antes de llevar el código a producción, es vital validar la compilación final.

*   **Typescript Check**: Ejecuta `vue-tsc --noEmit` para verificar errores de tipo en toda la aplicación antes de compilar.
*   **Prueba de Build Local**:
    ```bash
    npm run build
    npx serve dist -s
    ```
    Esto permite probar el comportamiento de la aplicación optimizada (minificada) antes de subirla a un servidor real.

---

## Solucion de Problemas Comunes (Troubleshooting)

### Variables de Entorno (.env)
*   **Problema**: Los cambios en `.env` no se reflejan.
*   **Solucion**: Reinicia el servidor de desarrollo (`npm run dev`) cada vez que modifiques el archivo `.env`. Vite solo carga estas variables al iniciar.
*   **Prefijo VITE_**: Recuerda que solo las variables que empiezan por `VITE_` son accesibles desde el código frontend por seguridad.

### Errores de CORS
Si el navegador bloquea las peticiones:
1.  Verifica que `VITE_API_URL` apunte a la dirección correcta (ej: `http://localhost:5901/api`).
2.  Asegúrate de que el backend tenga configurado `AllowOrigins` para el puerto del frontend (5900 o 5173).

### Problemas con el Estado (Pinia)
Si el Dashboard no muestra datos tras el login:
*   Verifica que el `tokenRef` en el AuthStore se esté actualizando.
*   Usa `localStorage.getItem('token')` en la consola del navegador para confirmar que el token persiste.

---

## Comandos Utiles por Casos de Uso

### Desarrollo Rapido
*   **Preview de Componentes**: Instala `vite-plugin-vue-devtools` para una barra flotante de depuración en la propia página.
*   **Linting Automatico**: `npm run lint -- --fix` (Corrige automáticamente problemas de formato).

### Optimizacion de Recursos
*   **Analizador de Bundle**: Puedes añadir `rollup-plugin-visualizer` para ver qué librerías ocupan más espacio en tu compilación final.

---

## Seguridad en el Cliente

El frontend nunca almacena credenciales de usuario. El token JWT se gestiona de forma segura y se utiliza exclusivamente para autorizar peticiones. La aplicación detecta automáticamente la expiración del token y solicita una nueva autenticación para proteger la integridad de los datos del usuario.
