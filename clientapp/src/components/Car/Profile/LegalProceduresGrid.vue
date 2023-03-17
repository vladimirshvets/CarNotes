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
            <LegalProceduresGridItem
                v-for="legalProcedure in legalProcedures"
                :key="legalProcedure.id"
                :legalProcedure="legalProcedure"
                class="legal-procedures-container list-container"
            />
        </tbody>
    </v-table>
</template>

<script>
import LegalProceduresGridItem from './LegalProceduresGridItem.vue'
export default {
    name: 'LegalProceduresGrid',
    props: ['legalProcedures'],
    components: {
        LegalProceduresGridItem
    }
}
</script>
