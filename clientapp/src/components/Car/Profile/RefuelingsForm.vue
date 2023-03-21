<template>
    <v-container>
        <v-row justify="center">
            <v-dialog
                v-model="dialog"
                @click:outside="closeDialog"
                width="1024"
            >
                <v-form @submit.prevent="submit">
                    <v-card>
                        <v-card-title>
                            <span class="text-h5">Refueling Info</span>
                        </v-card-title>
                        <v-card-text>
                            <v-container>
                                <v-row>
                                    <v-col cols="12" sm="6" md="6">
                                        <v-text-field
                                            name="date"
                                            label="Date*"
                                            required
                                        ></v-text-field>
                                    </v-col>
                                    <v-col cols="12" sm="6" md="6">
                                        <v-text-field
                                            name="odometerValue"
                                            label="Mileage, km*"
                                            required
                                        ></v-text-field>
                                    </v-col>
                                    <v-col cols="12" sm="6" md="6">
                                        <v-text-field
                                            name="volume"
                                            label="Volume*"
                                            required
                                        ></v-text-field>
                                    </v-col>
                                    <v-col cols="12" sm="6" md="6">
                                        <v-text-field
                                            name="price"
                                            label="Price per l., BYN*"
                                            required
                                        ></v-text-field>
                                    </v-col>
                                    <v-col cols="12" sm="6" md="6">
                                        <v-text-field
                                            name="distributor"
                                            label="Distributor"
                                        ></v-text-field>
                                    </v-col>
                                    <v-col cols="12" sm="6" md="6">
                                        <v-text-field
                                            name="address"
                                            label="Address"
                                        ></v-text-field>
                                    </v-col>
                                    <v-col cols="12">
                                        <v-text-field
                                            name="comment"
                                            label="Comment"
                                        ></v-text-field>
                                    </v-col>
                                </v-row>
                            </v-container>
                        </v-card-text>
                        <v-card-actions>
                            <small>* required fields</small>
                            <v-spacer></v-spacer>
                            <v-btn
                                color="blue-darken-1"
                                variant="text"
                                @click="closeDialog"
                            >
                                Cancel
                            </v-btn>
                            <v-btn
                                type="submit"
                                color="success"
                                variant="outlined"
                            >
                                Save
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
export default {
    name: 'RefuelingsForm',
    props: [
        'showForm',
        'distributorAutocomplete',
        'addressAutocomplete'
    ],
    computed: {
        dialog: {
            get() {
                return this.showForm
            },
            set(value) {
                this.$emit('close', value)
            }
        }
    },
    methods: {
        async submit(event) {
            //const results = await event
            //alert(JSON.stringify(results, null, 2))
            const formData = event.target.elements;
            const payload = {
                carId: this.$route.params.id,
                date: formData.date.value,
                odometerValue: formData.odometerValue.value,
                volume: formData.volume.value,
                price: formData.price.value,
                distributor: formData.distributor.value,
                address: formData.address.value,
                comment: formData.comment.value
            };
            this.$emit('submit', payload);
        },
        closeDialog() {
            this.dialog = false;
        }
    }
}
</script>
