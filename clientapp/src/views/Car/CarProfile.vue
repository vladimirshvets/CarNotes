<template>

    <div class="tab-wrap" id="car-profile">
        <v-card class="car-card-wrapper">
            <v-card-title>
                <span>{{ carInfo.make }} {{ carInfo.model }} {{ carInfo.generation }}</span>
                <v-sheet v-if="carInfo.plate" border rounded class="plate-number">
                    {{ carInfo.plate }}
                </v-sheet>
            </v-card-title>
            <v-card-subtitle>
                {{ carInfo.year }}
            </v-card-subtitle>
            <v-card-text>
                <div v-if="carInfo.vin">* {{ carInfo.vin }} *</div>
            </v-card-text>
        </v-card>

        <v-card class="car-tabs-wrapper">
            <v-card-title class="bg-teal">
                <router-link class="car-profile-tab-link" :to="{ name: 'CarStats' }">Statistics</router-link> |
                <router-link class="car-profile-tab-link" :to="{ name: 'RefuelingsList' }">Refuelings</router-link> |
                <router-link class="car-profile-tab-link" :to="{ name: 'WashingsList' }">Washings</router-link> |
                <router-link class="car-profile-tab-link" :to="{ name: 'SparePartsList' }">Spare Parts</router-link> |
                <router-link class="car-profile-tab-link" :to="{ name: 'ServicesList' }">Services</router-link> |
                <router-link class="car-profile-tab-link" :to="{ name: 'LegalProceduresList' }">Legal Procedures</router-link>

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
    .car-card-wrapper {
        margin: 1em 2em;
    }

    .plate-number {
        display: inline;
        margin-left: 10px;
        padding: 2px;
        height: 25px;
        width: 80px;
        font-size: 16px;
    }

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

    .car-tabs-wrapper {
        margin: 0 2em;
    }
</style>
