import { store } from './store'
import { createRouter, createWebHistory } from 'vue-router'
import HomePage from './views/HomePage.vue'
import LoginPage from './views/User/LoginPage.vue'
import RegisterPage from './views/User/RegisterPage.vue'
import UserProfile from './views/User/UserProfile.vue'
import CarList from './views/Car/CarList.vue'
import CarProfile from './views/Car/CarProfile.vue'
import CarDetails from './views/Car/CarDetails.vue'
import CarStats from './views/Car/Details/CarStats.vue'
import LegalProceduresList from './views/Car/Details/LegalProceduresList.vue'
import RefuelingsList from './views/Car/Details/RefuelingsList.vue'
import ServicesList from './views/Car/Details/ServicesList.vue'
import SparePartsList from './views/Car/Details/SparePartsList.vue'
import TextNotesList from './views/Car/Details/TextNotesList.vue'
import WashingsList from './views/Car/Details/WashingsList.vue'

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
        path: '/account/login',
        name: 'Login',
        component: LoginPage,
        meta: {
            title: 'Login',
            requiresAuth: false
        },
        beforeEnter: (to, from, next) => {
            if (store.getters.isAuthenticated) {
                next({ name: 'Home' })
            } else {
                next()
            }
        }
    },
    {
        path: '/account/logout',
        name: 'Logout',
        beforeEnter: (to, from, next) => {
            if (store.getters.isAuthenticated) {
                store.dispatch('logout')
                    .then(() => {
                        next({ name: 'Home' })
                    })
                    .catch(error => {
                        console.log(`Logout error: ${error}`)
                    });
            } else {
                next({ name: 'Home' });
            }
        }
    },
    {
        path: '/account/register',
        name: 'Register',
        component: RegisterPage,
        meta: {
            title: 'Register',
            requiresAuth: false
        },
        beforeEnter: (to, from, next) => {
            if (store.getters.isAuthenticated) {
                next({ name: 'Home' })
            } else {
                next()
            }
        }
    },
    {
        path: '/account',
        name: 'Profile',
        component: UserProfile,
        meta: {
            title: 'Driver Profile',
            requiresAuth: true
        }
    },
    {
        path: '/cars',
        name: 'Cars',
        component: CarList,
        meta: {
            title: 'My Garage',
            requiresAuth: true
        }
    },
    {
        path: '/cars/profile/:carId?',
        name: 'CarProfile',
        component: CarProfile,
        meta: {
            title: 'Car Profile',
            requiresAuth: true
        }
    },
    {
        path: '/cars/details/:carId',
        name: 'CarDetails',
        component: CarDetails,
        meta: {
            title: 'Car Stats',
            requiresAuth: true
        },
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
            },
            {
                path: 'text-notes',
                name: 'TextNotesList',
                component: TextNotesList
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
    document.title = to.meta.title ?? 'Car Notes';
    if (to.meta.requiresAuth) {
        requireAuth(to, from, next);
    } else {
        next();
    }
});

export { router };
