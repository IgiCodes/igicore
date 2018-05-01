<template>
	<main>
		<div class="row">
			<div class="col-8">
				<h1 class="display-1 mb-5 text-white">Characters</h1>
			</div>

			<div class="col-4 text-right">
				<button type="button" class="btn btn-outline-light mt-5 mr-3" @click="$emit('finish')"><font-awesome-icon icon="info" fixed-width /> Info</button>
				<button type="button" class="btn btn-outline-danger mt-5" data-toggle="modal" data-target=".disconnect-modal"><font-awesome-icon icon="times" fixed-width /> Disconnect</button>
			</div>
		</div>

		<section class="container-fluid mt-4">
			<div class="row">
				<div v-for="character in characters" :key="character.Id" class="col-auto mb-3">
					<character :character="character" @select="selectCharacter" />
				</div>

				<div class="col-auto mb-3">
					<div class="card border-0 bg-transparent text-center pt-5">
						<div class="card-body">
							<a href="#" class="text-success" data-toggle="modal" data-target=".new-modal">
								<font-awesome-layers class="fa-7x" fixed-width>
									<font-awesome-icon icon="square" class="text-white" />
									<font-awesome-icon icon="plus-square" />
								</font-awesome-layers>
							</a>
						</div>
					</div>
				</div>
			</div>
		</section>

		<div ref="newModal" class="modal fade new-modal" data-backdrop="static" tabindex="-1" role="dialog" aria-hidden="true">
			<div class="modal-dialog modal-lg">
				<div class="modal-content">
					<div class="modal-header">
						<h3 class="modal-title">New Character</h3>

						<button type="button" class="close" data-dismiss="modal" aria-label="Close">
							<span aria-hidden="true">&times;</span>
						</button>
					</div>

					<form @submit.prevent="submitNew">
						<div class="modal-body">
							<div class="form-row">
								<div class="form-group col-md-4">
									<label for="forename">Forename</label>
									<input id="forename" v-model="newCharacter.forename" class="form-control" type="text" name="forename" placeholder="required" pattern="^[a-zA-Z- ]{2,}$" title="Full forename, must be at least 2 characters" autocomplete="given-name" required />
								</div>

								<div class="form-group col-md-4">
									<label for="middlename">Middle Name(s)</label>
									<input id="middlename" v-model="newCharacter.middlename" class="form-control" type="text" name="middlename" placeholder="optional" pattern="^[a-zA-Z- ]+$" title="Full middle name(s), separated by a space" autocomplete="additional-name" />
								</div>

								<div class="form-group col-md-4">
									<label for="surname">Surname</label>
									<input id="surname" v-model="newCharacter.surname" class="form-control" type="text" name="surname" placeholder="required" pattern="^[a-zA-Z- ]{2,}$" title="Full surname, must be at least 2 characters" autocomplete="family-name" required />
								</div>
							</div>

							<div class="form-group">
								<div class="custom-control custom-radio custom-control-inline">
									<input id="genderMale" v-model.number="newCharacter.gender" class="custom-control-input" type="radio" name="gender" value="0" required />
									<label class="custom-control-label" for="genderMale">Male</label>
								</div>

								<div class="custom-control custom-radio custom-control-inline">
									<input id="genderFemale" v-model.number="newCharacter.gender" class="custom-control-input" type="radio" name="gender" value="1" required />
									<label class="custom-control-label" for="genderFemale">Female</label>
								</div>
							</div>

							<div class="form-group">
								<label for="dob">Date of Birth</label>
								<input id="dob" v-model="newCharacter.dob" class="form-control" type="date" name="dob" min="1900-01-01" max="2000-01-01" required />
							</div>
						</div>

						<div class="modal-footer">
							<button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
							<button type="submit" class="btn btn-success">Create Character</button>
						</div>
					</form>
				</div>
			</div>
		</div>

		<div ref="disconnectModal" class="modal fade disconnect-modal" data-backdrop="static" tabindex="-1" role="dialog" aria-hidden="true">
			<div class="modal-dialog">
				<div class="modal-content">
					<div class="modal-header">
						<h3 class="modal-title">Disconnect</h3>

						<button type="button" class="close" data-dismiss="modal" aria-label="Close">
							<span aria-hidden="true">&times;</span>
						</button>
					</div>

					<div class="modal-body">
						<p>Are you sure you want to disconnect from <b>{{ serverName }}</b>?</p>
					</div>

					<div class="modal-footer">
						<button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
						<button type="submit" class="btn btn-danger" @click="disconnect">Disconnect</button>
					</div>
				</div>
			</div>
		</div>
	</main>
</template>

<script>
import { mapGetters } from 'vuex'
import { FontAwesomeIcon, FontAwesomeLayers } from '@fortawesome/vue-fontawesome'
import 'bootstrap'
import $ from 'jquery'
import Character from './Character'
import Nui from '../../helpers/Nui'

export default {
	name: 'CharacterSelect',

	components: {
		Character,
		FontAwesomeIcon,
		FontAwesomeLayers
	},

	data() {
		return {
			newCharacter: {
				forename: '',
				middlename: '',
				surname: '',
				gender: 0,
				dob: '1999-01-01'
			}
		}
	},

	computed: {
		...mapGetters([
			'characters',
			'serverName'
		])
	},

	methods: {
		async submitNew() {
			await Nui.send('character-create', this.newCharacter)

			$(this.$refs.newModal).modal('hide')

			this.newCharacter = {
				forename: '',
				middlename: '',
				surname: '',
				gender: 0,
				dob: '1999-01-01'
			}
		},

		async selectCharacter(id) {
			await this.$store.dispatch('selectCharacter', id)

			this.$emit('select')
		},

		async deleteCharacter() {
			await this.$store.dispatch('deleteCharacter', this.character.Id)

			$(this.$refs.deleteModal).modal('hide')
		},

		disconnect() {
			$(this.$refs.disconnectModal).modal('hide')

			this.$emit('disconnect')
		}
	}
}
</script>

<style scoped>
section {
	height: 80%;
	overflow-y: scroll;
}
</style>
