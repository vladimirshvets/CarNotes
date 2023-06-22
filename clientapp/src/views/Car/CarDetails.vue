<template>
    <div class="car-tabs-navigation">
        <v-navigation-drawer expand-on-hover rail>
            <v-list>
                <v-list-item
                    :prepend-avatar="require(`@/assets/car/profile/avatars/0.jpg`)"
                    :title="carFullName"
                    :subtitle="carInstance.plate"
                ></v-list-item>
            </v-list>
            <v-divider></v-divider>
            <v-list density="compact" nav>
                <v-list-item prepend-icon="mdi-chart-bar" title="Statistics" value="stats" :to="{ name: 'CarStats' }"></v-list-item>
                <v-list-item prepend-icon="mdi-gas-station" title="Refuelings" value="refuelings" :to="{ name: 'RefuelingsList' }"></v-list-item>
                <v-list-item prepend-icon="mdi-car-wash" title="Washings" value="washings" :to="{ name: 'WashingsList' }"></v-list-item>
                <v-list-item prepend-icon="mdi-car-turbocharger" title="Spare Parts" value="spareparts" :to="{ name: 'SparePartsList' }"></v-list-item>
                <v-list-item prepend-icon="mdi-car-wrench" title="Services" value="services" :to="{ name: 'ServicesList' }"></v-list-item>
                <v-list-item prepend-icon="mdi-file-document-check" title="Legal Procedures" value="legalprocedures" :to="{ name: 'LegalProceduresList' }"></v-list-item>
            </v-list>
        </v-navigation-drawer>
    </div>
    <router-view :carInstance="carInstance"></router-view>
</template>

<script>
import '@/styles/views/car/details/_common.less';
import api from '@/api.js';
export default {
    name: "CarDetails",
    computed: {
        carFullName() {
            return `${this.carInstance.make} ${this.carInstance.model} ${this.carInstance.generation ?? ""}`
        }
    },
    data() {
        return {
            carInstance: {}
        }
    },
    async created() {
        await api
            .get(`/api/cars/${this.$route.params.carId}`)
            .then((response) => {
                this.carInstance = response.data;
            })
            .catch((error) => {
                console.error(error);
            });
    }
}
</script>
