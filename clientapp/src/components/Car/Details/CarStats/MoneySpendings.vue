<template>
    <section class="distance-stats stats-section section-light">
        <div class="section-header">
            <div class="section-title">Spendings</div>
            <div class="section-subtitle quote">"an arm and a leg"</div>
        </div>
        <div class="section-content">
            <div class="spendings-summary">
                <v-row>
                    <v-col cols="12" md="6" sm="6">
                        <div class="circle-wrap">
                            <div class="circle-text">
                                <div class="value">0.32</div>
                                <div class="label">$ / km</div>
                            </div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="6" sm="6">
                        <div class="circle-wrap">
                            <div class="circle-text">
                                <div class="value">1.27</div>
                                <div class="label">BYN / km</div>
                            </div>
                        </div>
                    </v-col>
                </v-row>
            </div>
            <div class="spendings-details">
                <v-row>
                    <v-col cols="12" md="2" sm="2">
                        <div class="spendings-item">
                            <div class="value">$123</div>
                            <div class="label">Refuelings</div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="2" sm="2">
                        <div class="spendings-item">
                            <div class="value">$234</div>
                            <div class="label">Washings</div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="2" sm="2">
                        <div class="spendings-item">
                            <div class="value">$345</div>
                            <div class="label">Spare Parts (Maintenance)</div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="2" sm="2">
                        <div class="spendings-item">
                            <div class="value">$456</div>
                            <div class="label">Spare Parts (Service)</div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="2" sm="2">
                        <div class="spendings-item">
                            <div class="value">$567</div>
                            <div class="label">Services</div>
                        </div>
                    </v-col>
                    <v-col cols="12" md="2" sm="2">
                        <div class="spendings-item">
                            <div class="value">$678</div>
                            <div class="label">Legal Procedures</div>
                        </div>
                    </v-col>
                </v-row>
            </div>
        </div>
    </section>
</template>

<script>
export default {
    name: 'MoneySpendings'
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