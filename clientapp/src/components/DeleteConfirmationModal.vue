<template>
    <v-dialog
        v-model="modal"
        width="auto"
    >
        <v-card>
            <v-card-title>{{ title }}</v-card-title>
            <v-card-text>{{ text }}</v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                    color="blue-darken-1"
                    variant="text"
                    @click="modal = false"
                >
                    <span>Cancel</span>
                </v-btn>
                <v-btn
                    color="red"
                    variant="outlined"
                    @click="submit"
                >
                    Delete
                    <v-icon
                        end
                        icon="mdi-checkbox-marked-circle"
                    ></v-icon>
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
export default {
    name: 'DeleteConfirmationModal',
    props: {
        showModal: Boolean,
        title: String,
        text: String
    },
    computed: {
        modal: {
            get() {
                return this.showModal;
            },
            set(value) {
                this.$emit('triggerModal', value);
            }
        }
    },
    methods: {
        submit() {
            this.$emit('remove');
        }
    }
}
</script>
