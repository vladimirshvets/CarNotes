<template>
    <v-container>
        <h1 v-if="!carId">Add Car</h1>
        <h1 v-else>Edit Car</h1>

        <v-form @submit.prevent="onSubmit">
            <v-row>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInfo.make"
                        label="Make"
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInfo.model"
                        label="Model"
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInfo.generation"
                        label="Generation"
                    ></v-text-field>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInfo.year"
                        label="Year"
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInfo.ownedFrom"
                        label="Owned From"
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInfo.ownedTo"
                        label="Owned To"
                    ></v-text-field>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInfo.vin"
                        label="VIN"
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInfo.plate"
                        label="Plate Number"
                    ></v-text-field>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="3">
                    <v-select
                        v-model="carInfo.engineType"
                        label="Engine Type"
                        :items="engineTypes"
                    ></v-select>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInfo.engineDisplacement"
                        label="Engine Displacement"
                        disabled
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInfo.enginePower"
                        label="Engine Power"
                        disabled
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInfo.transmissionType"
                        label="Transmission Type"
                        disabled
                    ></v-text-field>
                </v-col>
            </v-row>
            <v-row class="actions">
                <v-col cols="auto">
                    <v-btn
                        type="submit"
                    >
                        Save
                    </v-btn>
                </v-col>
                <v-col cols="auto">
                    <v-btn
                        v-if="carId"
                        color="warning"
                        @click="removalModal = true"
                    >
                        Delete
                    </v-btn>
                </v-col>
            </v-row>
        </v-form>
        <delete-confirmation-modal
            :showModal="removalModal"
            @triggerModal="triggerRemovalModal"
            @submit="onRemove"
            title="Delete Car"
            text="Are you sure you want to delete this car with all related notes?<br/><br/>This action can be undone."
        ></delete-confirmation-modal>
    </v-container>
</template>

<script>
import api from '@/api.js';
import DeleteConfirmationModal from '@/components/DeleteConfirmationModal.vue';
export default {
    name: 'CarProfile',
    components: {
        DeleteConfirmationModal
    },
    data() {
        return {
            // ToDo: load options from server?
            engineTypes: [
                { title: "Diesel", value: 0 },
                { title: "Petrol", value: 1 },
                { title: "Methane", value: 2 },
                { title: "Electric", value: 3 },
            ],
            carId: this.$route.params.carId,
            carInfo: {},
            removalModal: false
        }
    },
    async created() {
        if (this.carId) {
            await api
                .get(`/api/cars/${this.carId}`)
                .then((response) => {
                    this.carInfo = response.data;
                })
                .catch((error) => {
                    console.error(error);
                });
        }
    },
    methods: {
        async onSubmit() {
            if (!this.carId) {
                await api
                    .post(`/api/cars`, this.carInfo)
                    .then(() => {
                        this.$router.push({ name: 'Cars' });
                    })
                    .catch(error => {
                        console.error(error);
                    })
            } else {
                await api
                    .put(`/api/cars/${this.carId}`, this.carInfo)
                    .then(() => {
                        this.$router.push({ name: 'Cars' });
                    })
                    .catch(error => {
                        console.error(error);
                    })
            }
        },
        async onRemove() {
            await api
                .delete(`/api/cars/${this.carId}`)
                .then(() => {
                    this.$router.push({ name: 'Cars' });
                })
                .catch(error => {
                    console.error(error);
                });
        },
        triggerRemovalModal(state) {
            this.removalModal = state;
        }
    }
}
</script>

<style lang="less" scoped>
</style>
