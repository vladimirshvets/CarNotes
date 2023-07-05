<template>
    <section class="stats-wrapper">
        <photo-slider />
        <car-summary :carInstance="carInstance"/>
        <distance-stats />
        <money-spendings />
        <note-history />
        <mileage-chart :key="mileageChartKey" :labels="mileageDates" :values="mileageValues" />
    </section>
</template>

<script>
import { mapGetters } from 'vuex';
import PhotoSlider from '@/components/Car/Details/Stats/PhotoSlider.vue';
import CarSummary from '@/components/Car/Details/Stats/CarSummary.vue';
import DistanceStats from '@/components/Car/Details/Stats/DistanceStats.vue';
import MoneySpendings from '@/components/Car/Details/Stats/MoneySpendings.vue';
import NoteHistory from '@/components/Car/Details/Stats/NoteHistory.vue';
import MileageChart from '@/components/Car/Details/Stats/MileageChart.vue';
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
        NoteHistory,
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
.stats-wrapper {
    .stats-section:nth-child(2n+1) {
        background-color: rgba(1, 106, 89, 0.1);
    }
}

.stats-section {
    padding: 3em;

    .section-header {
        padding-bottom: 1.5em;
        text-align: center;
        letter-spacing: 0.1em;
        font-weight: 200;

        .section-title {
            font-size: 28px;
            text-transform: uppercase;
        }

        .section-subtitle {
            font-size: 22px;
            font-style: italic;
        }
    }
}
</style>
