<template>
    <tr
        class="text-note-item list-item"
        v-on:dblclick="editItem(item)"
    >
        <td><span>{{ item.mileage.date }}</span></td>
        <td><span>{{ item.mileage.odometerValue }}</span></td>
        <td><span>{{ item.title }}</span></td>
        <td><span>{{ item.tag }}</span></td>
        <td><span>{{ item.text }}</span></td>
        <td><span>{{ item.comment }}</span></td>
    </tr>
</template>

<script>
export default {
    name: 'TextNoteGridItem',
    props: {
        item: Object
    },
    methods: {
        editItem(item) {
            let itemCopy = { ...item };
            this.$store.commit('setFormData', itemCopy);
            this.$parent.$emit('editItem');
        }
    }
}
</script>

<style lang="less" scoped>
.list-item {
    &:hover{
        background-color: #e6ff84;
    }
}
</style>
