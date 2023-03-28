<template>
    <div v-if="isLoading"></div>
    <div v-else class="tab-wrap" id="car-washings">
        <div class="summary-wrap">
            <total-costs
                :totalAmount="totalAmountSum"
                :baseTotalAmount="baseTotalAmountSum"
            />
        </div>
        <div class="form-wrap">
            <washing-form
                :showForm="showForm"
                @triggerForm="triggerForm"
                @save="save"
                @update="update"
                @remove="remove"
                :suggestedTitles="titleList"
                :suggestedAddresses="addressList"
            />
        </div>
        <div class="grid-wrap">
            <washing-grid
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
import axios from 'axios';
import { mapGetters, mapMutations } from 'vuex';
import TotalCosts from '@/components/Car/Profile/TotalCosts.vue';
import WashingForm from '@/components/Car/Profile/WashingForm.vue';
import WashingGrid from '@/components/Car/Profile/WashingGrid.vue';

export default {
    name: 'WashingsList',
    components: {
        TotalCosts,
        WashingForm,
        WashingGrid
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
        titleList() {
            return this.items
                .map(r => r.title)
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
        this.$store.dispatch('loadMileages', this.$route.params.carId);
        await this.getItems();
    },
    methods: {
        async getItems() {
            this.setIsLoading(true);
            await axios
                .get(`/api/washings/getByCar/${this.$route.params.carId}`)
                .then((response) => {
                    this.items = response.data;
                })
                .finally(() => {
                    this.setIsLoading(false);
                });
        },
        async save(payload) {
            this.setIsLoading(true);
            await axios
                .post('/api/washings', payload)
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
                .put(`/api/washings/${id}`, payload)
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
                .delete(`/api/washings/${id}`, {
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
