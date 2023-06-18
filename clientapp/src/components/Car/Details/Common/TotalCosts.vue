<template>
    <div class="total-costs-wrap">
        <v-row>
            <v-col cols="12" md="6" sm="6">
                <div class="circle-wrap">
                    <div class="circle-text">
                        <div class="value">{{ totalAmount?.toFixed(2) }}</div>
                        <div class="label">BYN</div>
                    </div>
                </div>
            </v-col>
            <v-col cols="12" md="6" sm="6">
                <div class="circle-wrap">
                    <div class="circle-text">
                        <div class="value">{{ baseTotalAmount?.toFixed(2) }}</div>
                        <div class="label">USD</div>
                    </div>
                </div>
            </v-col>
        </v-row>
    </div>
</template>

<script>
export default {
    name: 'TotalCosts',
    props: {
        totalAmount: Number,
        baseTotalAmount: Number
    }
}
</script>

<style scoped>
.total-costs-wrap {
    padding-bottom: 2em;
}
</style>
