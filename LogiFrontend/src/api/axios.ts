import axios from 'axios';

const api = axios.create({
    baseURL: 'https://localhost:7081/api', // Ajustar al puerto real del backend
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
        }
        return Promise.reject(error);
    }
);

export default api;
