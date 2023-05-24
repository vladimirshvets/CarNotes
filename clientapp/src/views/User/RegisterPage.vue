<template>
    <v-container>
        <v-card class="register-form-wrapper">
            <v-card-title class="title">Create an account</v-card-title>
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
                <v-text-field
                    v-model="passwordConfirmation"
                    :readonly="loading"
                    :rules="[required, confirmPassword]"
                    clearable
                    label="Password Confirmation"
                    placeholder="Confirm your password"
                    type="password"
                ></v-text-field>
                <br>
                <v-btn
                    :disabled="!form || password !== passwordConfirmation"
                    :loading="loading"
                    block
                    color="success"
                    size="large"
                    type="submit"
                    variant="elevated"
                >
                    Sign Up
                </v-btn>
            </v-form>
            <v-alert
                v-if="outcomeType"
                :text="outcomeText"
                :type="outcomeType"
                variant="tonal"
                class="alert"
            ></v-alert>
            <div class="login-link">
                Already have an account?
                <router-link :to="{ name: 'Login' }">Sign In</router-link>
            </div>
        </v-card>
    </v-container>
</template>

<script>
import '@/styles/views/user/account/_forms.less';
import { router } from '@/router.js';
export default {
    name: 'RegisterPage',
    data: () => ({
        form: false,
        email: null,
        password: null,
        passwordConfirmation: null,
        loading: false,
        outcomeText: null,
        outcomeType: null
    }),

    methods: {
        async onSubmit() {
            if (!this.form) {
                return;
            }
            const payload = {
                "email": this.email,
                "password": this.password
            };

            this.outcomeType = null;
            this.loading = true;
            this.$store.dispatch('register', payload)
                .then(result => {
                    this.loading = false;
                    this.outcomeText = result.message;
                    this.outcomeType = result.type;
                    if (result.status === 200) {
                        setTimeout(() => {
                            router.push({ name: 'Cars'});
                        }, 500);
                    }
                });
        },
        required(v) {
            return !!v || 'Field is required'
        },
        confirmPassword(v) {
            return  v === this.password || 'Password confirmation doesn\'t match password'
        }
    }
}
</script>
