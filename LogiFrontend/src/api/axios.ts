import axios from 'axios';

const api = axios.create({
    baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5901/api',
    headers: {
        'Content-Type': 'application/json',
    },
});

// Interceptor para inyectar Token
api.interceptors.request.use((config) => {
    const token = localStorage.getItem('auth_token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// Interceptor para manejo de errores global
api.interceptors.response.use(
    (response) => response,
<<<<<<< HEAD
    async (error) => {
        const originalRequest = error.config;

        // Evitar bucle infinito si la petición de refresh falla
        if (originalRequest.url.includes('/auth/refresh')) {
            const { useAuthStore } = await import('../stores/auth');
            const auth = useAuthStore();
            auth.logout();
            window.location.href = '/login';
            return Promise.reject(error);
        }

        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;

            try {
                const { useAuthStore } = await import('../stores/auth');
                const auth = useAuthStore();

                const refreshed = await auth.silentRefresh();

                if (refreshed) {
                    originalRequest.headers.Authorization = `Bearer ${auth.token}`;
                    return api(originalRequest);
                }
            } catch (err) {
                // Falló el refresh
            }

            const { useAuthStore } = await import('../stores/auth');
            const auth = useAuthStore();
            auth.logout();
            window.location.href = '/login';
=======
    (error) => {
        let message = 'Ocurrió un error inesperado.';
        let type = 'system'; // 'user' o 'system'

        if (!error.response) {
            // Error de Red (Servidor apagado o inaccesible)
            message = 'No se pudo conectar con el servidor. Verifique que el Backend esté encendido (Puerto 5901).';
            type = 'network';
        } else {
            const status = error.response.status;
            const data = error.response.data;

            if (status === 401) {
                localStorage.removeItem('auth_token');
                if (window.location.pathname !== '/login') {
                    window.location.href = '/login';
                }
                message = 'Su sesión ha expirado. Por favor, ingrese de nuevo.';
            } else if (data && data.detail) {
                // Mensaje enviado por nuestro ExceptionHandlingMiddleware
                message = data.detail;
                type = status >= 400 && status < 500 ? 'user' : 'system';
            }
>>>>>>> origin/test
        }

        // Adjuntamos el mensaje formateado al objeto de error para que la UI lo use
        error.friendlyMessage = message;
        error.errorType = type;

        return Promise.reject(error);
    }
);

export default api;
