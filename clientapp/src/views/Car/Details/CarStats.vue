<template>
    <photo-slider />
    <car-summary :carInstance="carInstance"/>
    <distance-stats :carInstance="carInstance" />
    <money-spendings />
    <mileage-chart :key="mileageChartKey" :labels="mileageDates" :values="mileageValues" />
</template>

<script>
import { mapGetters } from 'vuex';
import PhotoSlider from '@/components/Car/Details/CarStats/PhotoSlider.vue';
import CarSummary from '@/components/Car/Details/CarStats/CarSummary.vue';
import DistanceStats from '@/components/Car/Details/CarStats/DistanceStats.vue';
import MoneySpendings from '@/components/Car/Details/CarStats/MoneySpendings.vue';
import MileageChart from '@/components/Car/Details/CarStats/MileageChart.vue';
export default {
    name: 'CarStats',
    props: {
        carInstance: Object
    },
    components: {
        PhotoSlider,
        CarSummary,
        DistanceStats,
        MoneySpendings,
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
    }
}
</script>

<style lang="less">
.stats-section {
    padding: 3em;

    &.section-dark {
        background-color: rgba(1, 106, 89, 0.1);
    }

    .section-title {
        letter-spacing: 0.1em;
        text-transform: uppercase;
        text-align: center;
        font-size: 28px;
        font-weight: 200;
        padding-bottom: 1.5em;
    }
}
</style>
