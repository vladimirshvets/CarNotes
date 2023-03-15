import { createRouter, createWebHistory } from 'vue-router'
import CarsList from './components/CarsList.vue'
import CarDetails from './components/CarDetails.vue'
import CarProfile from './views/CarProfile'
import RefuelingsList from './views/RefuelingsList.vue'
import WashingsList from './views/WashingsList.vue'

// Router
const routes = [
  { 
    path: '/', 
    component: CarsList 
  },
  { 
    path: '/cars/:id', 
    component: CarDetails,
    children: [
      { 
        path: '', 
        name: 'carProfile', 
        component: CarProfile 
      },
      {
        path: 'refuelings',
        name: 'carRefuelings',
        component: RefuelingsList
      },
      { 
        path: 'washings', 
        name: 'carWashings', 
        component: WashingsList 
      }
    ]
  }
]

export const router = createRouter({
  history: createWebHistory(),
  routes: routes,
})
