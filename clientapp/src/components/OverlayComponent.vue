<template>
    <v-overlay
        v-model="overlay"
        class="align-center justify-center"
    >
        <v-progress-circular
            color="primary"
            indeterminate
            size="64"
        ></v-progress-circular>
    </v-overlay>
</template>

<script>
  export default {
    data() {
        return {
            overlay: false
        }
    },
    methods: {
        triggerOverlay(state) {
            this.overlay = state;
        }
    }
  }
</script>
