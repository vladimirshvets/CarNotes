<template>
    <v-col col="12" sm="12" md="12">
        <v-autocomplete
            name="mileage"
            label="Mileage"
            v-model="mileage"
            :items="mileages"
            item-title="odometerValue"
            return-object
        >
            <template v-slot:item="{ props, item }">
                <v-list-item
                    v-bind="props"
                    :title="item?.raw?.odometerValue"
                    :subtitle="item?.raw?.date"
                ></v-list-item>
            </template>
        </v-autocomplete>
    </v-col>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
    name: 'MileageInput',
    data() {
        return {
            mileage: '',
        }
    },
    computed: {
        ...mapGetters([
            'mileages'
        ])
    }

}
</script>
