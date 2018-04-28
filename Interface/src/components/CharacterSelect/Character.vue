<template>
	<div class="card character-card bg-white">
		<h4 class="card-header">{{ character.Forename }} {{ character.Middlename }} {{ character.Surname }}</h4>

		<ul class="list-group list-group-flush">
			<li class="list-group-item"><font-awesome-icon icon="transgender" fixed-width class="mr-2" /> {{ character.GenderString }}</li>
			<li class="list-group-item"><font-awesome-icon icon="calendar" fixed-width class="mr-2" /> {{ character.DateOfBirthFormatted }}</li>
			<li class="list-group-item"><font-awesome-icon icon="dollar-sign" fixed-width class="mr-2" /> $1,000.00</li>
		</ul>

		<div class="card-body">
			<button type="button" class="btn btn-success btn-lg px-5 btn-load" @click="selectCharacter">Play</button>
			<button type="button" class="btn btn-danger float-right mt-1 btn-delete" @click="deleteCharacter"><font-awesome-icon icon="trash" fixed-width /></button>
		</div>
	</div>
</template>

<script>
import FontAwesomeIcon from '@fortawesome/vue-fontawesome'

export default {
	name: 'Character',

	components: {
		FontAwesomeIcon
	},

	props: {
		character: {
			type: Object,
			required: true
		}
	},

	methods: {
		async selectCharacter() {
			await this.$store.dispatch('selectCharacter', this.character.Id)
		},

		async deleteCharacter() {
			await this.$store.dispatch('selectCharacter', this.character.Id)
		}
	}
}
</script>
