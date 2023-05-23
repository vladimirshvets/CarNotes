import { createStore } from 'vuex';
import common from './common/common.js';
import auth from './auth/auth.js';

export const store = createStore({
    modules: {
        common,
        auth
    } 
});
