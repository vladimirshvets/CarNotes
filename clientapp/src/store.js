import { createStore } from 'vuex'

export const store = createStore({
    state: {
        isLoading: false,
        snackbar: {
            show: false,
            text: ''
        },
        formData: {}
    },
    getters: {
        isLoading: state => state.isLoading,
        formData: state => state.formData,
        snackbar: state => state.snackbar.show,
        snackbarText: state => state.snackbar.text,
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
        }
    },
    actions: {

    }
});
