<template>
    <section class="stats-section section-dark">
        <div class="section-header">
            <div class="section-title">Distance</div>
            <div class="section-subtitle">"1/4 times around the world"</div>
        </div>
        <div class="section-content">
            <v-row>
                <v-col cols="12" md="4" sm="4">
                    <div class="circle-wrap">
                        <div class="circle-text">
                            <div class="value">{{ averageConsumption?.toFixed(2) }}</div>
                            <div class="label">l. / 100 km</div>
                        </div>
                    </div>
                </v-col>
                <v-col cols="12" md="4" sm="4">
                    <div class="circle-wrap">
                        <div class="circle-text">
                            <div class="value">{{ odometerDelta }}</div>
                            <div class="label">km total</div>
                        </div>
                    </div>
                </v-col>
                <v-col cols="12" md="4" sm="4">
                    <div class="circle-wrap">
                        <div class="circle-text">
                            <div class="value">{{ monthlyMileageStats?.toFixed(0) }}</div>
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
import moment from 'moment';
export default {
    name: 'DistanceStats',
    props: {
        carInstance: Object
    },
    data() {
        return {
            averageConsumption: 0,
            odometerDelta: 0
        }
    },
    computed: {
        monthlyMileageStats() {
            const from = this.carInstance?.ownedFrom;
            const to = this.carInstance?.ownedTo;
            const fromDate = from ? moment(from) : moment();
            const toDate = to ? moment(to) : moment();
            const duration = moment.duration(toDate.diff(fromDate));
            const days = duration.asDays();
            return days === 0 ? 0 : this.odometerDelta / days * 30.4375;
        },
    },
    created() {
        api
            .get(`/api/stats/average-consumption/${this.$route.params.carId}`)
            .then(response => {
                this.averageConsumption = response.data;
            });
        api
            .get(`/api/stats/odometer-delta/${this.$route.params.carId}`)
            .then(response => {
                this.odometerDelta = response.data;
            });
    }
}
</script>
