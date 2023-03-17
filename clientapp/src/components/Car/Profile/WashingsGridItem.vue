<template>
    <tr class="washing-item">
        <td><span>{{ washing.mileage.date }}</span></td>
        <td><span>{{ washing.mileage.odometerValue }}</span></td>
        <td><span>{{ washing.title }}</span></td>
        <td><span>{{ washing.address }}</span></td>
        <td><span v-if="washing.isContact">+</span></td>
        <td><span v-if="washing.isDegreaserUsed">+</span></td>
        <td><span v-if="washing.isPolishUsed">+</span></td>
        <td><span v-if="washing.isAntiRainUsed">+</span></td>
        <td><span>{{ washing.totalAmount }}</span></td>
        <td><span></span></td>
        <td><span>{{ washing.comment }}</span></td>
    </tr>
</template>

<script>
export default {
    name: 'WashingsGridItem',
    props: ['washing']
}
</script>
