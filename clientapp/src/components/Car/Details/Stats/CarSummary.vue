<template>
    <section class="stats-section section-light">
        <div class="section-header">
            <div class="section-title">{{ carInstance.make }} {{ carInstance.model }} {{ carInstance.generation }}</div>
            <div class="section-subtitle">{{ carInstance.year }}</div>
        </div>
        <div class="section-content">
            <v-row>
                <v-col cols="12" md="4">
                    <div v-if="carInstance.vin">* {{ carInstance.vin }} *</div>
                    <v-sheet v-if="carInstance.plate" border rounded class="plate-number">
                        {{ carInstance.plate }}
                    </v-sheet>
                </v-col>
                <v-col cols="12" md="4" class="avatar-wrap">
                    <v-avatar
                        class="avatar"
                        size="240"
                        rounded="1"
                    >
                        <v-img :src="require(`@/assets/car/profile/avatars/0.jpg`)" alt="User Avatar"></v-img>
                    </v-avatar>
                </v-col>
                <v-col cols="12" md="4">
                    <div>{{ carInstance.engineTypeText }}</div>
                </v-col>
            </v-row>
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
.section-content {
    .avatar-wrap {
        display: flex;
        justify-content: center;
    }

    .plate-number {
        display: inline;
        padding: 2px;
        height: 25px;
        width: 80px;
        font-size: 16px;
    }
}
</style>
