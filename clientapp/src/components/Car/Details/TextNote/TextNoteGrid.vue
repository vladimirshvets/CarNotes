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
                <th class="text-left">Tag</th>
                <th class="text-left">Text</th>
                <th class="text-left">Comment</th>
            </tr>
        </thead>
        <tbody>
            <text-note-grid-item
                v-for="item in items"
                :key="item.id"
                :item="item"
                class="text-notes-container list-container"
            />
        </tbody>
    </v-table>
</template>

<script>
import TextNoteGridItem from './TextNoteGridItem.vue';
export default {
    name: 'ServiceGrid',
    props: {
        items: Array
    },
    components: {
        TextNoteGridItem
    }
}
</script>
