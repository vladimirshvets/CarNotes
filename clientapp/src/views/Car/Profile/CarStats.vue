<template>
    <div>
        Average Fuel Consumption, l. / 100 km: <span v-html="averageConsumption?.toFixed(2)"></span>
    </div>
    <div>
        Total Distance, km: <span v-html="odometerDelta"></span>
    </div>
    <div>
        Monthly Distance, km: {{ monthlyMileageStats?.toFixed(0) }}
    </div>
    <br />
    <MileageChart :key="mileageChartKey" :labels="mileageDates" :values="mileageValues" />
</template>

<script>
import api from '@/api.js';
import moment from 'moment';
import { mapGetters, mapMutations } from 'vuex';
import MileageChart from '../../../components/Car/Profile/Stats/MileageChart.vue'

export default {
    name: 'CarStats',
    props: {
        carSummary: Object
    },
    components: {
        MileageChart
    },
    data() {
        return {
            carId: this.$route.params.carId,
            averageConsumption: null,
            odometerDelta: null
        }
    },
    computed: {
        monthlyMileageStats() {
            const from = this.carSummary.ownedFrom;
            const to = this.carSummary.ownedTo;
            const fromDate = moment(from);
            const toDate = to ? moment(to) : moment();
            const duration = moment.duration(toDate.diff(fromDate));
            const days = duration.asDays();
            return days == 0 ? null : this.odometerDelta / days * 30.4375;
        },
        mileageChartKey() {
            return this.mileages.length;
        },
        mileageDates() {
            return this.mileages.map(m => m.date).reverse();
        },
        mileageValues() {
            return this.mileages.map(m => m.odometerValue).reverse();
        },
        ...mapGetters([
            'mileages'
        ])
    },
    created() {
        this.$store.dispatch('loadMileages', this.carId);
        api
            .get(`/api/stats/average-consumption/${this.carId}`)
            .then(response => {
                this.averageConsumption = response.data;
            });
        api
            .get(`/api/stats/odometer-delta/${this.carId}`)
            .then(response => {
                this.odometerDelta = response.data;
            });
    },
    methods: {
        ...mapMutations([
            'setMileages',
        ])
    }
}
</script>
