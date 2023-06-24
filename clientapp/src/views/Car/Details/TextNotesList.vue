<template>
    <section class="text-notes-list stats-section section-light">
        <div class="section-header">
            <div class="section-title">Text Notes</div>
        </div>
        <div class="section-content">
            <text-note-grid
                :items="items"
                @editItem="triggerForm(true)"
            />
            <text-note-form
                :showForm="showForm"
                @triggerForm="triggerForm"
                @save="save"
                @update="update"
                @remove="remove"
                :suggestedTags="tagList"
            />
            <v-btn
                class="button-add"
                icon="mdi-plus"
                size="large"
                @click="triggerForm(true)"
            ></v-btn>
        </div>
    </section>
</template>

<script>
import api from '@/api.js';
import { mapGetters, mapMutations } from 'vuex';
import TextNoteForm from '@/components/Car/Details/TextNote/TextNoteForm.vue';
import TextNoteGrid from '@/components/Car/Details/TextNote/TextNoteGrid.vue';


export default {
    name: 'ServicesList',
    components: {
        TextNoteForm,
        TextNoteGrid
    },
    computed: {
        tagList() {
            return this.items
                .map(r => r.tag)
                .filter((value, index, self) => value && self.indexOf(value) === index);
        },
        ...mapGetters([
            'isLoading'
        ])
    },
    data() {
        return {
            items: [],
            showForm: false
        }
    },
    async created() {
        await this.actualizeData();
    },
    methods: {
        async actualizeData() {
            this.$store.dispatch('loadMileages', this.$route.params.carId);
            this.getItems();
        },
        getItems() {
            this.setIsLoading(true);
            api
                .get(`/api/textNotes/getByCar/${this.$route.params.carId}`)
                .then((response) => {
                    this.items = response.data;
                })
                .finally(() => {
                    this.setIsLoading(false);
                });
        },
        async save(payload) {
            this.setIsLoading(true);
            await api
                .post('/api/textNotes', payload)
                .then(() => {
                    this.actualizeData();
                    this.triggerForm(false);
                    this.snackbar("The record has been saved.");
                })
                .catch(error => {
                    console.log(error);
                })
                .finally(() => {
                    this.setIsLoading(false);
                });
        },
        async update(id, payload) {
            this.setIsLoading(true);
            await api
                .put(`/api/textNotes/${id}`, payload)
                .then(() => {
                    this.actualizeData();
                    this.triggerForm(false);
                    this.snackbar("The record has been updated.")
                })
                .catch(error => {
                    console.log(error);
                })
                .finally(() => {
                    this.setIsLoading(false);
                });
        },
        async remove(id, payload) {
            this.setIsLoading(true);
            await api
                .delete(`/api/textNotes/${id}`, {
                    data: payload
                })
                .then(() => {
                    this.actualizeData();
                    this.triggerForm(false);
                    this.snackbar("The record has been removed.")
                })
                .catch(error => {
                    console.log(error.response.data);
                })
                .finally(() => {
                    this.setIsLoading(false);
                });
        },
        triggerForm(state) {
            this.showForm = state;
            if (!state) {
                this.setFormData({});
            }
        },
        ...mapMutations([
            'setIsLoading',
            'snackbar',
            'setFormData',
        ])
    }
}
</script>

<style lang="less" scoped>
.button-add {
    background-color: #016a59;
    color: #fff;
    position: fixed;
    right: 50px;
    bottom: 50px;
    z-index: 1000;
    transition: transform 0.3s;

    &:hover {
        transform: rotate(90deg) scale(1.1);
    }
}
</style>
