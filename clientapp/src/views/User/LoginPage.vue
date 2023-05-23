<template>
    <div class="d-flex flex-column">
        <v-card class="login-form-wrapper">
            <v-card-title class="title">Login</v-card-title>
            <v-form
                class="form"
                v-model="form"
                @submit.prevent="onSubmit"
            >
                <v-text-field
                    v-model="email"
                    :readonly="loading"
                    :rules="[required]"
                    class="mb-2"
                    clearable
                    label="Email"
                ></v-text-field>
                <v-text-field
                    v-model="password"
                    :readonly="loading"
                    :rules="[required]"
                    clearable
                    label="Password"
                    placeholder="Enter your password"
                    type="password"
                ></v-text-field>
                <br>

                <v-btn
                    :disabled="!form"
                    :loading="loading"
                    block
                    color="success"
                    size="large"
                    type="submit"
                    variant="elevated"
                >
                    Sign In
                </v-btn>
            </v-form>
            <v-alert
                v-if="outcomeType"
                :text="outcomeText"
                :type="outcomeType"
                variant="tonal"
            ></v-alert>
            <v-divider class="divider"></v-divider>
            <v-card-subtitle>Log in with any popular provider:</v-card-subtitle>
            <v-card-text>
                <v-btn class="auth-btn google-auth" prepend-icon="mdi-google">Google (not impl. yet)</v-btn>
                <v-btn class="auth-btn github-auth" prepend-icon="mdi-github">GitHub (not impl. yet)</v-btn>
            </v-card-text>
        </v-card>
    </div>
</template>

<script>
import { router } from '@/router.js';
export default {
    name: 'LoginPage',
    data: () => ({
        form: false,
        email: null,
        password: null,
        loading: false,
        outcomeText: null,
        outcomeType: null
    }),

    methods: {
        async onSubmit () {
            if (!this.form) {
                return;
            }

            this.outcomeType = null;
            this.loading = true;

            const payload = {
                "email": this.email,
                "password": this.password
            };
            this.$store.dispatch('login', payload)
                .then((result) => {
                    this.loading = false;
                    this.outcomeText = result.message;
                    this.outcomeType = result.type;
                    if (result.status === 200) {
                        router.push({ name: 'Cars'});
                    }
                });
        },
        required (v) {
            return !!v || 'Field is required'
        }
    }
}
</script>

<style lang="less" scoped>
.login-form-wrapper {
    margin: 2em auto;
    padding: 0 2em 2em;
    width: 60%;

    .form {
        margin-bottom: 1em;
    }

    .divider {
        margin: 1em;
    }

    .title {
        padding-top: 2em;
        padding-bottom: 2em;
        text-align: center;
        text-transform: uppercase;
    }

    .auth-btn {
        width: 100%;
        margin: 1em 0;

        &.google-auth {
            color: #fff;
            background-color: #db4437;
        }

        &.github-auth {}
    }
}
</style>
