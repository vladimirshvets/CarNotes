<template>
    <section class="stats-section">
        <div class="section-header">
            <div class="section-title">Distance</div>
            <div class="section-subtitle">"1/4 times around the world"</div>
        </div>
        <div class="section-content">
            <v-row>
                <v-col cols="12" md="4" sm="4">
                    <div class="circle-wrap">
                        <div class="circle-text">
                            <div class="value">{{ averageFuelConsumption?.toFixed(2) }}</div>
                            <div class="label">l. / 100 km</div>
                        </div>
                    </div>
                </v-col>
                <v-col cols="12" md="4" sm="4">
                    <div class="circle-wrap">
                        <div class="circle-text">
                            <div class="value">{{ totalDistance }}</div>
                            <div class="label">km total</div>
                        </div>
                    </div>
                </v-col>
                <v-col cols="12" md="4" sm="4">
                    <div class="circle-wrap">
                        <div class="circle-text">
                            <div class="value">{{ distancePerMonth }}</div>
                            <div class="label">km / month</div>
                        </div>
                    </div>
                </v-col>
            </v-row>
        </div>
    </section>
</template>

<script>
import api from '@/api.js';
export default {
    name: 'DistanceStats',
    data() {
        return {
            totalDistance: 0,
            distancePerMonth: 0,
            averageFuelConsumption: 0
        }
    },
    created() {
        api
            .get(`/api/personal-stats/distance/${this.$route.params.carId}`)
            .then(response => {
                this.totalDistance = response.data.totalDistance;
                this.distancePerMonth = response.data.distancePerMonth;
                this.averageFuelConsumption = response.data.averageFuelConsumption;
            });
    }
}
</script>
