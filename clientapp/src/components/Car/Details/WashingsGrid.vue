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
            <WashingsGridItem
                v-for="washing in washings"
                :key="washing.id"
                :washing="washing"
                class="washings-container list-container"
            />
        </tbody>
    </v-table>
</template>

<script>
import WashingsGridItem from './WashingsGridItem.vue'
export default {
    name: 'WashingsGrid',
    props: ['washings'],
    components: {
        WashingsGridItem
    }
}
</script>
