<template>
    <v-app>
        <NavBar />
        <OverlayComponent />
        <SnackbarComponent />
        <v-main>
            <router-view></router-view>
        </v-main>
    </v-app>

</template>

<script>
import NavBar from './components/NavBar.vue'
import OverlayComponent from './components/OverlayComponent.vue'
import SnackbarComponent from '@/components/SnackbarComponent.vue'

export default {
    name: 'App',
    components: {
        NavBar,
        OverlayComponent,
        SnackbarComponent
    }
}
</script>

<style>
#app {

}
</style>
