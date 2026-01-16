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
    (error) => {
        if (error.response?.status === 401) {
            localStorage.removeItem('auth_token');
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

export default api;
