<template>
	<main>
		<h1 class="display-1 mb-5 text-white">Characters</h1>

		<div id="character-cards" class="card-deck">
			<character v-for="character in characters" :key="character.Id" :character="character" />

			<div class="card no-bg text-center pt-5">
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

		<div class="modal fade new-modal" data-backdrop="static" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
			<div class="modal-dialog modal-lg">
				<div class="modal-content">
					<div class="modal-header">
						<h3 class="modal-title">New Character</h3>

						<button type="button" class="close" data-dismiss="modal" aria-label="Close">
							<span aria-hidden="true">&times;</span>
						</button>
					</div>

					<form>
						<div class="modal-body">
							<div class="form-row">
								<div class="form-group col-md-4">
									<label for="forename">Forename</label>
									<input id="forename" class="form-control" type="text" value="John" name="forename" placeholder="required" pattern="^[a-zA-Z- ]{2,}$" title="Full forename, must be at least 2 characters" autocomplete="given-name" required />
								</div>

								<div class="form-group col-md-4">
									<label for="middlename">Middle Name(s)</label>
									<input id="middlename" class="form-control" type="text" value="" name="middlename" placeholder="optional" pattern="^[a-zA-Z- ]+$" title="Full middle name(s), separated by a space" autocomplete="additional-name" />
								</div>

								<div class="form-group col-md-4">
									<label for="surname">Surname</label>
									<input id="surname" class="form-control" type="text" value="Smith" name="surname" placeholder="required" pattern="^[a-zA-Z- ]{2,}$" title="Full surname, must be at least 2 characters" autocomplete="family-name" required />
								</div>
							</div>

							<div class="form-group">
								<div class="custom-control custom-radio custom-control-inline">
									<input id="genderMale" class="custom-control-input" type="radio" name="gender" value="0" checked required />
									<label class="custom-control-label" for="genderMale">Male</label>
								</div>

								<div class="custom-control custom-radio custom-control-inline">
									<input id="genderFemale" class="custom-control-input" type="radio" name="gender" value="1" required />
									<label class="custom-control-label" for="genderFemale">Female</label>
								</div>
							</div>

							<div class="form-group">
								<label for="dob">Date of Birth</label>
								<input id="dob" class="form-control" type="date" name="dob" value="1999-01-01" min="1900-01-01" max="2000-01-01" required />
							</div>
						</div>

						<div class="modal-footer">
							<button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
							<button type="submit" class="btn btn-success btn-lg">Create Character</button>
						</div>
					</form>
				</div>
			</div>
		</div>
	</main>
</template>

<script>
import { mapGetters } from 'vuex'
import { FontAwesomeIcon, FontAwesomeLayers } from '@fortawesome/vue-fontawesome'
import Character from './Character'

export default {
	name: 'CharacterSelect',

	components: {
		Character,
		FontAwesomeIcon,
		FontAwesomeLayers
	},

	computed: {
		...mapGetters([
			'characters'
		])
	}
}
</script>

<style scoped>
main {
	width: 60%;
	height: 60%;
	padding: 5% 0;
	margin: 0 auto;
}

h1 {
	font-family: pricedown, sans-serif;
	-webkit-text-fill-color: #fff;
	-webkit-text-stroke: 2.5px #000;
	text-shadow:
		3px 3px 0 #000,
		-1px -1px 0 #000,
		1px -1px 0 #000,
		-1px  1px 0 #000,
		1px  1px 0 #000;
}

.card.no-bg {
	background-color: transparent;
	border: 0;
}
</style>
