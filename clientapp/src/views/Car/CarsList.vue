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
                class="car-card-wrapper"
            >
                <v-card-title>
                    <v-badge
                        color="info"
                        :content="car.numberOfActionRecords"
                    >
                    <span class="title-text">{{ car.make }} {{ car.model.toString() }}</span>
                    </v-badge>
                </v-card-title>
                <v-card-subtitle>
                    {{ car.year.toString() }}
                </v-card-subtitle>
                <v-card-text>
                    <div>
                        {{ car.plate }}
                    </div>
                    <div>
                        {{ car.engineTypeText }}
                    </div>
                    <div>
                        {{ periodOfOwnership(car.ownedFrom, car.ownedTo) }}
                    </div>
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
            'isLoading',
            'jwt'
        ])
    },
    methods: {
        periodOfOwnership(from, to) {
            if (!from && !to) {
                return "";
            }
            if (!to) {
                return from + " - now";
            }
            if (!from) {
                return "till " + to;
            }
            return from + " - " + to;
        },
        ...mapMutations([
            'setIsLoading'
        ])
    },
    mounted() {
        this.setIsLoading(true);
        axios
            .get('/api/cars/list', {
                headers: {
                    Authorization: `Bearer ${this.jwt}`,
                }
            })
            .then(response => {
                this.cars = response.data;
            })
            .catch(err => {
                console.log(err);
                this.error = true;
            })
            .finally(() => {
                this.setIsLoading(false)
                this.cars.forEach(car => {
                    axios
                        .get(`/api/stats/action-records/${car.id}`)
                        .then(response => {
                            car.numberOfActionRecords = response.data;
                        })
                        .catch(err => {
                            console.log(err);
                            car.numberOfActionRecords = 0;
                        });
                });
            });
    }
}
</script>

<style lang="less" scoped>
    .car-card-wrapper {
        margin: 1em 2em;

        .title-text {
            padding-right: 14px;
        }
    }
</style>
