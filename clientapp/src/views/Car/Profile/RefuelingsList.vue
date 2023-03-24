<template>
    <div v-if="isLoading"></div>
    <div v-else class="tab-wrap" id="car-refuelings">
        <v-container class="summary-wrap">
            <v-row>
                <v-col cols="12" sm="3">
                    <div>Total:</div>
                    <div>BYN {{ totalAmountSum.toFixed(2) }} | USD {{ baseTotalAmountSum.toFixed(2) }}</div>
                </v-col>
                <v-col cols="12" sm="1">
                    <v-btn
                        icon="mdi-plus" 
                        size="large"
                        color="primary"
                        @click="triggerRefuelingForm(true)"
                    ></v-btn>
                </v-col>
            </v-row>
        </v-container>
        <div class="form-wrap">
            <RefuelingsForm
                :showForm="showForm"
                @triggerForm="triggerRefuelingForm"
                @save="save"
                @update="update"
                @remove="remove"
                :distributorAutocomplete="distributorList" 
                :addressAutocomplete="addressList"
            />
        </div>
        <div class="grid-wrap">
            <RefuelingsGrid 
                :refuelings="refuelingItems"
                @doubleClickItem="triggerRefuelingForm(true)"
            />
        </div>
    </div>
</template>

<script>
import axios from 'axios'
import { mapGetters, mapMutations } from 'vuex';
import RefuelingsForm from '@/components/Car/Profile/RefuelingsForm.vue'
import RefuelingsGrid from '@/components/Car/Profile/RefuelingsGrid.vue'

export default {
    name: 'RefuelingsList',
    components: {
        RefuelingsForm,
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
        },
        distributorList() {
            return this.refuelingItems
                .map(r => r.distributor)
                .filter((value, index, self) => value && self.indexOf(value) === index);
        },
        addressList() {
            return this.refuelingItems
                .map(r => r.address)
                .filter((value, index, self) => value && self.indexOf(value) === index);
        },
        ...mapGetters([
            'isLoading'
        ])
    },
    data() {
        return {
            refuelingItems: [],
            showForm: false,
        }
    },
    async created() {
        this.getItems();
    },
    methods: {
        async getItems() {
            this.setIsLoading(true);
            const result = await axios
                .get(`/api/refuelings/getByCar/${this.$route.params.id}`)
                .finally(() => {
                    this.setIsLoading(false);
                });
            const refuelings = result.data;
            this.refuelingItems = refuelings;
        },
        async save() {
            //this.setIsLoading(true);
            // await axios
            //     .post('/api/refuelings', payload)
            //     .then(() => {
            //         this.getItems();
            //         this.triggerRefuelingForm(false);
            //         this.showSnackbar("The record has been saved.");
            //     })
            //     .catch(error => {
            //         console.log(error);
            //     })
            //     .finally(() => {
            //         this.setIsLoading(false);
            //     });

            this.triggerRefuelingForm(false);
            this.showSnackbar("The record has been saved.");
        },
        async update() {
            this.triggerRefuelingForm(false);
            this.showSnackbar("The record has been updated.")
        },
        async remove() {
            this.triggerRefuelingForm(false);
            this.showSnackbar("The record has been removed.")
        },
        triggerRefuelingForm(show) {
            this.showForm = show;
            if (show == false) {
                this.setFormData({});
            }
        },
        ...mapMutations([
            'setIsLoading',
            'setSnackbarText',
            'showSnackbar',
            'setFormData',
        ])
    }
}
</script>
