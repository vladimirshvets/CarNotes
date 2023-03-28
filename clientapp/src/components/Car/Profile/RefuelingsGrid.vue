<template>
    <v-table
        fixed-header
        density="compact"
    >
        <thead>
            <tr>
                <th class="text-left">Date</th>
                <th class="text-left">Odometer</th>
                <th class="text-left">Volume, l.</th>
                <th class="text-left">Price per l., BYN</th>
                <th class="text-left">Total, BYN</th>
                <th class="text-left">Total, USD</th>
                <th class="text-left">Distributor</th>
                <th class="text-left">Address</th>
                <th class="text-left">Comment</th>
            </tr>
        </thead>
        <tbody>
            <refuelings-grid-item
                v-for="item in items"
                :key="item.id"
                :item="item"
                class="refuelings-container list-container"
            />
        </tbody>
    </v-table>
</template>

<script>
import RefuelingsGridItem from './RefuelingsGridItem.vue'
export default {
    name: 'RefuelingsGrid',
    props: ['items'],
    components: {
        RefuelingsGridItem
    }
}
</script>
