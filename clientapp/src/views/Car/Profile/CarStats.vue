<template>
    <h4>Stats will be collected soon...</h4>

    <MileageChart :key="mileageChartKey" :labels="mileageDates" :values="mileageValues" />
</template>

<script>
import axios from 'axios'
import MileageChart from '../../../components/Car/Profile/Stats/MileageChart.vue'



export default {
    name: 'CarStats',
    components: {
        MileageChart
    },
    data() {
        return {
            mileageChartKey: 0,
            mileageDates: [],
            mileageValues: []
        }
    },
    async created() {
        const result = await axios.get(`/api/mileages/getByCar/${this.$route.params.id}`);
        const mileages = result.data;
        this.mileageDates = mileages.map(m => m.date).reverse();
        this.mileageValues = mileages.map(m => m.odometerValue).reverse();
        this.mileageForceRerender();
    },
    methods: {
        mileageForceRerender() {
            this.mileageChartKey += 1;
        },
    }
}
</script>
