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
        }

        // Adjuntamos el mensaje formateado al objeto de error para que la UI lo use
        error.friendlyMessage = message;
        error.errorType = type;

        return Promise.reject(error);
    }
);

export default api;
