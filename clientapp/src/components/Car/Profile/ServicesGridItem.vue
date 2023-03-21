<template>
    <tr class="service-item list-item">
        <td><span>{{ service.mileage.date }}</span></td>
        <td><span>{{ service.mileage.odometerValue }}</span></td>
        <td><span>{{ service.title }}</span></td>
        <td><span>{{ service.stationName }}</span></td>
        <td><span>{{ service.address }}</span></td>
        <td><span>{{ service.websiteUrl }}</span></td>
        <td><span>{{ service.costOfWork.toFixed(2) }}</span></td>
        <td><span>{{ service.costOfSpareParts.toFixed(2) }}</span></td>
        <td><span>{{ service.totalAmount.toFixed(2) }}</span></td>
        <td><span></span></td>
        <td><span>{{ service.comment }}</span></td>
    </tr>
</template>

<script>
export default {
    name: 'ServicesGridItem',
    props: ['service']
}
</script>

<style lang="less" scoped>
.list-item:hover{
    background-color: #e6ff84;
}
</style>
