<template>
    <v-table
        fixed-header
        density="compact"
    >
        <thead>
            <tr>
                <th class="text-left">Order Date</th>
                <th class="text-left">Purchase Date</th>
                <th class="text-left">Installation Date</th>
                <th class="text-left">Installation Odometer</th>
                <th class="text-left">Removal Date</th>
                <th class="text-left">Removal Odometer</th>
                <th class="text-left">Group</th>
                <th class="text-left">Name</th>
                <th class="text-left">UoM</th>
                <th class="text-left">OE</th>
                <th class="text-left">OE Number</th>
                <th class="text-left">Replacement Number</th>
                <th class="text-left">Manufacturer</th>
                <th class="text-left">Country of Origin</th>
                <th class="text-left">Price, BYN</th>
                <th class="text-left">Qty</th>
                <th class="text-left">Total, BYN</th>
                <th class="text-left">Total, USD</th>
                <th class="text-left">Shop URL</th>
                <th class="text-left">Shop Address</th>
                <th class="text-left">Production Date</th>
                <th class="text-left">Expiration Date</th>
                <th class="text-left">Comment</th>
            </tr>
        </thead>
        <tbody>
            <SparePartsGridItem
                v-for="sparePart in spareParts"
                :key="sparePart.id"
                :sparePart="sparePart"
                class="spare-parts-container list-container"
            />
        </tbody>
    </v-table>
</template>

<script>
import SparePartsGridItem from './SparePartsGridItem.vue'
export default {
    name: 'SparePartsGrid',
    props: ['spareParts'],
    components: {
        SparePartsGridItem
    }
}
</script>
