<template>
    <v-app>
        <nav-bar />
        <overlay-component />
        <snackbar-component />
        <v-main>
            <router-view></router-view>
        </v-main>
        <footer-component />
    </v-app>
</template>

<script>
import NavBar from '@/components/Common/NavBar.vue';
import OverlayComponent from '@/components/Common/OverlayComponent.vue';
import SnackbarComponent from '@/components/Common/SnackbarComponent.vue';
import FooterComponent from '@/components/Common/FooterComponent.vue';
export default {
    name: 'App',
    components: {
        NavBar,
        OverlayComponent,
        SnackbarComponent,
        FooterComponent
    }
}
</script>

<style>
#app {

}
</style>
