<template>
    <div class="tab-wrap" id="car-washings">
        <div class="summary-wrap">
            <div>
                <div>Total:</div>
                <div>BYN {{ totalAmountSum.toFixed(2) }} | USD {{ baseTotalAmountSum.toFixed(2) }}</div>
            </div>
        </div>

        <div class="grid-wrap">
            <WashingsGrid :washings="washingItems" />
        </div>
    </div>
</template>

<script>
import axios from 'axios'
import WashingsGrid from '../../../components/Car/Profile/WashingsGrid.vue'

export default {
    name: 'WashingsList',
    components: {
        WashingsGrid
    },
    computed: {
        totalAmountSum() {
            return this.washingItems.reduce(
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
            washingItems: []
        }
    },
    async created() {
        const result = await axios.get('/api/washings/getByCar/' + this.$route.params.id);
        const washings = result.data;
        this.washingItems = washings;
    }
}
</script>
