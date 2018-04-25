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
						<img src="../images/male.png"/>
					</div>
				</div>
			</div>
			<div @click="clickGear" class="section gear col-sm-3 offset-sm-1">
				<div v-show="showGear" class="gear">
				</div>
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
		clickGear() {
			if (this.showGear) this.showGear = false
			else this.showGear = true
		}
	}
}
</script>

<style scoped>
.inv-container {
	display: flex;
	justify-content: center;
	flex-direction: column;
	height: 100%;
	width: 100%;
}
.inv-container > .inventory {
	background: rgba(40,40,40,0.8);
	height: 80%;
}

.inventory > .section {
	height: 100%;
}
.inventory > .section > div {
	background: rgba(40,40,40,0.8);
	width: 100%;
	height: 100%;
}

.section.character  > .character {
	display: flex;
	background: none;
	justify-content: center;
	flex-direction: column;
}
.section.character > .character > .background {
	position: absolute;
}
.section.character > .character > .background > img {
	opacity: 0.3;
	width: 100%;
	-webkit-filter: drop-shadow(1px 1px 0 rgba(255,255,255,0.5))
		drop-shadow(-1px -1px 0 rgba(255,255,255,0.5));
	filter: drop-shadow(1px 1px 0 rgba(255,255,255,0.5))
		drop-shadow(-1px -1px 0 rgba(255,255,255,0.5));
}
.section.character > .character > .row {
	height: 25%;
	width: 100%;
	margin: 0;
}
.section.character > .character > .row > .slot-container {
	display: flex;
	justify-content: center;
	align-items: center;
}
.section.character > .character > .row > .slot-container > .slot {
	width: 6em;
	height: 6em;
	background: rgba(255,255,255,0.3);
}
</style>