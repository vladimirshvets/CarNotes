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
                        <v-btn :to="{ name: 'CarProfile' }">Add car</v-btn>
                    </div>
                </div>
                <v-row>
                    <v-col
                        v-for="carInstance in cars"
                        :key="carInstance.id"
                        cols="12" lg="4" md="6" sm="6"
                    >
                    <!-- ToDo: move card to separate component -->
                        <v-card class="car-card-wrapper d-flex flex-column">
                            <div
                                v-if="Boolean(carInstance.ownedTo)"
                                class="overlay"
                            ></div>
                            <v-card-title>
                                <v-badge
                                    class="records-number-badge"
                                    :content="carInstance.numberOfActionRecords"
                                    color="#009688"
                                >
                                    <span class="title-text">{{ carInstance.make }} {{ carInstance.model?.toString() }} {{ carInstance.generation?.toString() }}</span>
                                </v-badge>
                            </v-card-title>
                            <v-card-subtitle>
                                {{ carInstance.year?.toString() }}
                            </v-card-subtitle>
                            <v-card-text class="d-flex flex-no-wrap">
                                <v-avatar
                                    class="ma-2"
                                    size="100"
                                    rounded="1"
                                >
                                    <v-img
                                        v-if="carInstance.avatarUrl"
                                        :src="`/static/images/${carInstance.avatarUrl}`"
                                        cover
                                        alt="Car Avatar"
                                    ></v-img>
                                    <v-img
                                        v-else
                                        :src="require(`@/assets/car/avatars/logo_480.jpg`)"
                                        alt="Car Avatar"
                                    ></v-img>
                                </v-avatar>
                                <div>
                                    <v-sheet v-if="carInstance.plate" border rounded class="plate-number">
                                        {{ carInstance.plate }}
                                    </v-sheet>
                                    <div v-if="carInstance.vin">
                                        * {{ carInstance.vin }} *
                                    </div>
                                    <div>
                                        {{ carInstance.engineTypeText }}
                                    </div>
                                </div>
                            </v-card-text>
                            <v-card-subtitle
                                v-html="periodOfOwnership(carInstance.ownedFrom, carInstance.ownedTo)"
                            ></v-card-subtitle>
                            <v-card-actions class="mt-auto">
                                <v-tooltip text="Edit" location="right">
                                    <template v-slot:activator="{ props }">
                                        <v-btn :to="{ name: 'CarProfile', params: { carId: carInstance.id} }" icon="mdi-car-cog" v-bind="props"></v-btn>
                                    </template>
                                </v-tooltip>
                                <v-btn :to="{ name: 'CarStats', params: { carId: carInstance.id} }" class="ml-auto">Details</v-btn>
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
                formattedDuration += `${days} days`;
            }
            if (days == 0 && months == 0 && years == 0) {
                formattedDuration = "just created";
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
                this.cars.forEach(carInstance => {
                    api
                        .get(`/api/stats/action-records/${carInstance.id}`)
                        .then(response => {
                            carInstance.numberOfActionRecords = response.data;
                        })
                        .catch(err => {
                            console.log(err);
                            carInstance.numberOfActionRecords = 0;
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
