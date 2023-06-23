<template>
    <v-dialog
        v-model="form"
        @click:outside="closeForm"
        width="1024"
        scrollable
    >
        <v-form
            @submit.prevent="submit(formData.id)"
            class="form"
        >
            <v-card>
                <v-card-title>
                    <span class="text-h5">Washing Info</span>
                </v-card-title>
                <v-card-text>
                    <small>* required fields</small>
                    <v-container>
                        <v-row>
                            <v-col v-if="!formData.id" cols="12" sm="12">
                                <v-switch
                                    v-model="useExistingMileage"
                                    hide-details
                                    inset
                                    color="success"
                                    label="Use existing mileage"
                                ></v-switch>
                            </v-col>
                            <v-col v-if="useExistingMileage" col="12" sm="12" md="12">
                                <mileage-input
                                    v-model="formData.mileage"
                                    :readonly="Boolean(formData.id)"
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
                            <v-col  cols="12" sm="12" md="12">
                                <span v-if="!useExistingMileage && Boolean(mileageMatch)">Mileage already exists</span>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="12" sm="6" md="6">
                                <v-combobox
                                    name="title"
                                    label="Title*"
                                    v-model="formData.title"
                                    :items="suggestedTitles"
                                    required
                                ></v-combobox>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-combobox
                                    name="address"
                                    label="Address"
                                    v-model="formData.address"
                                    :items="suggestedAddresses"
                                ></v-combobox>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="totalAmount"
                                    label="Total Amount, BYN"
                                    v-model="formData.totalAmount"
                                    required
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-checkbox
                                    name="isContact"
                                    label="Contact Washing"
                                    v-model="formData.isContact"
                                    color="info"
                                ></v-checkbox>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-checkbox
                                    name="isDegreaserUsed"
                                    label="Degreaser used"
                                    v-model="formData.isDegreaserUsed"
                                    color="info"
                                ></v-checkbox>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-checkbox
                                    name="isPolish"
                                    label="Polish used"
                                    v-model="formData.isPolishUsed"
                                    color="info"
                                ></v-checkbox>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-checkbox
                                    name="isAntiRainUsed"
                                    label="Anti-rain used"
                                    v-model="formData.isAntiRainUsed"
                                    color="info"
                                ></v-checkbox>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-checkbox
                                    name="isInteriorCleaned"
                                    label="Interior Cleaning"
                                    v-model="formData.isInteriorCleaned"
                                    color="info"
                                ></v-checkbox>
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
                        @click="removalModal = true"
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
        <delete-confirmation-modal
            :showModal="removalModal"
            @triggerModal="triggerRemovalModal"
            @submit="remove"
            title="Delete Washing"
            text="Are you sure you want to delete this record?"
        ></delete-confirmation-modal>
    </v-dialog>
</template>

<script>
import { mapGetters } from 'vuex';
import DeleteConfirmationModal from '@/components/Common/DeleteConfirmationModal.vue';
import MileageInput from '@/components/Car/Details/Common/MileageInput.vue';
export default {
    name: 'WashingForm',
    components: {
        MileageInput,
        DeleteConfirmationModal
    },
    props: {
        showForm: Boolean,
        suggestedTitles: Array,
        suggestedAddresses: Array
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
            let formData = this.formDataObj;
            formData.newMileage = {
                date: '',
                odometerValue: ''
            };
            return formData;
        },
        mileageMatch() {
            return this.mileages.find(item =>
                item.date == this.formData.newMileage.date &&
                item.odometerValue == this.formData.newMileage.odometerValue
            );
        },
        ...mapGetters([
            'mileages',
            'formDataObj'
        ])
    },
    data() {
        return {
            useExistingMileage: false,
            removalModal: false
        }
    },
    watch: {
        formData: function() {
            this.useExistingMileage = Boolean(this.formData.id);
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
                mileage: this.useExistingMileage
                    ? this.formData.mileage
                    : this.mileageMatch ?? this.formData.newMileage,
                title: this.formData.title,
                address: this.formData.address,
                isContact: this.formData.isContact,
                isDegreaserUsed: this.formData.isDegreaserUsed,
                isPolishUsed: this.formData.isPolishUsed,
                isAntiRainUsed: this.formData.isAntiRainUsed,
                isInteriorCleaned: this.formData.isInteriorCleaned,
                totalAmount: this.formData.totalAmount,
                comment: this.formData.comment
            };
            if (this.formData.id) {
                this.$emit('update', this.formData.id, payload);
            } else {
                this.$emit('save', payload);
            }
        },
        remove() {
            const payload = {
                carId: this.$route.params.carId,
                mileage: {
                    id: this.formData.mileage.id
                }
            }
            this.$emit('remove', this.formData.id, payload);
        },
        closeForm() {
            this.form = false;
        },
        triggerRemovalModal(state) {
            this.removalModal = state;
        }
    }
}
</script>

<style lang="less" scoped>
.form {
    overflow: scroll;
}
</style>
