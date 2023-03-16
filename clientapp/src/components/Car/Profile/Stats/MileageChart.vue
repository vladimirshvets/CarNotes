<template>
    <div class="stats-section-wrap stats-mileage-chart">
        <div class="stats-section-title">
            <h4>Mileage History</h4>
        </div>
        <div class="stats-section-content">
            <Line :data="chartData" :options="chartOptions" />
        </div>
    </div>
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
} from 'chart.js'
import { Line } from 'vue-chartjs'

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
    .stats-mileage-chart canvas {
        height: 250px;
    }
</style>
