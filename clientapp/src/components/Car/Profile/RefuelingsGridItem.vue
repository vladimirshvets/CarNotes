<template>
    <tr class="refueling-item list-item">
        <td><span>{{ refueling.mileage.date }}</span></td>
        <td><span>{{ refueling.mileage.odometerValue }}</span></td>
        <td><span>{{ refueling.volume.toFixed(1) }}</span></td>
        <td><span>{{ refueling.price.toFixed(2) }}</span></td>
        <td><span></span>{{ refueling.totalAmount.toFixed(2) }}</td>
        <td><span></span></td>
        <td><span>{{ refueling.distributor }}</span></td>
        <td><span>{{ refueling.address }}</span></td>
        <td><span>{{ refueling.comment }}</span></td>
    </tr>
</template>

<script>
export default {
    name: 'RefuelingsGridItem',
    props: ['refueling']
}
</script>

<style lang="less" scoped>
.list-item:hover{
    background-color: #e6ff84;
}
</style>
