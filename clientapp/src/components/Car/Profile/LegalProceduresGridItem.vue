<template>
    <tr class="legal-procedure-item list-item">
        <td><span>{{ legalProcedure.mileage.date }}</span></td>
        <td><span>{{ legalProcedure.mileage.odometerValue }}</span></td>
        <td><span>{{ legalProcedure.title }}</span></td>
        <td><span>{{ legalProcedure.group }}</span></td>
        <td><span>{{ legalProcedure.totalAmount.toFixed(2) }}</span></td>
        <td><span></span></td>
        <td><span>{{ legalProcedure.expirationDate }}</span></td>
        <td><span>{{ legalProcedure.comment }}</span></td>
    </tr>
</template>

<script>
export default {
    name: 'LegalProceduresGridItem',
    props: ['legalProcedure']
}
</script>

<style lang="less" scoped>
.list-item:hover{
    background-color: #e6ff84;
}
</style>
