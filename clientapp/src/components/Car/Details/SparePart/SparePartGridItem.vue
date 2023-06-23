<template>
    <tr
        class="spare-part-item list-item"
        v-on:dblclick="editItem(item)"
    >
        <td><span>{{ item.orderDate }}</span></td>
        <td><span>{{ item.purchaseDate }}</span></td>
        <td><span>{{ item.installationMileage.date }}</span></td>
        <td><span>{{ item.installationMileage.odometerValue }}</span></td>
        <td><span>{{ item.removalMileage?.date }}</span></td>
        <td><span>{{ item.removalMileage?.odometerValue }}</span></td>
        <td><span>{{ item.group }}</span></td>
        <td><span>{{ item.name }}</span></td>
        <td><span>{{ item.uoM }}</span></td>
        <td><span v-if="item.isOE"><v-icon>mdi-check</v-icon></span></td>
        <td><span>{{ item.oeNumber }}</span></td>
        <td><span>{{ item.replacementNumber }}</span></td>
        <td><span>{{ item.manufacturer }}</span></td>
        <td><span>{{ item.countryOfOrigin }}</span></td>
        <td><span>{{ item.price?.toFixed(2) }}</span></td>
        <td><span>{{ item.qty }}</span></td>
        <td><span>{{ item.totalAmount?.toFixed(2) }}</span></td>
        <td><span class="base-total">{{ item.baseTotalAmount?.toFixed(2) }}</span></td>
        <td><span>{{ item.shopWebsiteUrl }}</span></td>
        <td><span>{{ item.shopAddress }}</span></td>
        <td><span>{{ item.productionDate }}</span></td>
        <td><span>{{ item.expirationDate }}</span></td>
        <td><span>{{ item.comment }}</span></td>
    </tr>
</template>

<script>
export default {
    name: 'SparePartGridItem',
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
