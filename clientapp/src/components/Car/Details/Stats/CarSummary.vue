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
                        v-if="carInstance.avatarUrl"
                        class="avatar"
                        size="240"
                        rounded="1"
                    >
                        <v-img
                            :src="`/static/images/${carInstance.avatarUrl}`"
                            cover
                            alt="Car Avatar"
                        ></v-img>
                    </v-avatar>
                    <v-avatar
                        v-else
                        class="avatar default"
                        size="240"
                        rounded="1"
                    >
                        <v-img
                            :src="require(`@/assets/car/avatars/logo_480.jpg`)"
                            alt="Car Avatar"
                        ></v-img>
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

        .avatar {
            &.default {
                border: solid 1px;
            }
        }
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
