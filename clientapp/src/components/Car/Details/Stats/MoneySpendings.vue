<template>
    <section class="stats-section section-light">
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
                                <div class="label">BYN / km</div>
                            </div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="4" sm="4">
                        <div class="circle-wrap">
                            <div class="circle-text">
                                <div class="value">{{ moneyTotal?.toFixed(2) }}</div>
                                <div class="label">BYN total</div>
                            </div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="4" sm="4">
                        <div class="circle-wrap">
                            <div class="circle-text">
                                <div class="value">{{ moneyPerMonth?.toFixed(2) }}</div>
                                <div class="label">BYN / month</div>
                            </div>
                        </div>
                    </v-col>
                </v-row>
            </div>
            <div class="spendings-details">
                <v-row>
                    <v-col cols="12" md="2" sm="2">
                        <div class="spendings-item">
                            <div class="value">$---</div>
                            <div class="label">Refuelings</div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="2" sm="2">
                        <div class="spendings-item">
                            <div class="value">$---</div>
                            <div class="label">Washings</div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="2" sm="2">
                        <div class="spendings-item">
                            <div class="value">$---</div>
                            <div class="label">Spare Parts (Maintenance)</div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="2" sm="2">
                        <div class="spendings-item">
                            <div class="value">$---</div>
                            <div class="label">Spare Parts (Service)</div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="2" sm="2">
                        <div class="spendings-item">
                            <div class="value">$---</div>
                            <div class="label">Services</div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="2" sm="2">
                        <div class="spendings-item">
                            <div class="value">$---</div>
                            <div class="label">Legal Procedures</div>
                        </div>
                    </v-col>
                </v-row>
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
            moneyTotal: 0,
            moneyPerKm: 0,
            moneyPerMonth: 0
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
    }
}
</script>

<style lang="less" scoped>
.spendings-details {
    padding-top: 3em;

    .spendings-item {
        text-align: center;

        .value {
            font-size: 24px;
        }
    }
}

</style>