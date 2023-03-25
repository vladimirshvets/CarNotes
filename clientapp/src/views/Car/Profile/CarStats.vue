<template>
    <MileageChart :key="mileageChartKey" :labels="mileageDates" :values="mileageValues" />
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import MileageChart from '../../../components/Car/Profile/Stats/MileageChart.vue'

export default {
    name: 'CarStats',
    components: {
        MileageChart
    },
    computed: {
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
        this.$store.dispatch('loadMileages', this.$route.params.carId);
    },
    methods: {
        ...mapMutations([
            'setMileages',
        ])
    }
}
</script>
