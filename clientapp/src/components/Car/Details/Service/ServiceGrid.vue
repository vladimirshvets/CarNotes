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
                <th class="text-left">Station</th>
                <th class="text-left">Address</th>
                <th class="text-left">Url</th>
                <th class="text-left">Cost of work, BYN</th>
                <th class="text-left">Cost of parts, BYN</th>
                <th class="text-left">Total, BYN</th>
                <th class="text-left">Total, USD</th>
                <th class="text-left">Comment</th>
            </tr>
        </thead>
        <tbody>
            <service-grid-item
                v-for="item in items"
                :key="item.id"
                :item="item"
                class="services-container list-container"
            />
        </tbody>
    </v-table>
</template>

<script>
import ServiceGridItem from './ServiceGridItem.vue';
export default {
    name: 'ServiceGrid',
    props: {
        items: Array
    },
    components: {
        ServiceGridItem
    }
}
</script>
