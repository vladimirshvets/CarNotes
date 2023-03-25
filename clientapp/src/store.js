import { createStore } from 'vuex'
import axios from 'axios';

export const store = createStore({
    state: {
        isLoading: false,
        snackbar: {
            show: false,
            text: ''
        },
        formData: {},
        mileages: [],
    },
    getters: {
        isLoading: state => state.isLoading,
        snackbar: state => state.snackbar.show,
        snackbarText: state => state.snackbar.text,
        formData: state => state.formData,
        mileages: state => state.mileages
    },
    mutations: {
        setIsLoading(state, isLoading) {
            state.isLoading = isLoading;
        },
        showSnackbar(state, text) {
            state.snackbar.show = true;
            state.snackbar.text = text;
        },
        hideSnackbar(state) {
            state.snackbar.show = false;
        },
        setFormData(state, formData) {
            state.formData = formData;
        },
        setMileages(state, mileages) {
            state.mileages = mileages;
        }
    },
    actions: {
        async loadMileages({ commit }, carId) {
            const result = await axios.get(`/api/mileages/getByCar/${carId}`);
            let mileages = result.data;
            commit('setMileages', mileages);
        }
    }
});
