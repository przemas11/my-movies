import Vue from 'vue';
import App from './App.vue';
import PortalVue from "portal-vue";
import Vuelidate from 'vuelidate';
Vue.use(PortalVue);
Vue.use(Vuelidate);

new Vue({
    render: (h) => h(App),
}).$mount('#app');