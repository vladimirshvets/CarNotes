// App
import { createApp } from 'vue'
import App from './App.vue'

// Router
import { router } from './router'

// Vuetify
import { vuetify } from './vuetify'

// Vuex store
import { store } from './store/index.js'

createApp(App)
    .use(router)
    .use(vuetify)
    .use(store)
    .mount('#app')
