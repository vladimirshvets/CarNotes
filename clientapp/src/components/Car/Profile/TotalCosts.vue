<template>
    <v-card
        color="#385F73"
        theme="dark"
        max-width="300px"
    >
        <v-card-title class="text-h5">
            Total Costs
        </v-card-title>
        <v-card-text>
            <div>BYN {{ totalAmount?.toFixed(2) }}</div>
            <div>USD {{ baseTotalAmount?.toFixed(2) }}</div>
        </v-card-text>
    </v-card>
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
