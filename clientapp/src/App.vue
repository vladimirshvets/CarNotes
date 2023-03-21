<template>
    <v-app>
        <NavBar />
        <OverlayComponent />
        <v-main>
            <router-view></router-view>
        </v-main>
    </v-app>

</template>

<script>
import NavBar from './components/NavBar.vue'
import OverlayComponent from './components/OverlayComponent.vue'

export default {
    name: 'App',
    components: {
        NavBar,
        OverlayComponent
    }
}
</script>

<style>
#app {

}
</style>
