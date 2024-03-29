<template>
    <v-container>
        <h1 v-if="!carId">Add Car</h1>
        <h1 v-else>Edit Car</h1>

        <v-form @submit.prevent="onSubmit">
            <v-row>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInstance.make"
                        label="Make"
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInstance.model"
                        label="Model"
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInstance.generation"
                        label="Generation"
                    ></v-text-field>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInstance.year"
                        label="Year"
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInstance.ownedFrom"
                        label="Owned From"
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInstance.ownedTo"
                        label="Owned To"
                    ></v-text-field>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInstance.vin"
                        label="VIN"
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInstance.plate"
                        label="Plate Number"
                    ></v-text-field>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="3">
                    <v-select
                        v-model="carInstance.engineType"
                        label="Engine Type"
                        :items="engineTypes"
                    ></v-select>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInstance.engineDisplacement"
                        label="Engine Displacement"
                        disabled
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInstance.enginePower"
                        label="Engine Power"
                        disabled
                    ></v-text-field>
                </v-col>
                <v-col cols="3">
                    <v-text-field
                        v-model="carInstance.transmissionType"
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

        <br>
        <v-card v-if="carId" class="pa-md-4 pa-sm-4" title="Update Avatar">
            <v-form class="form" @submit.prevent="onFileFormSubmit">
                <v-file-input
                    v-model="selectedFile"
                    label="Avatar"
                    prepend-icon="mdi-camera"
                    clearable
                    show-size
                    :rules="rules"
                    accept="image/jpg"
                    @change="handleFileUpload"
                ></v-file-input>
                <v-btn type="submit">Update</v-btn>
            </v-form>
        </v-card>
    </v-container>
</template>

<script>
import api from '@/api.js';
import DeleteConfirmationModal from '@/components/Common/DeleteConfirmationModal.vue';
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
            carInstance: {},
            removalModal: false,

            selectedFile: null,
            rules: [
                value => {
                    return !value || !value.length || value[0].size < 10485760 || 'Avatar size should be less than 10 MB.'
                }
            ]
        }
    },
    async created() {
        if (this.carId) {
            await api
                .get(`/api/cars/${this.carId}`)
                .then((response) => {
                    this.carInstance = response.data;
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
                    .post(`/api/cars`, this.carInstance)
                    .then(() => {
                        this.$router.push({ name: 'Cars' });
                    })
                    .catch(error => {
                        console.error(error);
                    })
            } else {
                await api
                    .put(`/api/cars/${this.carId}`, this.carInstance)
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
        },
        handleFileUpload(event) {
            const file = event.target.files[0];
            this.formData = new FormData();
            this.formData.append('file', file);
        },
        onFileFormSubmit() {
            api
                .post(`/api/images/avatar/${this.carInstance.id}`, this.formData, {
                    headers: {
                        "Content-Type": "multipart/form-data",
                    }
                })
                .then((response) => {
                    // ToDo:
                    // Update carInstance object
                    // this.carInstance.avatarUrl = response.data;
                    console.log(response.data);
                })
                .catch(error => {
                    console.error(error);
                });
        }
    }
}
</script>

<style lang="less" scoped>
</style>
