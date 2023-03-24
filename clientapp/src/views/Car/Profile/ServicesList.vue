<template>
    <div class="tab-wrap" id="car-services">
        <div class="summary-wrap">
            <div>
                <div>Total:</div>
                <div>BYN {{ totalAmountSum.toFixed(2) }} | USD {{ baseTotalAmountSum.toFixed(2) }}</div>
            </div>
        </div>

        <div class="grid-wrap">
            <ServicesGrid :services="serviceItems" />
        </div>
    </div>
</template>

<script>
import axios from 'axios'
import ServicesGrid from '../../../components/Car/Profile/ServicesGrid.vue'

export default {
    name: 'ServicesList',
    components: {
        ServicesGrid
    },
    computed: {
        totalAmountSum() {
            return this.serviceItems.reduce(
                (sum, item) => sum + Number(item.totalAmount),
                0
            )
        },
        baseTotalAmountSum() {
            return 0;
        }
    },
    data() {
        return {
            serviceItems: []
        }
    },
    async created() {
        const result = await axios.get(`/api/services/getByCar/${this.$route.params.id}`);
        const services = result.data;
        this.serviceItems = services;
    }
}
</script>
