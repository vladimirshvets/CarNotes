<template>
    <tr class="spare-part-item list-item">
        <td><span>{{ sparePart.orderDate }}</span></td>
        <td><span>{{ sparePart.purchaseDate }}</span></td>
        <td><span>{{ sparePart.installationMileage.date }}</span></td>
        <td><span>{{ sparePart.installationMileage.odometerValue }}</span></td>
        <td><span></span></td>
        <td><span></span></td>
        <td><span>{{ sparePart.group }}</span></td>
        <td><span>{{ sparePart.name }}</span></td>
        <td><span>{{ sparePart.uoM }}</span></td>
        <td><span v-if="sparePart.isOE">+</span></td>
        <td><span>{{ sparePart.oeNumber }}</span></td>
        <td><span>{{ sparePart.replacementNumber }}</span></td>
        <td><span>{{ sparePart.manufacturer }}</span></td>
        <td><span>{{ sparePart.countryOfOrigin }}</span></td>
        <td><span>{{ sparePart.price.toFixed(2) }}</span></td>
        <td><span>{{ sparePart.qty }}</span></td>
        <td><span>{{ sparePart.totalAmount.toFixed(2) }}</span></td>
        <td><span></span></td>
        <td><span>{{ sparePart.shopWebsiteUrl }}</span></td>
        <td><span>{{ sparePart.shopAddress }}</span></td>
        <td><span>{{ sparePart.productionDate }}</span></td>
        <td><span>{{ sparePart.expirationDate }}</span></td>
        <td><span>{{ sparePart.comment }}</span></td>
    </tr>
</template>

<script>
export default {
    name: 'SparePartsGridItem',
    props: ['sparePart']
}
</script>

<style lang="less" scoped>
.list-item:hover{
    background-color: #e6ff84;
}
</style>
