<template>
    <v-table
        fixed-header
        density="compact"
    >
        <thead>
            <tr>
                <th class="text-left">Date</th>
                <th class="text-left">Odometer</th>
                <th class="text-left">Title</th>
                <th class="text-left">Address</th>
                <th class="text-left">Contact</th>
                <th class="text-left">Degreaser</th>
                <th class="text-left">Polish</th>
                <th class="text-left">Anti-rain</th>
                <th class="text-left">Total, BYN</th>
                <th class="text-left">Total, USD</th>
                <th class="text-left">Comment</th>
            </tr>
        </thead>
        <tbody>
            <washing-grid-item
                v-for="item in items"
                :key="item.id"
                :item="item"
                class="washings-container list-container"
            />
        </tbody>
    </v-table>
</template>

<script>
import WashingGridItem from './WashingGridItem.vue';
export default {
    name: 'WashingGrid',
    props: {
        items: Array
    },
    components: {
        WashingGridItem
    }
}
</script>
