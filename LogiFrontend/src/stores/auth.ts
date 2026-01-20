import { defineStore } from 'pinia';
import api from '../api/axios';
import { useToastStore } from './toast';

interface User {
    id: string;
    userName: string;
    email: string;
    roles: string[];
}

interface AuthState {
    user: User | null;
    token: string | null;
    loading: boolean;
    error: string | null;
}

export const useAuthStore = defineStore('auth', {
    state: (): AuthState => ({
        user: JSON.parse(localStorage.getItem('user') || 'null'),
        token: localStorage.getItem('auth_token'),
        loading: false,
        error: null,
    }),

    getters: {
        isAuthenticated: (state) => !!state.token,
        isAdmin: (state) => state.user?.roles.includes('ADMIN') || false,
    },

    actions: {
        async login(credentials: any) {
            this.loading = true;
            this.error = null;
            try {
                const { data } = await api.post('/auth/login', credentials);
                this.token = data.token;
                this.user = {
                    id: data.id,
                    userName: data.userName,
                    email: data.email,
                    roles: data.roles
                };

                localStorage.setItem('auth_token', this.token!);
                localStorage.setItem('user', JSON.stringify(this.user));

                const toast = useToastStore();
                toast.show(`¡Bienvenido de nuevo, ${this.user.userName}!`, 'success');

                return true;
            } catch (err: any) {
                this.error = err.friendlyMessage || 'Error en el inicio de sesión';
                const toast = useToastStore();
                toast.show(this.error!, 'error');
                return false;
            } finally {
                this.loading = false;
            }
        },

        logout() {
            const toast = useToastStore();
            toast.show('Sesión cerrada correctamente', 'info');
            this.user = null;
            this.token = null;
            localStorage.removeItem('auth_token');
            localStorage.removeItem('user');
        }
    }
});
