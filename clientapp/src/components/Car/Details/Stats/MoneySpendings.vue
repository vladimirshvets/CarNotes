<template>
    <section class="stats-section">
        <div class="section-header">
            <div class="section-title">Spendings</div>
            <div class="section-subtitle quote">"an arm and a leg"</div>
        </div>
        <div class="section-content">
            <div class="spendings-summary">
                <v-row>
                    <v-col cols="12" md="4" sm="4">
                        <div class="circle-wrap">
                            <div class="circle-text">
                                <div class="value">{{ moneyPerKm?.toFixed(2) }}</div>
                                <div class="label">{{ currencyCode }} / km</div>
                            </div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="4" sm="4">
                        <div class="circle-wrap">
                            <div class="circle-text">
                                <div class="value">{{ moneyTotal?.toFixed(2) }}</div>
                                <div class="label">{{ currencyCode }} total</div>
                            </div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="4" sm="4">
                        <div class="circle-wrap">
                            <div class="circle-text">
                                <div class="value">{{ moneyPerMonth?.toFixed(2) }}</div>
                                <div class="label">{{ currencyCode }} / month</div>
                            </div>
                        </div>
                    </v-col>
                </v-row>
            </div>
            <div class="spendings-details">
                <v-table>
                    <tbody>
                        <tr>
                            <td>Refuelings</td>
                            <td>{{ currencyCode }} {{ refuelingsTotal?.toFixed(2) }}</td>
                        </tr>
                        <tr>
                            <td>Washings</td>
                            <td>{{ currencyCode }} {{ washingsTotal?.toFixed(2) }}</td>
                        </tr>
                        <tr>
                            <td>Spare Parts</td>
                            <td>{{ currencyCode }} {{ sparePartsTotal?.toFixed(2) }}</td>
                        </tr>
                        <tr>
                            <td>Services</td>
                            <td>{{ currencyCode }} {{ servicesTotal?.toFixed(2) }}</td>
                        </tr>
                        <tr>
                            <td>Legal Procedures</td>
                            <td>{{ currencyCode }} {{ legalProceduresTotal?.toFixed(2) }}</td>
                        </tr>
                    </tbody>
                </v-table>
            </div>
        </div>
    </section>
</template>

<script>
import api from '@/api.js';
export default {
    name: 'MoneySpendings',
    data() {
        return {
            currencyCode: "BYN",
            moneyTotal: 0,
            moneyPerKm: 0,
            moneyPerMonth: 0,
            legalProceduresTotal: 0,
            refuelingsTotal: 0,
            servicesTotal: 0,
            sparePartsTotal: 0,
            washingsTotal: 0
        }
    },
    created() {
        api
            .get(`/api/personal-stats/money-spendings/${this.$route.params.carId}`)
            .then(response => {
                this.moneyTotal = response.data.moneyTotal;
                this.moneyPerKm = response.data.moneyPerKm;
                this.moneyPerMonth = response.data.moneyPerMonth;
            });
        api
            .get(`/api/personal-stats/money-spendings/details/${this.$route.params.carId}`)
            .then(response => {
                this.legalProceduresTotal = response.data.legalProceduresTotal;
                this.refuelingsTotal = response.data.refuelingsTotal;
                this.servicesTotal = response.data.servicesTotal;
                this.sparePartsTotal = response.data.sparePartsTotal;
                this.washingsTotal = response.data.washingsTotal;
            });
    }
}
</script>

<style lang="less" scoped>
.spendings-details {
    padding-top: 3em;
}
</style>
