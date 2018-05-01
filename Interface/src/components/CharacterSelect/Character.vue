<template>
	<div class="card character-card bg-white float-left m-3">
		<h4 class="card-header">{{ character.Forename }} {{ character.Middlename }} {{ character.Surname }}</h4>

		<ul class="list-group list-group-flush">
			<li class="list-group-item"><font-awesome-icon icon="transgender" fixed-width class="mr-2" /> {{ character.GenderString }}</li>
			<li class="list-group-item"><font-awesome-icon icon="calendar" fixed-width class="mr-2" /> {{ character.DateOfBirthFormatted }}</li>
			<li class="list-group-item"><font-awesome-icon icon="dollar-sign" fixed-width class="mr-2" /> $1,000.00</li>
		</ul>

		<div class="card-body">
			<button type="button" class="btn btn-success btn-lg px-5 btn-load" @click="$emit('select', character.Id)">Play</button>
			<button type="button" class="btn btn-danger float-right mt-1 btn-delete" data-toggle="modal" data-target=".delete-modal"><font-awesome-icon icon="trash" fixed-width /></button>
		</div>

		<div ref="deleteModal" class="modal fade delete-modal" data-backdrop="static" tabindex="-1" role="dialog" aria-hidden="true">
			<div class="modal-dialog">
				<div class="modal-content">
					<div class="modal-header">
						<h3 class="modal-title">Delete Character</h3>

						<button type="button" class="close" data-dismiss="modal" aria-label="Close">
							<span aria-hidden="true">&times;</span>
						</button>
					</div>

					<div class="modal-body">
						<p>Are you sure you want to delete <b>{{ character.Forename }} {{ character.Middlename }} {{ character.Surname }}</b>? This cannot easily be undone.</p>
					</div>

					<div class="modal-footer">
						<button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
						<button type="submit" class="btn btn-danger" @click="$emit('delete')">Delete Character</button>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
import 'bootstrap'
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
	}
}
</script>

<style scoped>
.card {
	width: 33vh;
}
</style>
