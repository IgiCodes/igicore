import Vue from 'vue'
import Vuex from 'vuex'
import server from './modules/server'
import characters from './modules/characters'

Vue.use(Vuex)

export default new Vuex.Store({
	strict: process.env.NODE_ENV !== 'production',

	modules: {
		server,
		characters
	}
})
