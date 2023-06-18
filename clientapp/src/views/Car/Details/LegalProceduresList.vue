<template>
    <section class="legal-procedures-list stats-section section-light">
        <div class="section-title">Legal Procedures</div>
        <div class="section-content">
            <total-costs
                :totalAmount="totalAmountSum"
                :baseTotalAmount="baseTotalAmountSum"
            />
            <legal-procedure-grid
                :items="items"
                @editItem="triggerForm(true)"
            />
            <legal-procedure-form
                :showForm="showForm"
                @triggerForm="triggerForm"
                @save="save"
                @update="update"
                @remove="remove"
                :suggestedTitles="titleList"
                :suggestedGroups="groupList"
            />
            <v-btn
                class="button-add"
                icon="mdi-plus"
                size="large"
                @click="triggerForm(true)"
            ></v-btn>
        </div>
    </section>
</template>

<script>
import api from '@/api.js';
import { mapGetters, mapMutations } from 'vuex';
import TotalCosts from '@/components/Car/Details/Common/TotalCosts.vue';
import LegalProcedureForm from '@/components/Car/Details/LegalProcedureForm.vue';
import LegalProcedureGrid from '@/components/Car/Details/LegalProcedureGrid.vue';

export default {
    name: 'LegalProceduresList',
    components: {
        TotalCosts,
        LegalProcedureForm,
        LegalProcedureGrid
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
                .get(`/api/legalProcedures/getByCar/${this.$route.params.carId}`)
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
                .post('/api/legalProcedures', payload)
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
                .put(`/api/legalProcedures/${id}`, payload)
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
                .delete(`/api/legalProcedures/${id}`, {
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
.button-add {
    background-color: #016a59;
    color: #fff;
    position: fixed;
    right: 50px;
    bottom: 50px;
    z-index: 1000;
    transition: transform 0.3s;

    &:hover {
        transform: rotate(90deg) scale(1.1);
    }
}
</style>
