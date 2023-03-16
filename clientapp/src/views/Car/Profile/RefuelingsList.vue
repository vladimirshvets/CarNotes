<template>
    <div class="tab-wrap" id="car-refuelings">
        <div class="summary-wrap">
            <div>
                <div>Total:</div>
                <div>BYN {{ totalAmountSum }} | USD {{ baseTotalAmountSum }}</div>
            </div>
        </div>

        <div class="grid-wrap">
            <RefuelingsGrid :refuelings="refuelingItems" />
        </div>
    </div>
</template>

<script>
import axios from 'axios'
import RefuelingsGrid from '../../../components/Car/Details/RefuelingsGrid.vue'

export default {
    name: 'RefuelingsList',
    components: {
        RefuelingsGrid
    },
    computed: {
        totalAmountSum() {
            return this.refuelingItems.reduce(
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
            refuelingItems: []
        }
    },
    async created() {
        const result = await axios.get('/api/refuelings/getByCar/' + this.$route.params.id);
        const refuelings = result.data;
        this.refuelingItems = refuelings;
    }
}
</script>
