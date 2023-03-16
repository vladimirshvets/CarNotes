import { createRouter, createWebHistory } from 'vue-router'
import CarsList from './views/Car/CarsList.vue'
import CarProfile from './views/Car/CarProfile.vue'
import CarStats from './views/Car/Profile/CarStats'
import RefuelingsList from './views/Car/Profile/RefuelingsList.vue'
import WashingsList from './views/Car/Profile/WashingsList.vue'

// Router
const routes = [
    {
        path: '/',
        name: 'Home',
        component: CarsList
    },
    {
        path: '/cars',
        name: 'Cars',
        component: CarsList
    },
    {
        path: '/cars/profile/:id',
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
            }
        ]
    },
    {
        path: '/:pathMatch(.*)*',
        name: 'NotFound',
        component: () => import('./views/NotFound.vue')
    }
]

export const router = createRouter({
    history: createWebHistory(),
    routes: routes,
    linkActiveClass: 'router-link-active'
})
