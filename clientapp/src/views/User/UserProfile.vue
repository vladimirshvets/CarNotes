<template>
    <v-container>
        <v-card class="user-form-wrapper">
            <v-card-title class="title">Driver License</v-card-title>
            <v-card-subtitle>{{ user.email }}</v-card-subtitle>
            <v-card-text class="d-flex flex-no-wrap">
                <v-avatar
                    class="avatar mr-3"
                    size="160"
                    rounded="0"
                >
                    <v-img :src="require(`@/assets/user/profile/avatars/${randomAvatar}.jpg`)" alt="User Avatar"></v-img>
                </v-avatar>
                <div>
                    <div class="user-prop">
                        <p class="label">Username</p>
                        <span>{{ user.userName }}</span>
                    </div>
                    <div class="user-prop">
                        <p class="label">First Name</p>
                        <span>{{ user.firstName }}</span>
                    </div>
                    <div class="user-prop">
                        <p class="label">Last Name</p>
                        <span>{{ user.lastName }}</span>
                    </div>
                    <div class="user-prop">
                        <p class="label">Issued</p>
                        <span>{{ formatDate(user.createdAt) }}</span>
                    </div>
                </div>
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

    .user-prop {
        margin-bottom: 3px;

        .label {
            color: #016a59;
            font-weight: 600;
        }
    }
}
</style>
