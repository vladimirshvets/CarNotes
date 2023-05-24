import api from '@/api.js';

// ToDo: refactor
const common = {
    state: {
        isLoading: false,
        snackbar: {
            show: false,
            text: ''
        },
        formData: {},
        mileages: []
    },
    getters: {
        isLoading: state => state.isLoading,
        snackbar: state => state.snackbar.show,
        snackbarText: state => state.snackbar.text,
        formDataObj: state => state.formData,
        mileages: state => state.mileages,
    },
    mutations: {
        setIsLoading(state, isLoading) {
            state.isLoading = isLoading;
        },
        snackbar(state, text) {
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
            const result = await api.get(`/api/mileages/getByCar/${carId}`);
            let mileages = result.data;
            commit('setMileages', mileages);
        }
    }
}

export default common;
