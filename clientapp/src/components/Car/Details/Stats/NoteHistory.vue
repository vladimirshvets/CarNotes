<template>
    <section class="stats-section">
        <div class="section-header">
            <div class="section-title">Note History</div>
            <div class="section-subtitle">"Want {{ take }} more? No problem!"</div>
        </div>
        <div class="section-content">
            <!--
                ToDo:
                add a sort of section scroller
                to avoid the long page.
             -->
            <v-timeline side="end" align="start">
                <v-timeline-item
                    v-for="(mileage, index) in mileages"
                    :key="index"
                    :dot-color="dotColor(index)"
                    size="small"
                >
                    <template v-slot:opposite>
                        <div class="me-4">
                            <strong>{{ mileage.odometerValue }}</strong>
                            <div class="text-caption">{{ mileage.date }}</div>
                        </div>
                    </template>
                     <div class="d-flex">
                        <div class="notes-wrapper">
                            <v-list
                                v-if="mileage.notes.length"
                                class="note-items"
                                density="compact"
                                rounded="xl"
                            >
                                <v-list-item
                                    v-for="note in mileage.notes"
                                    :key="note.id"
                                    :prepend-icon="noteIcon(note.noteType)"
                                >
                                    {{ note.noteTitle }}
                                </v-list-item>
                            </v-list>
                        </div>
                    </div>
                </v-timeline-item>
            </v-timeline>
            <div class="actions">
                <v-btn v-if="canLoadMore" @click="fetchNoteHistory">
                    Load more
                </v-btn>
                <div v-if="!canLoadMore && mileages.length > 0">
                    Wow! Looks like you made it to the very beginning!
                </div>
                <div v-if="!canLoadMore && mileages.length === 0">
                    You have no notes yet.
                </div>
            </div>
        </div>
    </section>
</template>

<script>
import api from '@/api.js';
export default {
    name: 'NoteHistory',
    data() {
        return {
            mileages: [],
            skip: 0,
            take: 10,
            canLoadMore: false
        }
    },
    created() {
        this.fetchNoteHistory();
    },
    methods: {
        fetchNoteHistory()
        {
            api
                .get(
                    `/api/personal-stats/note-history/${this.$route.params.carId}
                    ?skip=${this.skip}&take=${this.take}`
                )
                .then(response => {
                    const responseLength = response.data.length;
                    this.canLoadMore = responseLength === this.take;
                    this.mileages = this.mileages.concat(response.data);
                    this.skip += responseLength;
                });
        },
        dotColor(index) {
            return index % 2 == 0 ? "pink" : "teal-lighten-3";
        },
        noteIcon(noteType) {
            const noteIcons = {
                LegalProcedure : "mdi-file-document-check",
                Refueling : "mdi-gas-station",
                Service : "mdi-car-wrench",
                SparePart : "mdi-car-turbocharger",
                TextNote : "mdi-note-text-outline",
                Washing : "mdi-car-wash",
            }

            return noteIcons[noteType];
        }
    }
}
</script>

<style lang="less" scoped>
.notes-wrapper {
    .note-items {
        background: initial;
        border: solid 1px #016a59;
    }
}
.actions {
    text-align: center;
}
</style>
