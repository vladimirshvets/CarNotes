<template>
    <v-banner
        color="info"
        icon="mdi-car-info"
        :stacked="false"
    >
        <div>
            <h3>Welcome to Car Notes!</h3>
        </div>
        <div>
            <span>Description of the website will appear here someday :)</span>
        </div>
        <div>
            <v-btn
                v-if="isAuthenticated"
                :to="{ name: 'Cars' }"
                size="large"
                color="info"
            >
                Go to my garage
            </v-btn>
            <span v-else>Please log in to explore some features.</span>
        </div>
    </v-banner>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
    name: 'HomePage',
    computed: {
        ...mapGetters([
            'isAuthenticated'
        ])
    }
}
</script>

<style lang="less" scoped>
.v-banner {
    width: 640px;
    margin: auto;
    padding: 2em;
    font-size: 20px;

    div {
        padding-bottom: 1em;
    }
}
</style>
