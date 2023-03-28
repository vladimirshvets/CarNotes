<template>
    <tr 
        class="washing-item list-item"
        v-on:dblclick="editItem(item)"
    >
        <td><span>{{ item.mileage.date }}</span></td>
        <td><span>{{ item.mileage.odometerValue }}</span></td>
        <td><span>{{ item.title }}</span></td>
        <td><span>{{ item.address }}</span></td>
        <td><span v-if="item.isContact"><v-icon>mdi-check</v-icon></span></td>
        <td><span v-if="item.isDegreaserUsed"><v-icon>mdi-check</v-icon></span></td>
        <td><span v-if="item.isPolishUsed"><v-icon>mdi-check</v-icon></span></td>
        <td><span v-if="item.isAntiRainUsed"><v-icon>mdi-check</v-icon></span></td>
        <td><span>{{ item.totalAmount?.toFixed(2) }}</span></td>
        <td><span class="base-total">{{ item.baseTotalAmount?.toFixed(2) }}</span></td>
        <td><span>{{ item.comment }}</span></td>
    </tr>
</template>

<script>
export default {
    name: 'WashingGridItem',
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

    .base-total {
        color: #016a59;
    }
}
</style>
