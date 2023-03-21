<template>

    <div class="tab-wrap" id="car-profile">
        <v-card class="car-info-wrapper">
            <v-card-title>
                <span>{{ carInfo.make }} {{ carInfo.model }} {{ carInfo.generation }}</span>
                <v-sheet v-if="carInfo.plate" border rounded class="plate-number">
                    {{ carInfo.plate }}
                </v-sheet>
            </v-card-title>
            <v-card-subtitle>
                {{ carInfo.year }}
            </v-card-subtitle>
            <v-card-text>
                <div v-if="carInfo.vin">* {{ carInfo.vin }} *</div>
            </v-card-text>
        </v-card>

        <!-- https://github.com/vuejs/router/issues/845 -->
        <!-- <v-card class="car-tabs-wrapper">
            <v-card-title class="bg-teal">
                <v-tabs v-model="activeTab">
                    <v-tab v-for="tab in profileTabs" :key="tab.pid" :to="tab.route">
                        <ProfileTab :icon="tab.icon" :title="tab.title" />
                    </v-tab>
                </v-tabs>
            </v-card-title>

            <div class="car-profile-tab">
                <router-view></router-view>
            </div>
        </v-card> -->

        <v-card class="car-tabs-wrapper">
            <v-card-title class="links">
                <router-link class="car-profile-tab-link" :to="{ name: 'CarStats' }">
                    <v-icon>mdi-chart-bar</v-icon>
                    <span>Statistics</span>
                </router-link>
                <span class="link-separator">|</span>
                <router-link class="car-profile-tab-link" :to="{ name: 'RefuelingsList' }">
                    <v-icon>mdi-gas-station</v-icon>
                    <span>Refuelings</span>
                </router-link>
                <span class="link-separator">|</span>
                <router-link class="car-profile-tab-link" :to="{ name: 'WashingsList' }">
                    <v-icon>mdi-car-wash</v-icon>
                    <span>Washings</span>
                </router-link>
                <span class="link-separator">|</span>
                <router-link class="car-profile-tab-link" :to="{ name: 'SparePartsList' }">
                    <v-icon>mdi-car-turbocharger</v-icon>
                    <span>Spare Parts</span>
                </router-link>
                <span class="link-separator">|</span>
                <router-link class="car-profile-tab-link" :to="{ name: 'ServicesList' }">
                    <v-icon>mdi-car-wrench</v-icon>
                    <span>Services</span>
                </router-link>
                <span class="link-separator">|</span>
                <router-link class="car-profile-tab-link" :to="{ name: 'LegalProceduresList' }">
                    <v-icon>mdi-file-document-check</v-icon>
                    <span>Legal Procedures</span>
                </router-link>
            </v-card-title>

            <div class="car-profile-tab">
                <router-view></router-view>
            </div>
        </v-card>
    </div>

</template>

<script>
import axios from 'axios'
//import ProfileTab from '../../components/Car/Profile/ProfileTab.vue'

export default {
    name: "CarProfile",
    // components: {
    //     ProfileTab
    // },
    props: ["id"],
    data() {
        return {
            // activeTab: this.$route.path,
            // profileTabs: [
            //     { id: 1, icon: 'mdi-chart-bar', title: 'Statistics', route: { name: 'CarStats' } },
            //     { id: 2, icon: 'mdi-gas-station', title: 'Refuelings', route: { name: 'RefuelingsList' } },
            //     { id: 3, icon: 'mdi-car-wash', title: 'Washings', route: { name: 'WashingsList' } },
            //     { id: 4, icon: 'mdi-car-turbocharger', title: 'Spare Parts', route: { name: 'SparePartsList' } },
            //     { id: 5, icon: 'mdi-car-wrench', title: 'Services', route: { name: 'ServicesList' } },
            //     { id: 6, icon: 'mdi-file-document-check', title: 'Legal Procedures', route: { name: 'LegalProceduresList' } },
            // ],
            carInfo: {}
        }
    },
    async created() {
        const result = await axios.get(`/api/cars/${this.$route.params.id}`);
        const car = result.data;
        this.carInfo = car;
    }
}
</script>

<style lang="less" scoped>
    .car-info-wrapper {
        margin: 1em 2em;

        .plate-number {
            display: inline;
            margin-left: 10px;
            padding: 2px;
            height: 25px;
            width: 80px;
            font-size: 16px;
        }
    }

    .car-tabs-wrapper {
        margin: 0 2em;

        .links {
            background-color: #009688;
            color: #fff;
            font-size: 16px;

            .link-separator {
                margin: 0 10px;
            }
        }

        .car-profile-tab-link {
            text-decoration: none;
            cursor: pointer;
            color: #fff;
            font-weight: 500;

            &:hover {
                color: #e6ff84;
            }

            &.router-link-active {
                color: #cdff07;
            }

            i {
                font-size: 16px;
                margin-right: 5px;
                margin-bottom: 4px;
            }

            span {
                text-transform: uppercase;
            }
        }

        .car-profile-tab {
            margin: 1.4em;
        }
    }
</style>
