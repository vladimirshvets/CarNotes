<template>
    <h2>My Cars</h2>

    <section v-if="error">
        <p>Unable to retrieve the information at the moment, please try back later</p>
    </section>

    <section v-else>
        <div v-if="loading">
            Loading...
        </div>
        <div v-else class="d-flex flex-column">
            <v-card
                v-for="car in cars" 
                :key="car.id"
                :title="car.make + ' ' + car.model"
                :subtitle="car.year"
                class="car-card-wrapper"
                >
                <v-card-text>
                    {{ car.plate }}
                </v-card-text>
                <v-card-actions>
                    <v-btn :href="'cars/' + car.id">Details</v-btn>
                </v-card-actions>
            </v-card>
        </div>
    </section>

</template>

<script>
import axios from 'axios'

export default {
    name: 'MyCars',
    data() {
        return {
            cars: null,
            loading: true,
            error: false
        }
    },
    mounted() {
        axios
        .get('https://localhost:7140/api/cars/getByUser')
        .then(response => {
            this.cars = response.data;
        })
        .catch(error => {
            console.log(error);
            this.error = true;
        })
        .finally(() => (this.loading = false));
    }
}
</script>

<style scoped>
    .car-card-wrapper {
        margin: 1.2em;
    }
</style>
