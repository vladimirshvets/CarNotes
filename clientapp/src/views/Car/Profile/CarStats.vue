<template>
    <v-row>
        <v-col cols="12" md="4" sm="4">
            <div class="circle-wrap">
                <div class="circle-text">
                    <div class="value">{{ averageConsumption?.toFixed(2) }}</div>
                    <div class="label">l. / 100 km</div>
                </div>
            </div>
        </v-col>
        <v-col cols="12" md="4" sm="4">
            <div class="circle-wrap">
                <div class="circle-text">
                    <div class="value">{{ odometerDelta }}</div>
                    <div class="label">km total</div>
                </div>
            </div>
        </v-col>
        <v-col cols="12" md="4" sm="4">
            <div class="circle-wrap">
                <div class="circle-text">
                    <div class="value">{{ monthlyMileageStats?.toFixed(0) }}</div>
                    <div class="label">km per month</div>
                </div>
            </div>
        </v-col>
    </v-row>
    <MileageChart :key="mileageChartKey" :labels="mileageDates" :values="mileageValues" />
</template>

<script>
import api from '@/api.js';
import moment from 'moment';
import { mapGetters, mapMutations } from 'vuex';
import MileageChart from '../../../components/Car/Profile/Stats/MileageChart.vue'

export default {
    name: 'CarStats',
    props: {
        carSummary: Object
    },
    components: {
        MileageChart
    },
    data() {
        return {
            carId: this.$route.params.carId,
            averageConsumption: null,
            odometerDelta: null
        }
    },
    computed: {
        monthlyMileageStats() {
            const from = this.carSummary.ownedFrom;
            const to = this.carSummary.ownedTo;
            const fromDate = moment(from);
            const toDate = to ? moment(to) : moment();
            const duration = moment.duration(toDate.diff(fromDate));
            const days = duration.asDays();
            return days == 0 ? null : this.odometerDelta / days * 30.4375;
        },
        mileageChartKey() {
            return this.mileages.length;
        },
        mileageDates() {
            return this.mileages.map(m => m.date).reverse();
        },
        mileageValues() {
            return this.mileages.map(m => m.odometerValue).reverse();
        },
        ...mapGetters([
            'mileages'
        ])
    },
    created() {
        this.$store.dispatch('loadMileages', this.carId);
        api
            .get(`/api/stats/average-consumption/${this.carId}`)
            .then(response => {
                this.averageConsumption = response.data;
            });
        api
            .get(`/api/stats/odometer-delta/${this.carId}`)
            .then(response => {
                this.odometerDelta = response.data;
            });
    },
    methods: {
        ...mapMutations([
            'setMileages',
        ])
    }
}
</script>

<style lang="less" scoped>
.circle-wrap {
    margin: auto;
    width: 200px;
    height: 200px;
    border-radius: 50%;
    border: 1px solid #016a59;
    display: flex;
    justify-content: center;
    align-items: center;

    div {
        text-align: center;

        &.value {
            font-size: 28px;
        }

        &.label {
            color: #777;
        }
    }
}
</style>