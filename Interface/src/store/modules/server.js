const state = {
	resource: 'igicore',
	name: ''
}

const getters = {
	resourceName: state => state.resource,
	serverName: state => state.name
}

const actions = {

}

const mutations = {
	setEnvironment(state, environment) {
		state.resource = environment.ResourceName
		state.name = environment.ServerName
	}
}

export default {
	state,
	actions,
	getters,
	mutations
}
