<template>
	<div class="inv-container row">
		<div class="inventory col-sm-10 offset-sm-1 row">
			<div class="section character col-sm-3 ">
				<div class="character">
					<div class="row">
						<div class="col-sm-4 slot-container">
							<div class="slot"></div>
						</div>
						<div class="col-sm-4 slot-container">
							<div class="slot"></div>
						</div>
						<div class="col-sm-4 slot-container">
							<div class="slot"></div>
						</div>
					</div>
					<div class="row">
						<div class="col-sm-4 slot-container">
							<div class="slot"></div>
						</div>
						<div class="col-sm-4 slot-container">
							<div class="slot"></div>
						</div>
						<div class="col-sm-4 slot-container">
							<div class="slot"></div>
						</div>
					</div>
					<div class="row">
						<div class="col-sm-4 slot-container">
							<div class="slot"></div>
						</div>
						<div class="col-sm-4 slot-container">
							<div class="slot"></div>
						</div>
						<div class="col-sm-4 slot-container">
							<div class="slot"></div>
						</div>
					</div>
					<div class="row">
						<div class="col-sm-4 slot-container">
							<div class="slot"></div>
						</div>
						<div class="col-sm-4 slot-container">
							<div class="slot"></div>
						</div>
						<div class="col-sm-4 slot-container">
							<div class="slot"></div>
						</div>
					</div>
					<div class="background col-sm-12">
						<img src="../images/male.png" />
					</div>
				</div>
			</div>

			<div class="section gear col-sm-3 offset-sm-1" @click="toggleGear">
				<div v-show="showGear" class="gear"></div>
			</div>

			<div class="section col-sm-4 offset-sm-1">
				<div></div>
			</div>
		</div>
	</div>
</template>

<script>
export default {
	name: 'Inventory',

	data() {
		return {
			showGear: true
		}
	},

	methods: {
		toggleGear() {
			this.showGear = !this.showGear
		}
	}
}
</script>

<style scoped>
.inv-container {
	display: flex;
	flex-direction: column;
	justify-content: center;
	width: 100%;
	height: 100%;
}

.inv-container > .inventory {
	height: 80%;
	background: rgba(40, 40, 40, 0.8);
}

.inventory > .section {
	height: 100%;
}

.inventory > .section > div {
	width: 100%;
	height: 100%;
	background: rgba(40, 40, 40, 0.8);
}

.section.character > .character {
	display: flex;
	flex-direction: column;
	justify-content: center;
	background: none;
}

.section.character > .character > .background {
	position: absolute;
}

.section.character > .character > .background > img {
	width: 100%;
	filter:
		drop-shadow(1px 1px 0 rgba(255, 255, 255, 0.5))
		drop-shadow(-1px -1px 0 rgba(255, 255, 255, 0.5));
	opacity: 0.3;
}

.section.character > .character > .row {
	width: 100%;
	height: 25%;
	margin: 0;
}

.section.character > .character > .row > .slot-container {
	display: flex;
	align-items: center;
	justify-content: center;
}

.section.character > .character > .row > .slot-container > .slot {
	width: 6em;
	height: 6em;
	background: rgba(255, 255, 255, 0.3);
}
</style>
