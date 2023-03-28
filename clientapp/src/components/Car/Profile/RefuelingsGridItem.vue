<template>
    <tr
        class="refueling-item list-item"
        v-on:dblclick="editItem(item)"
    >
        <td><span>{{ item.mileage.date }}</span></td>
        <td><span>{{ item.mileage.odometerValue }}</span></td>
        <td><span>{{ item.volume?.toFixed(1) }}</span></td>
        <td><span>{{ item.price?.toFixed(2) }}</span></td>
        <td><span>{{ item.totalAmount?.toFixed(2) }}</span></td>
        <td><span class="base-total">{{ item.baseTotalAmount?.toFixed(2) }}</span></td>
        <td><span>{{ item.distributor }}</span></td>
        <td><span>{{ item.address }}</span></td>
        <td><span>{{ item.comment }}</span></td>
    </tr>
</template>

<script>
export default {
    name: 'RefuelingsGridItem',
    props: ['item'],
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

    .base-total {
        color: #016a59;
    }
}
</style>
