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
import { mapState } from 'vuex';

  export default {
    name: 'OverlayComponent',
    computed: mapState({
        overlay: state => state.isLoading
    })
  }
</script>
