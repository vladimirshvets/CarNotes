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
                    <span class="text-h5">Spare Part Info</span>
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
                            <v-col cols="12">Removal mileage</v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="12" sm="6" md="6">
                                <v-autocomplete
                                    name="category"
                                    label="Category*"
                                    v-model="formData.category"
                                    :items="suggestedCategories"
                                    required
                                ></v-autocomplete>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-combobox
                                    name="group"
                                    label="Group"
                                    v-model="formData.group"
                                    :items="suggestedGroups"
                                ></v-combobox>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="orderDate"
                                    label="Order Date"
                                    v-model="formData.orderDate"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="purchaseDate"
                                    label="Purchase Date"
                                    v-model="formData.purchaseDate"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-combobox
                                    name="name"
                                    label="Name"
                                    v-model="formData.name"
                                ></v-combobox>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-combobox
                                    name="uom"
                                    label="Unit of Measure"
                                    v-model="formData.uom"
                                ></v-combobox>
                            </v-col>
                            <v-col cols="12" sm="12" md="12">
                                <v-checkbox
                                    name="isOE"
                                    label="Original Equipment"
                                    v-model="formData.isOE"
                                    color="info"
                                ></v-checkbox>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="oeNumber"
                                    label="Original Equipment Number"
                                    v-model="formData.oeNumber"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="replacementNumber"
                                    label="Replacement Number"
                                    v-model="formData.replacementNumber"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="manufacturer"
                                    label="Manufacturer"
                                    v-model="formData.manufacturer"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="countryOfOrigin"
                                    label="Country of Origin"
                                    v-model="formData.countryOfOrigin"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="price"
                                    label="Price, BYN"
                                    v-model="formData.price"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="qty"
                                    label="Qty"
                                    v-model="formData.qty"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="shopWebsiteUrl"
                                    label="Shop Website URL"
                                    v-model="formData.shopWebsiteUrl"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="shopAddress"
                                    label="Shop Address"
                                    v-model="formData.shopAddress"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="productionDate"
                                    label="Production Date"
                                    v-model="formData.productionDate"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" md="6">
                                <v-text-field
                                    name="expirationDate"
                                    label="Expiration Date"
                                    v-model="formData.expirationDate"
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
            @remove="remove"
            title="Delete Spare Part"
            text="Are you sure you want to delete this record?"
        ></delete-confirmation-modal>
    </v-dialog>
</template>

<script>
import { mapGetters } from 'vuex';
import DeleteConfirmationModal from '@/components/DeleteConfirmationModal.vue';
import MileageInput from './MileageInput.vue';
export default {
    name: 'SparePartForm',
    components: {
        MileageInput,
        DeleteConfirmationModal
    },
    props: {
        showForm: Boolean,
        suggestedCategories: Array,
        suggestedGroups: Array
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
            let formData = this.$store.state.formData;
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
            'mileages'
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
                installationMileage: this.useExistingMileage
                    ? this.formData.mileage
                    : this.mileageMatch ?? this.formData.newMileage,
                removalMileage: null,
                category: this.formData.category,
                orderDate: this.formData.orderDate,
                purchaseDate: this.formData.purchaseDate,
                group: this.formData.group,
                name: this.formData.name,
                uom: this.formData.uom,
                isOE: this.formData.isOE,
                oeNumber: this.formData.oeNumber,
                replacementNumber: this.formData.replacementNumber,
                manufacturer: this.formData.manufacturer,
                countryOfOrigin: this.formData.countryOfOrigin,
                qty: this.formData.qty,
                price: this.formData.price,
                shopWebsiteUrl: this.formData.shopWebsiteUrl,
                shopAddress: this.formData.shopAddress,
                productionDate: this.formData.productionDate,
                expirationDate: this.formData.expirationDate,
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
                mileageId: this.formData.mileage.id
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
