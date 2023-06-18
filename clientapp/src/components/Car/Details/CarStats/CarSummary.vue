<template>
    <section class="distance-stats stats-section section-light">
        <div class="section-title">{{ carInstance.make }} {{ carInstance.model }} {{ carInstance.generation }}</div>
        <div class="section-subtitle">{{ carInstance.year }}</div>
        <div class="section-content">
            <div>* {{ carInstance.vin }} *</div>
            <v-sheet v-if="carInstance.plate" border rounded class="plate-number">
                {{ carInstance.plate }}
            </v-sheet>
            <div>{{ carInstance.engineTypeText }}</div>
        </div>
    </section>
</template>

<script>
export default {
    name: 'CarSummary',
    props: {
        carInstance: Object
    }
}
</script>

<style lang="less" scoped>
.plate-number {
    display: inline;
    padding: 2px;
    height: 25px;
    width: 80px;
    font-size: 16px;
}
</style>
