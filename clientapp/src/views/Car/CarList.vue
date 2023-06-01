<template>
    <v-container>
        <section v-if="error">
            <p>Unable to retrieve the information at the moment, please try back later</p>
        </section>
        <section v-else>
            <div v-if="isLoading"></div>
            <div v-else>
                <div class="heading">
                    <div class="title">My Garage</div>
                    <div class="actions">
                        <v-btn :to="{ name: 'CarForm' }">Add car</v-btn>
                    </div>
                </div>
                <v-row>
                    <v-col
                        v-for="car in cars"
                        :key="car.id"
                        cols="12" lg="4" md="6" sm="6"
                    >
                    <!-- ToDo: move card to separate component -->
                        <v-card class="car-card-wrapper d-flex flex-column">
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
                                    <span class="title-text">{{ car.make }} {{ car.model?.toString() }} {{ car.generation?.toString() }}</span>
                                </v-badge>
                            </v-card-title>
                            <v-card-subtitle>
                                {{ car.year?.toString() }}
                            </v-card-subtitle>
                            <v-card-text class="d-flex flex-no-wrap">
                                <v-avatar
                                    class="ma-2"
                                    size="100"
                                    rounded="1"
                                >
                                    <v-img :src="require(`@/assets/car/profile/avatars/0.jpg`)" alt="Car Avatar"></v-img>
                                </v-avatar>
                                <div>
                                    <v-sheet v-if="car.plate" border rounded class="plate-number">
                                        {{ car.plate }}
                                    </v-sheet>
                                    <div v-if="car.vin">
                                        * {{ car.vin }} *
                                    </div>
                                    <div>
                                        {{ car.engineTypeText }}
                                    </div>
                                </div>
                            </v-card-text>
                            <v-card-subtitle
                                v-html="periodOfOwnership(car.ownedFrom, car.ownedTo)"
                            ></v-card-subtitle>
                            <v-card-actions class="mt-auto">
                                <v-tooltip text="Edit" location="right">
                                    <template v-slot:activator="{ props }">
                                        <v-btn :to="{ name: 'CarForm', params: { carId: car.id} }" icon="mdi-car-cog" v-bind="props"></v-btn>
                                    </template>
                                </v-tooltip>
                                <v-btn :to="{ name: 'CarStats', params: { carId: car.id} }" class="ml-auto">Details</v-btn>
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
import moment from 'moment';
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
            if (!from) {
                return "Till " + moment(to).format('ll');
            }

            const fromDate = moment(from);
            const toDate = to ? moment(to) : moment();

            const duration = moment.duration(toDate.diff(fromDate));
            const years = duration.years();
            const months = duration.months();
            const days = duration.days();
            let formattedDuration = "";
            if (years > 0) {
                formattedDuration += `${years} years `;
            }
            if (months > 0) {
                formattedDuration += `${months} months `;
            }
            if (days > 0) {
                formattedDuration += `${days} days `;
            }
            formattedDuration = formattedDuration.trim();

            if (!to) {
                return `Since ${fromDate.format('ll')}<br/>(${formattedDuration})`;
            }
            return `${fromDate.format('ll')} - ${toDate.format('ll')}<br/>(${formattedDuration})`;
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
.heading {
    padding: 1em 0 2em;

    .title {
        font-size: 24px;
        text-align: center;
    }
}

.overlay {
    background-color: rgba(0, 0, 0, 0.2);
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

    .plate-number {
        padding: 2px 4px;
        width: fit-content;
        font-size: 16px;
    }
}
</style>
