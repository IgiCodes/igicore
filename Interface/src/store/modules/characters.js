import Nui from '../../helpers/Nui'

const state = {
	all: [],
	selected: ''
}

const getters = {
	characters: state => state.all,
	character: state => state.all.find(c => c.Id == state.selected) || null
}

const actions = {
	async selectCharacter({ commit }, id) {
		await Nui.send('character-load', id)

		commit('setCharacter', id)
	},

	async deleteCharacter({ commit }, id) {
		await Nui.send('character-delete', id)

		commit('setCharacter', null)
	}
}

const mutations = {
	setCharacter(state, id) {
		state.selected = id
	},

	setCharacters(state, characters) {
		state.all = []

		for (let i = 0; i < characters.length; i++) {
			const character = characters[i]

			character.GenderString = character.Gender == 0 ? 'Male' : 'Female'

			const dob = new Date(character.DateOfBirth)
			character.DateOfBirthFormatted = dob.getDate() + ' ' + dob.toLocaleString('en-US', {month: 'long'}) + ' ' + dob.getFullYear()

			state.all.push(character)
		}
	}
}

export default {
	state,
	getters,
	actions,
	mutations
}
