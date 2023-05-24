import axios from 'axios';
import { store } from './store/index.js';
import { router } from '@/router.js';

const api = axios.create();

api.interceptors.request.use(
    config => {
        const jwt = store.getters.jwt;
        if (jwt) {
            config.headers.setAuthorization(`Bearer ${jwt}`);
        }
        return config;
    },
    error => Promise.reject(error)
);

api.interceptors.response.use(
    response => {
        return response;
    },
    error => {
        if (error.response.status === 401) {
            store.dispatch('resetJwt');
            router.push({ name: 'Login' });
        }
        return Promise.reject(error);
    }
)

export default api;
