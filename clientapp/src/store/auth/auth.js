import axios from 'axios';

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
            await axios
                .post('/api/account/login', credentials)
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
                    commit('setJwt', null);
                    localStorage.removeItem('jwt');

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
        async logout({ commit, getters }) {
            await axios
                .post('/api/account/logout', {}, {
                    headers: {
                        Authorization: `Bearer ${getters.jwt}`,
                    }
                })
                .then((response) => {
                    console.log(response);
                    commit('setJwt', null);
                    localStorage.removeItem('jwt');
                });
        }
    }
};

export default auth;
