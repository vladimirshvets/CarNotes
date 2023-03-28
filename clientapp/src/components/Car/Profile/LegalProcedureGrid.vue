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
                <th class="text-left">Group</th>
                <th class="text-left">Total, BYN</th>
                <th class="text-left">Total, USD</th>
                <th class="text-left">Expiration Date</th>
                <th class="text-left">Comment</th>
            </tr>
        </thead>
        <tbody>
            <legal-procedure-grid-item
                v-for="item in items"
                :key="item.id"
                :item="item"
                class="legal-procedures-container list-container"
            />
        </tbody>
    </v-table>
</template>

<script>
import LegalProcedureGridItem from './LegalProcedureGridItem.vue';
export default {
    name: 'LegalProcedureGrid',
    props: {
        items: Array
    },
    components: {
        LegalProcedureGridItem
    }
}
</script>
