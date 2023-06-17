<template>
    <div class="tab-wrap" id="car-profile">
        <photo-slider></photo-slider>

        <!-- <v-navigation-drawer expand-on-hover rail>
            <v-list>
                <v-list-item
                    :prepend-avatar="require(`@/assets/car/profile/avatars/0.jpg`)"
                    :title="carFullName"
                    :subtitle="carSummary.plate"
                ></v-list-item>
            </v-list>
            <v-divider></v-divider>
            <v-list density="compact" nav>
                <v-list-item prepend-icon="mdi-gas-station" title="Refuelings" value="refuelings" :to="{ name: 'RefuelingsList' }"></v-list-item>
                <v-list-item prepend-icon="mdi-car-wash" title="Washings" value="washings" :to="{ name: 'WashingsList' }"></v-list-item>
                <v-list-item prepend-icon="mdi-car-turbocharger" title="Spare Parts" value="spareparts" :to="{ name: 'SparePartsList' }"></v-list-item>
                <v-list-item prepend-icon="mdi-car-wrench" title="Services" value="services" :to="{ name: 'ServicesList' }"></v-list-item>
                <v-list-item prepend-icon="mdi-file-document-check" title="Legal Procedures" value="legalprocedures" :to="{ name: 'LegalProceduresList' }"></v-list-item>
            </v-list>
        </v-navigation-drawer> -->

        <v-card class="car-summary-wrapper">
            <v-card-title>
                <span>{{ carSummary.make }} {{ carSummary.model }} {{ carSummary.generation }}</span>
                <v-sheet v-if="carSummary.plate" border rounded class="plate-number">
                    {{ carSummary.plate }}
                </v-sheet>
            </v-card-title>
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
                <router-view :carSummary="carSummary"></router-view>
            </div>
        </v-card>
    </div>

</template>

<script>
import api from '@/api.js';
import { mapGetters } from 'vuex';
import PhotoSlider from '@/components/Car/Profile/PhotoSlider.vue';
//import ProfileTab from '../../components/Car/Profile/ProfileTab.vue'

export default {
    name: "CarDetails",
    components: {
        PhotoSlider
        // ProfileTab
    },
    props: ["id"],
    computed: {
        carFullName() {
            return `${this.carSummary.make} ${this.carSummary.model} ${this.carSummary.generation}`
        },
        ...mapGetters([
            'jwt'
        ])
    },
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
            carSummary: {}
        }
    },
    async created() {
        await api
            .get(`/api/cars/${this.$route.params.carId}`)
            .then((response) => {
                this.carSummary = response.data;
            })
            .catch((error) => {
                console.error(error);
            });
    }
}
</script>

<style lang="less" scoped>
.car-summary-wrapper {
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
