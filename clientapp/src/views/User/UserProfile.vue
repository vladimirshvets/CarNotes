<template>
    <v-container>
        <v-card class="user-form-wrapper">
            <v-card-title class="title d-flex flex-no-wrap">
                <span class="mr-auto">Driver License</span>
                <v-tooltip v-if="!editMode" text="Edit" location="bottom">
                    <template v-slot:activator="{ props }">
                        <v-icon v-bind="props" @click="toggleEditMode(true)">mdi-account-edit</v-icon>
                    </template>
                </v-tooltip>
                <v-tooltip v-if="editMode" text="Save" location="bottom">
                    <template v-slot:activator="{ props }">
                        <v-icon v-bind="props" @click="onSubmit">mdi-check</v-icon>
                    </template>
                </v-tooltip>
                <v-tooltip v-if="editMode" text="Cancel" location="bottom">
                    <template v-slot:activator="{ props }">
                        <v-icon v-bind="props" @click="toggleEditMode(false)">mdi-close</v-icon>
                    </template>
                </v-tooltip>
            </v-card-title>
            <v-card-subtitle>{{ user.email }}</v-card-subtitle>
            <v-card-text class="d-flex flex-no-wrap">
                <v-avatar
                    class="avatar mr-3"
                    size="160"
                    rounded="0"
                >
                    <v-img :src="require(`@/assets/user/profile/avatars/${randomAvatar}.jpg`)" alt="User Avatar"></v-img>
                </v-avatar>
                <v-form class="form">
                    <div class="user-prop">
                        <p class="label">Username</p>
                        <span v-if="!editMode">{{ user.userName }}</span>
                        <v-text-field
                            v-if="editMode"
                            v-model="formData.userName"
                            variant="underlined"
                            density="compact"
                            hide-details="auto"
                        ></v-text-field>
                    </div>
                    <div class="user-prop">
                        <p class="label">First Name</p>
                        <span v-if="!editMode">{{ user.firstName }}</span>
                        <v-text-field
                            v-if="editMode"
                            v-model="formData.firstName"
                            variant="underlined"
                            density="compact"
                            hide-details="auto"
                        ></v-text-field>
                    </div>
                    <div class="user-prop">
                        <p class="label">Last Name</p>
                        <span v-if="!editMode">{{ user.lastName }}</span>
                        <v-text-field
                            v-if="editMode"
                            v-model="formData.lastName"
                            variant="underlined"
                            density="compact"
                            hide-details="auto"
                        ></v-text-field>
                    </div>
                    <div class="user-prop">
                        <p class="label">Issued</p>
                        <span>{{ formatDate(user.createdAt) }}</span>
                    </div>
                </v-form>
            </v-card-text>
        </v-card>
    </v-container>
</template>

<script>
import api from '@/api.js';
import moment from 'moment';
export default {
    name: 'UserProfile',
    data() {
        return {
            user: {},
            editMode: false,
            formData: null,
            randomAvatar: 0
        }
    },
    mounted() {
        this.randomAvatar = Math.floor(Math.random() * 8);
    },
    async created() {
        await api
            .get(`/api/account/profile`)
            .then((response) => {
                this.user = response.data;
            })
            .catch((error) => {
                console.error(error);
            });
    },
    methods: {
        formatDate(date) {
            if (!date) {
                return null;
            }
            return moment(date).format('LL');
        },
        toggleEditMode(state) {
            if (state) {
                this.formData = {
                    userName: this.user.userName,
                    firstName: this.user.firstName,
                    lastName: this.user.lastName
                };
                this.editMode = true;
            } else {
                this.editMode = false;
                this.formData = false;
            }
        },
        async onSubmit() {
            await api
                .put(`/api/account/profile/${this.user.id}`, this.formData)
                .then(response => {
                    this.user = response.data;
                    this.toggleEditMode(false);
                })
                .catch(error => {
                    console.error(error);
                });
        }
    }
}
</script>

<style lang="less" scoped>
.user-form-wrapper {
    margin: 3em auto;
    padding: 10px;
    max-width: 500px;
    border-radius: 10px;
    box-shadow: 5px 5px 10px 2px #4E8C5B;

    .title {
        text-transform: uppercase;
    }

    .avatar {
        max-width: 40%;
    }

    .form {
        width: 100%;

        .user-prop {
            margin-bottom: 3px;

            .label {
                color: #016a59;
                font-weight: 600;
            }
        }
    }
}
</style>
