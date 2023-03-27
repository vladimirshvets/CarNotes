<template>

    <section v-if="error">
        <p>Unable to retrieve the information at the moment, please try back later</p>
    </section>

    <section v-else>
        <div v-if="isLoading"></div>
        <div v-else class="d-flex flex-column">
            <v-card
                v-for="car in cars"
                :key="car.id"
                :title="car.make + ' ' + car.model.toString()"
                :subtitle="car.year.toString()"
                class="car-card-wrapper"
            >
                <v-card-text>
                    {{ car.plate }}
                </v-card-text>
                <v-card-actions>
                    <v-btn :to="{ name: 'CarStats', params: { carId: car.id} }">Details</v-btn>
                </v-card-actions>
            </v-card>
        </div>
    </section>

</template>

<script>
import axios from 'axios'
import { mapGetters, mapMutations } from 'vuex';

export default {
    name: 'CarsList',
    data() {
        return {
            cars: null,
            error: false
        }
    },
    computed: {
        ...mapGetters([
            'isLoading'
        ])
    },
    methods: {
        ...mapMutations([
            'setIsLoading'
        ])
    },
    mounted() {
        this.setIsLoading(true);
        axios
            .get('/api/cars/list')
            .then(response => {
                this.cars = response.data;
            })
            .catch(err => {
                console.log(err);
                this.error = true;
            })
            .finally(() => {
                this.setIsLoading(false)
            });
    }
}
</script>

<style scoped>
    .car-card-wrapper {
        margin: 1em 2em;
    }
</style>
