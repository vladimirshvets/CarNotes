<template>
    <div class="tab-wrap" id="car-legal-procedures">
        <div class="summary-wrap">
            <div>
                <div>Total:</div>
                <div>BYN {{ totalAmountSum.toFixed(2) }} | USD {{ baseTotalAmountSum.toFixed(2) }}</div>
            </div>
        </div>

        <div class="grid-wrap">
            <LegalProceduresGrid :legalProcedures="legalProcedureItems" />
        </div>
    </div>
</template>

<script>
import axios from 'axios'
import LegalProceduresGrid from '../../../components/Car/Profile/LegalProceduresGrid.vue'

export default {
    name: 'LegalProceduresList',
    components: {
        LegalProceduresGrid
    },
    computed: {
        totalAmountSum() {
            return this.legalProcedureItems.reduce(
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
            legalProcedureItems: []
        }
    },
    async created() {
        const result = await axios.get(`/api/legalProcedures/getByCar/${this.$route.params.carId}`);
        const legalProcedures = result.data;
        this.legalProcedureItems = legalProcedures;
    }
}
</script>
