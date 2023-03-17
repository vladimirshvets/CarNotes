<template>
    <tr class="service-item">
        <td><span>{{ service.mileage.date }}</span></td>
        <td><span>{{ service.mileage.odometerValue }}</span></td>
        <td><span>{{ service.title }}</span></td>
        <td><span>{{ service.stationName }}</span></td>
        <td><span>{{ service.address }}</span></td>
        <td><span>{{ service.websiteUrl }}</span></td>
        <td><span>{{ service.costOfWork }}</span></td>
        <td><span>{{ service.costOfSpareParts }}</span></td>
        <td><span>{{ service.totalAmount }}</span></td>
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
