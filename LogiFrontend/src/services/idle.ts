import { useAuthStore } from '../stores/auth';
import { useToastStore } from '../stores/toast';

const IDLE_LIMIT_MS = 15 * 60 * 1000; // 15 minutes
let idleTimer: any = null;
let lastActivity = Date.now();

function resetTimer() {
    lastActivity = Date.now();
    // Optimización: Si el timer ya está corriendo, no necesitamos reiniciarlo constantemente,
    // solo actualizamos lastActivity. El intervalo verificará la diferencia.
}

function checkIdle() {
    const auth = useAuthStore();
    if (!auth.isAuthenticated) return;

    if (Date.now() - lastActivity > IDLE_LIMIT_MS) {
        auth.logout();
        const toast = useToastStore();
        toast.show('Sesión cerrada por inactividad', 'info');
        window.location.href = '/login';
    }
}

export const IdleService = {
    start() {
        if (idleTimer) return;

        window.addEventListener('mousemove', resetTimer);
        window.addEventListener('keydown', resetTimer);
        window.addEventListener('click', resetTimer);
        window.addEventListener('scroll', resetTimer);

        idleTimer = setInterval(checkIdle, 60000); // Check every minute
    },

    stop() {
        if (!idleTimer) return;

        window.removeEventListener('mousemove', resetTimer);
        window.removeEventListener('keydown', resetTimer);
        window.removeEventListener('click', resetTimer);
        window.removeEventListener('scroll', resetTimer);

        clearInterval(idleTimer);
        idleTimer = null;
    }
};
