<template>
    <v-banner
        color="info"
        icon="mdi-car-info"
        :stacked="false"
    >
        <div>
            <h3>Welcome to Car Notes!</h3>
        </div>
        <div>
            <span>Description of the website will appear here someday :)</span>
        </div>
        <div>
            <v-btn
                v-if="isAuthenticated"
                :to="{ name: 'Cars' }"
                size="large"
                color="info"
            >
                My garage
            </v-btn>
            <span v-else>Please <router-link :to="{ name: 'Login' }">log in</router-link> to explore some features.</span>
        </div>
        <div v-if="totalUsers">
            Total users: {{ totalUsers }}
        </div>
        <div v-if="totalCars">
            Total cars: {{ totalCars }}
        </div>
    </v-banner>
</template>

<script>
import axios from 'axios';
import { mapGetters } from 'vuex';
export default {
    name: 'HomePage',
    data() {
        return {
            totalUsers: null,
            totalCars: null
        }
    },
    computed: {
        ...mapGetters([
            'isAuthenticated'
        ])
    },
    mounted() {
        axios
            .get(`/api/stats/total-users`)
            .then(response => {
                this.totalUsers = response.data;
            })
            .catch(error => {
                console.error(error);
            });

        axios
            .get(`/api/stats/total-cars`)
            .then(response => {
                this.totalCars = response.data;
            })
            .catch(error => {
                console.error(error);
            });
    }
}
</script>

<style lang="less" scoped>
.v-banner {
    max-width: 640px;
    margin: auto;
    padding: 2em;
    font-size: 20px;

    div {
        padding-bottom: 1em;
    }
}
</style>
