<template>
    <v-snackbar
        v-model="model"
        :timeout="3000"
        color="#016a59"
        rounded="pill"
    >
        {{ snackbarText }}
    </v-snackbar>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
export default {
    name: 'SnackbarComponent',
    computed: {
        model: {
            get() {
                return this.snackbar;
            },
            set() {
                this.hideSnackbar();
            }
        },
        ...mapGetters([
            'snackbar',
            'snackbarText'
        ])
    },
    methods: {
        ...mapMutations([
            'hideSnackbar'
        ])
    }
}
</script>
