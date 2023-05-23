import { store } from './store'
import { createRouter, createWebHistory } from 'vue-router'
import HomePage from './views/HomePage.vue'
import LoginPage from './views/User/LoginPage.vue'
import CarsList from './views/Car/CarsList.vue'
import CarProfile from './views/Car/CarProfile.vue'
import CarStats from './views/Car/Profile/CarStats'
import LegalProceduresList from './views/Car/Profile/LegalProceduresList.vue'
import RefuelingsList from './views/Car/Profile/RefuelingsList.vue'
import ServicesList from './views/Car/Profile/ServicesList.vue'
import SparePartsList from './views/Car/Profile/SparePartsList.vue'
import WashingsList from './views/Car/Profile/WashingsList.vue'

const requireAuth = (to, from, next) => {
    if (store.getters.isAuthenticated) {
        next();
    } else {
        next({ name: 'Login' });
    }
};

// Router
const routes = [
    {
        path: '/',
        name: 'Home',
        component: HomePage
    },
    {
        path: '/account',
        name: 'Profile',
        meta: { requiresAuth: true },
        children: [
            {
                path: 'login',
                name: 'Login',
                component: LoginPage,
                meta: { requiresAuth: false },
                beforeEnter: (to, from, next) => {
                    if (store.getters.isAuthenticated) {
                        next({ name: 'Cars' })
                    } else {
                        next()
                    }
                }
            },
            {
                path: 'logout',
                name: 'Logout',
                beforeEnter: (to, from, next) => {
                    store.dispatch('logout')
                        .then(() => {
                            next({ name: 'Home' })
                        })
                        .catch(error => {
                            console.log(`Logout error: ${error}`)
                        });
                }
            }
        ]
    },
    {
        path: '/cars',
        name: 'Cars',
        component: CarsList,
        meta: { requiresAuth: true }
    },
    {
        path: '/cars/profile/:carId',
        name: 'CarProfile',
        component: CarProfile,
        children: [
            {
                path: 'stats',
                name: 'CarStats',
                component: CarStats
            },
            {
                path: 'refuelings',
                name: 'RefuelingsList',
                component: RefuelingsList
            },
            {
                path: 'washings',
                name: 'WashingsList',
                component: WashingsList
            },
            {
                path: 'spare-parts',
                name: 'SparePartsList',
                component: SparePartsList
            },
            {
                path: 'services',
                name: 'ServicesList',
                component: ServicesList
            },
            {
                path: 'legal-procedures',
                name: 'LegalProceduresList',
                component: LegalProceduresList
            }
        ]
    },
    {
        path: '/:pathMatch(.*)*',
        name: 'NotFound',
        component: () => import('./views/NotFound.vue')
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes: routes,
    linkActiveClass: 'router-link-active'
});

router.beforeEach((to, from, next) => {
    if (to.meta.requiresAuth) {
        requireAuth(to, from, next);
    } else {
        next();
    }
});

export { router };
