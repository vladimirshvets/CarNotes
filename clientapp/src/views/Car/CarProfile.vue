<template>

    <div class="tab-wrap" id="car-profile">
        <v-card class="car-card-wrapper">
            <v-card-title>
                {{ carInfo.make }} {{ carInfo.model }} {{ carInfo.generation }}
            </v-card-title>
            <v-card-subtitle>
                {{ carInfo.year }}
            </v-card-subtitle>
            <v-card-text>
                <div>{{ carInfo.vin }}</div>
                <div>{{ carInfo.plate }}</div>
            </v-card-text>
        </v-card>

        <v-card class="car-tabs-wrapper">
            <v-card-title class="bg-teal">
                <router-link class="car-profile-tab-link" :to="{ name: 'CarStats' }">Statistics</router-link> |
                <router-link class="car-profile-tab-link" :to="{ name: 'RefuelingsList' }">Refuelings</router-link> |
                <router-link class="car-profile-tab-link" :to="{ name: 'WashingsList' }">Washings</router-link>

                <v-spacer></v-spacer>
            </v-card-title>

            <div class="car-profile-tab">
                <router-view></router-view>
            </div>
        </v-card>

    </div>

</template>

<script>
import axios from 'axios'

export default {
    name: "CarProfile",
    data() {
        return {
            carInfo: {}
        }
    },
    async created() {
        const result = await axios.get(`/api/cars/${this.$route.params.id}`);
        const car = result.data;
        this.carInfo = car;
    }
}
</script>

<style scoped>
    .car-profile-tab {
        margin: 1.4em;
    }

    .car-profile-tab-link {
        text-decoration: none;
        cursor: pointer;
        color: #fff;
        font-weight: 500;
    }

    .router-link-active {
        color:#cdff07;
    }

    .car-card-wrapper {
        margin: 1em 2em;
    }

    .car-tabs-wrapper {
        margin: 0 2em;
    }
</style>
