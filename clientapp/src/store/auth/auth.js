import api from '@/api.js';

const auth = {
    state: {
        jwt: localStorage.getItem('jwt') || null,
    },
    getters: {
        jwt: state => state.jwt,
        isAuthenticated: state => Boolean(state.jwt)
    },
    mutations: {
        setJwt(state, jwt) {
            state.jwt = jwt;
        },
    },
    actions: {
        async login({ commit }, credentials) {
            var result = null;
            await api
                .post('/api/auth/login', credentials)
                .then((response) => {
                    const jwt = response.data.token;
                    commit('setJwt', jwt);
                    localStorage.setItem('jwt', jwt);
                    result = {
                        status: 200,
                        type: "success",
                        message: "Successfully logged in!"
                    };
                })
                .catch(error => {
                    result = {
                        status: error.response.status,
                        type: "error"
                    };
                    switch (error.response.status) {
                        case 401:
                            result.message = "Incorrect login or password.";
                            break;
                        case 500:
                            result.message = "There is a problem on the server. Please try again later.";
                            break;
                        default:
                            result.message = "Unknown error.";
                    }
                });
            return result;
        },
        async register({ commit }, credentials) {
            var result = null;
            await api
                .post('/api/auth/register', credentials)
                .then((response) => {
                    const jwt = response.data.token;
                    commit('setJwt', jwt);
                    localStorage.setItem('jwt', jwt);
                    result = {
                        status: 200,
                        type: "success",
                        message: "Successfully registered!"
                    };
                })
                .catch(error => {
                    result = {
                        status: error.response.status,
                        type: "error"
                    };
                    switch (error.response.status) {
                        case 409:
                            result.message = "A user with specified email already exists.";
                            break;
                        case 500:
                            result.message = "There is a problem on the server. Please try again later.";
                            break;
                        default:
                            result.message = "Unknown error.";
                    }
                });
            return result;
        },
        async logout({ dispatch }) {
            await api
                .post('/api/auth/logout')
                .then(() => {
                    dispatch('resetJwt');
                });
        },
        resetJwt({ commit }) {
            commit('setJwt', null);
            localStorage.removeItem('jwt');
        }
    }
};

export default auth;
