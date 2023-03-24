<template>
    <div class="tab-wrap" id="car-spare-parts">
        <div class="summary-wrap">
            <div>
                <div>Total:</div>
                <div>BYN {{ totalAmountSum.toFixed(2) }} | USD {{ baseTotalAmountSum.toFixed(2) }}</div>
            </div>
        </div>

        <div class="filter-wrap">
            <v-row>
                <v-col cols="12" md="6">
                    <v-select
                        v-model="categoryFilterState"
                        :items="categoryFilterOptions"
                        @update:model-value="applyFilter()"
                        item-title="title"
                        item-value="value"
                        chips
                        label="Show Categories"
                        multiple
                    ></v-select>
                </v-col>
                <v-col cols="12" md="6">
                    <v-select
                        v-model="groupFilterState"
                        :items="groupFilterOptions"
                        @update:model-value="applyFilter()"
                        chips
                        label="Filter by Group"
                        multiple
                    >
                        <template v-slot:selection="{ item, index }">
                            <v-chip v-if="index < 2">
                                <span>{{ item.title }}</span>
                            </v-chip>
                            <span
                                v-if="index === 2"
                                class="text-grey text-caption align-self-center"
                            >
                                (+{{ value.length - 2 }} others)
                            </span>
                        </template>
                    </v-select>
                </v-col>
            </v-row>
        </div>

        <div class="filter-summary-wrap">
            <div>
                <div>Filtered:</div>
                <div>BYN {{ filteredTotalAmountSum.toFixed(2) }} | USD {{ filteredBaseTotalAmountSum.toFixed(2) }}</div>
            </div>
        </div>

        <div class="grid-wrap">
            <SparePartsGrid :spareParts="sparePartFilteredItems" />
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
        },
        filteredTotalAmountSum() {
            return this.sparePartFilteredItems.reduce(
                (sum, item) => sum + Number(item.totalAmount),
                0
            );
        },
        filteredBaseTotalAmountSum() {
            return 0;
        },
        groupFilterOptions() {
            return this.sparePartFilteredItems
                .map(item => item.group)
                .filter((value, index, self) => value && self.indexOf(value) === index);
        }
    },
    data() {
        return {
            sparePartItems: [],
            sparePartFilteredItems: [],
            categoryFilterState: [],
            categoryFilterOptions: [
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
            groupFilterState: [],
        }
    },
    async created() {
        const result = await axios.get(`/api/spareParts/getByCar/${this.$route.params.id}`);
        const spareParts = result.data;
        this.sparePartItems = spareParts;
        this.categoryFilterState = this.categoryFilterOptions.map(option => option.value);
        this.applyFilter();
    },
    methods: {
        applyFilter() {
            const categories = this.categoryFilterState;
            this.sparePartFilteredItems =
                this.sparePartItems.filter(item => categories.includes(item.category));

            const groups = this.groupFilterState;
            if (groups.length > 0) {
                this.sparePartFilteredItems =
                    this.sparePartFilteredItems.filter(item => groups.includes(item.group));
            }
        }
    }
}
</script>

<style lang="less" scoped>
.tab-wrap {
    .summary-wrap, .filter-summary-wrap {
        padding-bottom: 16px;
    }
}
</style>
