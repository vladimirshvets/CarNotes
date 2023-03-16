<template>

    <v-app-bar
        id="nav-bar"
        :elevation="2"
        density="compact"
    >
        <v-app-bar-title>
            <router-link class="title" :to="{ name: 'Home' }">
                CAR NOTES
            </router-link>
        </v-app-bar-title>
    </v-app-bar>


</template>

<script>
export default {
    name: 'NavBar',
}
</script>

<style scoped>
    #nav-bar {
        background-color: #016a59;
        color: #fff;
        border-bottom: 1px solid #ddd;
    }

    .v-toolbar-title .title {
        text-decoration: none;
        cursor: pointer;
        color: #fff;
    }
</style>
