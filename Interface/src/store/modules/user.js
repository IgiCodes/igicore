const state = {
	user: {}
}

const getters = {
	user: state => state.user
}

const actions = {

}

const mutations = {
	setUser(state, user) {
		state.user = user
	}
}

export default {
	state,
	actions,
	getters,
	mutations
}
