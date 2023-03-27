<template>
    <v-app-bar
        id="nav-bar"
        :elevation="2"
        density="compact"
    >
        <v-btn
            icon
            :to="{ name: 'Home' }"
        >
            <v-icon>mdi-car-multiple</v-icon>
        </v-btn>
        <v-app-bar-title>CAR NOTES</v-app-bar-title>
        <v-spacer></v-spacer>
        <v-btn
            icon
        >
          <v-icon>mdi-card-account-details-star</v-icon>
        </v-btn>
    </v-app-bar>
</template>

<script>
export default {
    name: 'NavBar',
}
</script>

<style lang="less" scoped>
    #nav-bar {
        background-color: #016a59;
        color: #fff;
        border-bottom: 1px solid #ddd;
    }
</style>
