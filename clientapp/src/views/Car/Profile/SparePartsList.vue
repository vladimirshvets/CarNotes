<template>
    <div class="tab-wrap" id="car-spare-parts">
        <div class="summary-wrap">
            <div>
                <div>Total:</div>
                <div>BYN {{ totalAmountSum }} | USD {{ baseTotalAmountSum }}</div>
            </div>
        </div>

        <!-- <div class="filter-wrap">
            <v-row>
                <v-col cols="12">
                    <v-combobox
                        v-model="select"
                        :items="filters"
                        label="Show olny"
                        multiple
                    >
                        <template v-slot:selection="data">
                            <v-chip
                                :key="JSON.stringify(data.item)"
                                v-bind="data.attrs"
                                :model-value="data.selected"
                                :disabled="data.disabled"
                                size="small"
                                @click:close="data.parent.selectItem(data.item)"
                            >
                            <template v-slot:prepend>
                                <v-avatar
                                    class="bg-accent text-uppercase"
                                    start
                                >{{ data.item.title.slice(0, 1) }}</v-avatar>
                            </template>
                            {{ data.item.title }}
                            </v-chip>
                        </template>
                    </v-combobox>
                </v-col>
            </v-row>
        </div> -->

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
            // filters: [
            //     {
            //         title: "Maintenance"
            //     },
            //     {
            //         title: "Retrofit"
            //     }
            // ]
        }
    },
    async created() {
        const result = await axios.get('/api/spareParts/getByCar/' + this.$route.params.id);
        const spareParts = result.data;
        this.sparePartItems = spareParts;
    },
    methods: {
        formatPrice(val) {
            return val.ToFixed(2);
        }
    }
}
</script>
