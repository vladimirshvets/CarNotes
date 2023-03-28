<template>
    <tr class="washing-item list-item">
        <td><span>{{ washing.mileage.date }}</span></td>
        <td><span>{{ washing.mileage.odometerValue }}</span></td>
        <td><span>{{ washing.title }}</span></td>
        <td><span>{{ washing.address }}</span></td>
        <td><span v-if="washing.isContact"><v-icon>mdi-check</v-icon></span></td>
        <td><span v-if="washing.isDegreaserUsed"><v-icon>mdi-check</v-icon></span></td>
        <td><span v-if="washing.isPolishUsed"><v-icon>mdi-check</v-icon></span></td>
        <td><span v-if="washing.isAntiRainUsed"><v-icon>mdi-check</v-icon></span></td>
        <td><span>{{ washing.totalAmount?.toFixed(2) }}</span></td>
        <td><span class="base-total">{{ washing.baseTotalAmount?.toFixed(2) }}</span></td>
        <td><span>{{ washing.comment }}</span></td>
    </tr>
</template>

<script>
export default {
    name: 'WashingsGridItem',
    props: ['washing']
}
</script>

<style lang="less" scoped>
.list-item {
    &:hover{
        background-color: #e6ff84;
    }

    .base-total {
        color: #016a59;
    }
}
</style>
