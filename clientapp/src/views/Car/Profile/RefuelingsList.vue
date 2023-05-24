<template>
    <div v-if="isLoading"></div>
    <div v-else class="tab-wrap" id="car-refuelings">
        <div class="summary-wrap">
            <total-costs
                :totalAmount="totalAmountSum"
                :baseTotalAmount="baseTotalAmountSum"
            />
        </div>
        <div class="form-wrap">
            <refueling-form
                :showForm="showForm"
                @triggerForm="triggerForm"
                @save="save"
                @update="update"
                @remove="remove"
                :suggestedDistributors="distributorList"
                :suggestedAddresses="addressList"
            />
        </div>
        <div class="grid-wrap">
            <refueling-grid
                :items="items"
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
import api from '@/api.js';
import { mapGetters, mapMutations } from 'vuex';
import TotalCosts from '@/components/Car/Profile/TotalCosts.vue';
import RefuelingForm from '@/components/Car/Profile/RefuelingForm.vue';
import RefuelingGrid from '@/components/Car/Profile/RefuelingGrid.vue';
export default {
    name: 'RefuelingsList',
    components: {
        TotalCosts,
        RefuelingForm,
        RefuelingGrid
    },
    computed: {
        totalAmountSum() {
            return this.items.reduce(
                (sum, item) => sum + Number(item.totalAmount), 0
            )
        },
        baseTotalAmountSum() {
            return this.items.reduce(
                (sum, item) => sum + Number(item.baseTotalAmount), 0
            )
        },
        distributorList() {
            return this.items
                .map(r => r.distributor)
                .filter((value, index, self) => value && self.indexOf(value) === index);
        },
        addressList() {
            return this.items
                .map(r => r.address)
                .filter((value, index, self) => value && self.indexOf(value) === index);
        },
        ...mapGetters([
            'isLoading'
        ])
    },
    data() {
        return {
            items: [],
            showForm: false
        }
    },
    async created() {
        await this.actualizeData();
    },
    methods: {
        async actualizeData() {
            this.$store.dispatch('loadMileages', this.$route.params.carId);
            await this.getItems();
        },
        async getItems() {
            this.setIsLoading(true);
            await api
                .get(`/api/refuelings/getByCar/${this.$route.params.carId}`)
                .then((response) => {
                    this.items = response.data;
                })
                .finally(() => {
                    this.setIsLoading(false);
                });
        },
        async save(payload) {
            this.setIsLoading(true);
            await api
                .post('/api/refuelings', payload)
                .then(() => {
                    this.actualizeData();
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
            await api
                .put(`/api/refuelings/${id}`, payload)
                .then(() => {
                    this.actualizeData();
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
            await api
                .delete(`/api/refuelings/${id}`, {
                    data: payload
                })
                .then(() => {
                    this.actualizeData();
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
.summary-wrap {
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
