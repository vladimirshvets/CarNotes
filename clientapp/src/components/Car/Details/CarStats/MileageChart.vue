<template>
    <section class="stats-section section-dark">
        <div class="section-header">
            <div class="section-title">Mileage History</div>
            <div class="section-content mileage-chart">
                <Line :data="chartData" :options="chartOptions" />
            </div>
        </div>
    </section>
</template>

<script>
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
} from 'chart.js';
import { Line } from 'vue-chartjs';

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
)

export default {
    name: 'MileageChart',
    components: {
        Line
    },
    props: ['labels', 'values'],
    data() {
        return {
            chartData: {
                labels: this.labels,
                datasets: [
                    {
                        label: 'Mileage, km',
                        backgroundColor: '#f87979',
                        data: this.values
                    }
                ]
            },
            chartOptions: {
                responsive: true,
                maintainAspectRatio: false
            }
        }
    }
}
</script>

<style scoped>
    .mileage-chart canvas {
        height: 250px;
    }
</style>
