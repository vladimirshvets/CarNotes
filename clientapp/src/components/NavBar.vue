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
            <v-menu>
                <template v-slot:activator="{ props }">
                    <v-btn v-if="isAuthenticated" icon="mdi-card-account-details-star" v-bind="props"></v-btn>
                    <v-btn v-if="!isAuthenticated" icon="mdi-login" v-bind="props"></v-btn>
                </template>

                <v-list class="account-menu">
                    <v-list-item v-if="!isAuthenticated">
                        <v-list-item-title>
                            <router-link :to="{ name: 'Login' }">Login</router-link>
                        </v-list-item-title>
                    </v-list-item>
                    <v-list-item v-if="isAuthenticated">
                        <v-list-item-title>
                            <router-link :to="{ name: 'Profile' }">Profile</router-link>
                        </v-list-item-title>
                    </v-list-item>
                    <v-list-item v-if="isAuthenticated">
                        <v-list-item-title>
                            <router-link :to="{ name: 'Logout' }">Logout</router-link>
                        </v-list-item-title>
                    </v-list-item>
                </v-list>
            </v-menu>
        </v-btn>
    </v-app-bar>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
    name: 'NavBar',
    computed: {
        ...mapGetters([
            'isAuthenticated'
        ])
    }
}
</script>

<style lang="less" scoped>
    #nav-bar {
        background-color: #016a59;
        color: #fff;
        border-bottom: 1px solid #ddd;
    }

    .account-menu {
        a {
            color: initial;
            text-decoration: none;

            &:hover {
                color: #016a59;
            }
        }
    }
</style>
