<template>
    <v-container>
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
                class="alert"
            ></v-alert>
            <div class="register-link">
                Don't have an account yet?
                <router-link :to="{ name: 'Register' }">Sign Up</router-link>
            </div>
            <v-divider class="divider"></v-divider>
            <v-card-subtitle>Log in with any popular provider:</v-card-subtitle>
            <v-card-text>
                <v-tooltip text="Google" location="bottom">
                    <template v-slot:activator="{ props }">
                        <v-btn v-bind="props" class="auth-btn google-auth" icon="mdi-google"></v-btn>
                    </template>
                </v-tooltip>
                <v-tooltip text="Facebook" location="bottom">
                    <template v-slot:activator="{ props }">
                        <v-btn v-bind="props" class="auth-btn facebook-auth" icon="mdi-facebook"></v-btn>
                    </template>
                </v-tooltip>
                <v-tooltip text="GitHub" location="bottom">
                    <template v-slot:activator="{ props }">
                        <v-btn v-bind="props" class="auth-btn github-auth" icon="mdi-github"></v-btn>
                    </template>
                </v-tooltip>
            </v-card-text>
        </v-card>
    </v-container>
</template>

<script>
import '@/styles/views/user/account/_forms.less';
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
            this.$store.dispatch('login', payload)
                .then((result) => {
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
        }
    }
}
</script>
