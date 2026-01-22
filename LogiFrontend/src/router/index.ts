import { createRouter, createWebHistory } from 'vue-router';
import LoginView from '../views/Auth/LoginView.vue';
import RegisterView from '../views/Auth/RegisterView.vue';
import DashboardView from '../views/Dashboard/DashboardView.vue';
import ProfileView from '../views/Profile/ProfileView.vue';
import { useAuthStore } from '../stores/auth';

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: '/', redirect: '/dashboard' },
        { path: '/login', component: LoginView, meta: { guest: true } },
        { path: '/register', component: RegisterView, meta: { guest: true } },
        {
            path: '/dashboard',
            component: DashboardView,
            meta: { requiresAuth: true }
        },
        {
            path: '/profile',
            component: ProfileView,
            meta: { requiresAuth: true }
        },
        {
            path: '/gallery',
            component: () => import('../views/GalleryView.vue'),
            meta: { requiresAuth: true }
        },
        {
            path: '/admin/cards',
            component: () => import('../views/Admin/CardDashboardView.vue'),
            meta: { requiresAuth: true, requiresAdmin: true }
        },
    ]
});

router.beforeEach((to, _from, next) => {
    const auth = useAuthStore();

    if (to.meta.requiresAuth && !auth.isAuthenticated) {
        next('/login');
    } else if (to.meta.requiresAdmin && !auth.user?.roles.includes('Admin')) {
        next('/dashboard');
    } else if (to.meta.guest && auth.isAuthenticated) {
        next('/dashboard');
    } else {
        next();
    }
});

export default router;
