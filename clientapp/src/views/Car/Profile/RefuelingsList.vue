<template>
    <div class="tab-wrap" id="car-refuelings">
        <v-container class="summary-wrap">
            <v-row>
                <v-col cols="12" sm="6">
                    <div>Total:</div>
                    <div>BYN {{ totalAmountSum.toFixed(2) }} | USD {{ baseTotalAmountSum.toFixed(2) }}</div>
                </v-col>
                <v-col cols="12" sm="6">
                    <v-btn
                        @click="triggerRefuelingForm(true)"
                        color="primary"
                    >
                        Add Refueling
                    </v-btn>
                </v-col>
            </v-row>

        </v-container>
        <div class="form-wrap">
            <RefuelingsForm
                :showForm="showForm"
                @close="triggerRefuelingForm"
                @submit="save"
                :distributorAutocomplete="distributorList" 
                :addressAutocomplete="addressList"
            />
        </div>
        <div class="grid-wrap">
            <RefuelingsGrid :refuelings="refuelingItems" />
        </div>
    </div>

    <!-- ToDo: create reusable component -->
    <v-snackbar
        v-model="snackbar"
        :timeout="2000"
        color="#016a59"
        rounded="pill"
    >
        Refueling info has been saved.
    </v-snackbar>
</template>

<script>
import axios from 'axios'
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
        }
    },
    data() {
        return {
            refuelingItems: [],
            showForm: false,
            snackbar: false
        }
    },
    async created() {
        this.getItems();
    },
    methods: {
        async getItems() {
            const result = await axios.get('/api/refuelings/getByCar/' + this.$route.params.id);
            const refuelings = result.data;
            this.refuelingItems = refuelings;
        },
        async save(refuelingData) {
            await axios
                .post('/api/refuelings', refuelingData)
                .then(response => {
                    // ToDo:
                    // Option 1. Update and sort array
                    const item = response.data;
                    this.refuelingItems.push(item);

                    // or Option 2. Reload array
                    this.getItems();
                    this.triggerRefuelingForm(false);
                    this.snackbar = true;
                })
                .catch(error => {
                    console.log(error);
                })
                .finally(() => {
                });
        },
        triggerRefuelingForm(value) {
            this.showForm = value;
        }
    }
}
</script>
