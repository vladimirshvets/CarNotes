<template>
    <v-container>
        <v-row justify="center">
            <v-dialog
                v-model="form"
                @click:outside="closeForm"
                width="1024"
            >
                <v-form @submit.prevent="submit(formData.id)">
                    <v-card>
                        <v-card-title>
                            <span class="text-h5">Refueling Info</span>
                        </v-card-title>
                        <v-card-text>
                            <small>* required fields</small>
                            <v-container>
                                <v-row>
                                    <v-col cols="12" sm="12">
                                        <v-switch
                                            v-if="!formData.id"
                                            v-model="useExistingMileage"
                                            hide-details
                                            inset
                                            color="success"
                                            label="Use existing mileage"
                                        ></v-switch>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col v-if="useExistingMileage" col="12" sm="12" md="12">
                                        <mileage-input
                                            v-model="formData.mileage"
                                        />
                                    </v-col>
                                    <v-col v-if="!useExistingMileage" cols="12" sm="6" md="6">
                                        <v-text-field
                                            name="date"
                                            label="Date*"
                                            v-model="formData.newMileage.date"
                                            required
                                        ></v-text-field>
                                    </v-col>
                                    <v-col v-if="!useExistingMileage" cols="12" sm="6" md="6">
                                        <v-text-field
                                            name="odometerValue"
                                            label="Mileage, km*"
                                            v-model="formData.newMileage.odometerValue"
                                            required
                                        ></v-text-field>
                                    </v-col>
                                    <v-col cols="12" sm="6" md="6">
                                        <v-text-field
                                            name="volume"
                                            label="Volume*"
                                            v-model="formData.volume"
                                            required
                                        ></v-text-field>
                                    </v-col>
                                    <v-col cols="12" sm="6" md="6">
                                        <v-text-field
                                            name="price"
                                            label="Price per l., BYN*"
                                            v-model="formData.price"
                                            required
                                        ></v-text-field>
                                    </v-col>
                                    <v-col cols="12" sm="6" md="6">
                                        <v-text-field
                                            name="distributor"
                                            label="Distributor"
                                            v-model="formData.distributor"
                                        ></v-text-field>
                                    </v-col>
                                    <v-col cols="12" sm="6" md="6">
                                        <v-text-field
                                            name="address"
                                            label="Address"
                                            v-model="formData.address"
                                        ></v-text-field>
                                    </v-col>
                                    <v-col cols="12">
                                        <v-text-field
                                            name="comment"
                                            label="Comment"
                                            v-model="formData.comment"
                                        ></v-text-field>
                                    </v-col>
                                </v-row>
                            </v-container>
                        </v-card-text>
                        <v-card-actions>
                            <v-btn
                                v-if="formData.id"
                                color="red"
                                variant="outlined"
                                @click="remove"
                            >
                                <v-icon
                                    start
                                    icon="mdi-alert"
                                ></v-icon>
                                <span>Delete</span>
                            </v-btn>
                            <v-spacer></v-spacer>
                            <v-btn
                                color="blue-darken-1"
                                variant="text"
                                @click="closeForm"
                            >
                                <span>Cancel</span>
                            </v-btn>
                            <v-btn
                                type="submit"
                                color="success"
                                variant="outlined"
                            >
                                <span v-if="!formData.id">Save</span>
                                <span v-if="formData.id">Update</span>
                                <v-icon
                                    end
                                    icon="mdi-checkbox-marked-circle"
                                ></v-icon>
                            </v-btn>
                        </v-card-actions>
                    </v-card>
                </v-form>
            </v-dialog>
        </v-row>
    </v-container>
</template>

<script>
import MileageInput from './MileageInput.vue';
export default {
    name: 'RefuelingsForm',
    components: {
        MileageInput
    },
    props: {
        showForm: Boolean,
        distributorAutocomplete: Array,
        addressAutocomplete: Array
    },
    computed: {
        form: {
            get() {
                return this.showForm;
            },
            set(value) {
                this.$emit('triggerForm', value);
            }
        },
        formData() {
            let refuelingData = this.$store.state.formData;
            refuelingData.newMileage = {
                date: '',
                odometerValue: ''
            };
            return refuelingData;
        }
    },
    data() {
        return {
            useExistingMileage: false
        }
    },
    watch: {
        formData: function() {
            this.useExistingMileage = this.formData.id ? true : false;
        }
    },
    methods: {
        async submit() {
            // ToDo:
            // front-side validation.
            //const results = await event
            //alert(JSON.stringify(results, null, 2))
            const payload = {
                carId: this.$route.params.carId,
                mileage: this.useExistingMileage ? this.formData.mileage : this.formData.newMileage,
                volume: this.formData.volume,
                price: this.formData.price,
                distributor: this.formData.distributor,
                address: this.formData.address,
                comment: this.formData.comment
            };
            if (this.formData.id) {
                this.$emit('update', this.formData.id, payload);
            } else {
                this.$emit('save', payload);
            }
        },
        remove() {
            // ToDo:
            // Add confirmation dialog.
            const payload = {
                carId: this.$route.params.carId,
                mileageId: this.formData.mileage.id
            }
            this.$emit('remove', this.formData.id, payload);
        },
        closeForm() {
            this.form = false;
        },
    }
}
</script>
