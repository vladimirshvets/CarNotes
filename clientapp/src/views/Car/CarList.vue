<template>
    <v-container>
        <section v-if="error">
            <p>Unable to retrieve the information at the moment, please try back later</p>
        </section>
        <section v-else>
            <div v-if="isLoading"></div>
            <div v-else>
                <v-row>
                    <v-col cols="12">
                        <h1>My Garage</h1>
                        <v-btn>Add car</v-btn>
                    </v-col>
                    <v-col
                        v-for="car in cars"
                        :key="car.id"
                        cols="12" md="4" sm="6"
                    >
                        <v-card class="car-card-wrapper">
                            <div
                                v-if="Boolean(car.ownedTo)"
                                class="overlay"
                            ></div>
                            <v-card-title>
                                <v-badge
                                    class="records-number-badge"
                                    :content="car.numberOfActionRecords"
                                    color="#009688"
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
                                <div class="info-text">
                                    {{ periodOfOwnership(car.ownedFrom, car.ownedTo) }}
                                </div>
                            </v-card-text>
                            <v-card-actions class="actions">
                                <v-btn :to="{ name: 'CarStats', params: { carId: car.id} }">Details</v-btn>
                            </v-card-actions>
                        </v-card>
                    </v-col>
                </v-row>
            </div>
        </section>
    </v-container>
</template>

<script>
import api from '@/api.js';
import { mapGetters, mapMutations } from 'vuex';

export default {
    name: 'CarList',
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
        api
            .get('/api/cars/list')
            .then(response => {
                this.cars = response.data;
                this.cars.forEach(car => {
                    api
                        .get(`/api/stats/action-records/${car.id}`)
                        .then(response => {
                            car.numberOfActionRecords = response.data;
                        })
                        .catch(err => {
                            console.log(err);
                            car.numberOfActionRecords = 0;
                        });
                });
            })
            .catch(() => {
                this.error = true;
            })
            .finally(() => {
                this.setIsLoading(false);
            });
    }
}
</script>

<style lang="less" scoped>
.overlay {
    background-color: rgba(0, 0, 0, 0.1);
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
}
.car-card-wrapper {
    height: 100%;

    .title-text {
        padding-right: 14px;
    }

    .info-text {
        display: inline-block;
    }
}
</style>
