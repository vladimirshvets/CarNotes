<template>
    <h2>My Cars</h2>

    <section v-if="error">
        <p>Unable to retrieve the information at the moment, please try back later</p>
    </section>

    <section v-else>
        <div v-if="loading">
            Loading...
        </div>
        <div v-else>
            <li v-for="car in cars" :key="car.id">
                <span>{{ car.make }} {{ car.model }}</span>
            </li>
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

</style>
