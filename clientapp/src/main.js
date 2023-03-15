// App
import { createApp } from 'vue'
import App from './App.vue'

// Router
import { router } from './router'

// Vuetify
import { vuetify } from './vuetify'

createApp(App)
    .use(router)
    .use(vuetify)
    .mount('#app')
