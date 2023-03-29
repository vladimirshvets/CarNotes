<template>
    <div v-if="isLoading"></div>
    <div v-else class="tab-wrap" id="car-spare-parts">
        <div class="summary-wrap">
            <total-costs
                :totalAmount="totalAmountSum"
                :baseTotalAmount="baseTotalAmountSum"
            />
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
            <total-costs
                :totalAmount="filteredTotalAmountSum"
                :baseTotalAmount="filteredBaseTotalAmountSum"
            />
        </div>
        <div class="form-wrap">
            <!-- ToDo: pass autofill data -->
            <spare-part-form
                :showForm="showForm"
                @triggerForm="triggerForm"
                @save="save"
                @update="update"
                @remove="remove"
                :suggestedCategories="categoryFilterOptions"
                :suggestedGroups="groupList"
            />
        </div>
        <div class="grid-wrap">
            <spare-part-grid
                :items="filteredItems"
                @editItem="triggerForm(true)"
            />
        </div>
        <div class="actions-wrap">
            <v-btn
                class="button-add"
                icon="mdi-plus"
                size="large"
                color="primary"
                @click="triggerForm(true)"
            ></v-btn>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { mapGetters, mapMutations } from 'vuex';
import TotalCosts from '@/components/Car/Profile/TotalCosts.vue';
import SparePartForm from '@/components/Car/Profile/SparePartForm.vue';
import SparePartGrid from '@/components/Car/Profile/SparePartGrid.vue';
export default {
    name: 'SparePartsList',
    components: {
        TotalCosts,
        SparePartForm,
        SparePartGrid
    },
    computed: {
        totalAmountSum() {
            return this.items.reduce(
                (sum, item) => sum + Number(item.totalAmount), 0
            );
        },
        baseTotalAmountSum() {
            return this.items.reduce(
                (sum, item) => sum + Number(item.baseTotalAmount), 0
            );
        },
        filteredTotalAmountSum() {
            return this.filteredItems.reduce(
                (sum, item) => sum + Number(item.totalAmount), 0
            );
        },
        filteredBaseTotalAmountSum() {
            return this.filteredItems.reduce(
                (sum, item) => sum + Number(item.baseTotalAmount), 0
            );
        },
        groupFilterOptions() {
            return this.items
                .map(item => item.group)
                .filter((value, index, self) => value && self.indexOf(value) === index);
        },
        // ToDo: prepare autofill data
        groupList() {
            return this.items
                .map(r => r.group)
                .filter((value, index, self) => value && self.indexOf(value) === index);
        },
        ...mapGetters([
            'isLoading'
        ])
    },
    data() {
        return {
            items: [],
            filteredItems: [],
            showForm: false,
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
            groupFilterState: []
        }
    },
    async created() {
        this.$store.dispatch('loadMileages', this.$route.params.carId);
        await this.getItems();
    },
    methods: {
        async getItems() {
            this.setIsLoading(true);
            await axios
                .get(`/api/spareParts/getByCar/${this.$route.params.carId}`)
                .then((response) => {
                    this.items = response.data;
                })
                .finally(() => {
                    this.setIsLoading(false);
                });

            // ToDo: should it be called every time after getItems()?
            this.categoryFilterState = this.categoryFilterOptions.map(option => option.value);
            this.applyFilter();
        },
        applyFilter() {
            const categories = this.categoryFilterState;
            this.filteredItems =
                this.items.filter(item => categories.includes(item.category));

            const groups = this.groupFilterState;
            if (groups.length > 0) {
                this.filteredItems =
                    this.filteredItems.filter(item => groups.includes(item.group));
            }
        },
        async save(payload) {
            this.setIsLoading(true);
            await axios
                .post('/api/spareParts', payload)
                .then(() => {
                    this.getItems();
                    this.triggerForm(false);
                    this.snackbar("The record has been saved.");
                })
                .catch(error => {
                    console.log(error);
                })
                .finally(() => {
                    this.setIsLoading(false);
                });
        },
        async update(id, payload) {
            this.setIsLoading(true);
            await axios
                .put(`/api/spareParts/${id}`, payload)
                .then(() => {
                    this.getItems();
                    this.triggerForm(false);
                    this.snackbar("The record has been updated.")
                })
                .catch(error => {
                    console.log(error);
                })
                .finally(() => {
                    this.setIsLoading(false);
                });
        },
        async remove(id, payload) {
            this.setIsLoading(true);
            await axios
                .delete(`/api/spareParts/${id}`, {
                    data: payload
                })
                .then(() => {
                    this.getItems();
                    this.triggerForm(false);
                    this.snackbar("The record has been removed.")
                })
                .catch(error => {
                    console.log(error.response.data);
                })
                .finally(() => {
                    this.setIsLoading(false);
                });
        },
        triggerForm(state) {
            this.showForm = state;
            if (!state) {
                this.setFormData({});
            }
        },
        ...mapMutations([
            'setIsLoading',
            'snackbar',
            'setFormData',
        ])
    }
}
</script>

<style lang="less" scoped>
.summary-wrap, .filter-summary-wrap {
    padding-bottom: 2em;
}

.actions-wrap {
    .button-add {
        position: fixed;
        right: 50px;
        bottom: 50px;
        z-index: 1000;
        transition: transform 0.3s;

        &:hover {
            transform: rotate(90deg) scale(1.1);
        }
    }
}
</style>
