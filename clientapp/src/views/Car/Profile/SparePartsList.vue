<template>
    <div class="tab-wrap" id="car-spare-parts">
        <div class="summary-wrap">
            <div>
                <div>Total:</div>
                <div>BYN {{ totalAmountSum }} | USD {{ baseTotalAmountSum }}</div>
            </div>
        </div>

        <div class="filter-wrap">
            <v-select
                v-model="selectedItems"
                :items="filterItems"
                item-title="title"
                item-value="value"
                chips
                label="Show Only"
                multiple
            ></v-select>
        </div>

        <div class="grid-wrap">
            <SparePartsGrid :spareParts="sparePartItems" />
        </div>
    </div>
</template>

<script>
import axios from 'axios'
import SparePartsGrid from '../../../components/Car/Profile/SparePartsGrid.vue'

export default {
    name: 'SparePartsList',
    components: {
        SparePartsGrid
    },
    computed: {
        totalAmountSum() {
            return this.sparePartItems.reduce(
                (sum, item) => sum + Number(item.totalAmount),
                0
            );
        },
        baseTotalAmountSum() {
            return 0;
        }
    },
    data() {
        return {
            sparePartItems: [],
            sparePartFilteredItems: [],
            filterItems: [
                {
                    title: "Maintenance",
                    value: "maintenance"
                },
                {
                    title: "Service",
                    value: "service"
                },
                {
                    title: "Retrofit",
                    value: "retrofit"
                }
            ],
            selectedItems: [],
        }
    },
    async created() {
        const result = await axios.get('/api/spareParts/getByCar/' + this.$route.params.id);
        const spareParts = result.data;

        this.selectedItems = this.filterItems.slice();
        this.sparePartItems = spareParts;
    },
    methods: {
        formatPrice(val) {
            return val.ToFixed(2);
        }
    }
}
</script>
