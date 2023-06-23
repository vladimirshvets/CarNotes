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
            <refueling-grid-item
                v-for="item in items"
                :key="item.id"
                :item="item"
                class="refuelings-container list-container"
            />
        </tbody>
    </v-table>
</template>

<script>
import RefuelingGridItem from './RefuelingGridItem.vue';
export default {
    name: 'RefuelingGrid',
    props: {
        items: Array
    },
    components: {
        RefuelingGridItem
    }
}
</script>
