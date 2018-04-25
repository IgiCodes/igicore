<template>
	<main>
		<loading v-show="false" />
		<character-select v-show="true" ref="CharacterSelect" />
		<inventory v-show="showInventory" />
		<interact v-show="showInteract" ref="Interact" />
	</main>
</template>

<script>
import $ from 'jquery'
import Loading from './components/Loading'
import CharacterSelect from './components/CharacterSelect/CharacterSelect'
import Inventory from './components/Inventory'
import Interact from './components/Interact'

export default {
	name: 'App',

	components: {
		Loading,
		CharacterSelect,
		Inventory,
		Interact
	},

	data() {
		return {
			showInventory: false,
			showInteract: false
		}
	},

	mounted() {
		//$(window).on('resize', this.onResize).trigger('resize')
		$(window).on('message', this.onMessage)
		$(window).on('keypress', this.onKeypress)
	},

	beforeDestroy() {
		//$(window).off('resize', this.onResize)
		$(window).off('message', this.onMessage)
		$(window).off('keypress', this.onKeypress)
	},

	methods: {
		onResize() {
			const page = $('body')

			// TODO: Get safezone size from game
			const safezone = 0.03 // 97%
			const x = (page.width() * safezone) / 2
			const width = page.width() - (x * 2)
			const y = (page.height() * safezone) / 2
			const height = page.height() - (y * 2)

			$('body > main').css({
				'left': x,
				'width': width,
				'top': y,
				'height': height
			})
		},

		onMessage(e) {
			if (e.originalEvent.data === undefined) return
			if (e.originalEvent.data.type === undefined) return

			const type = e.originalEvent.data.type
			const data = e.originalEvent.data.data || null

			console.debug('NUI', type, data)

			if (type == 'screen:character-creation:characters') {
				this.$refs.CharacterSelect.load(data)
			}
		},

		onKeypress(e) {
			if (!e.metaKey) e.preventDefault()

			if (e.key == 'q') { // 'tab' is hard to test in Chrome
				this.showInventory = !this.showInventory
			}

			if (e.key == 'e') {
				this.showInteract = !this.showInteract

				this.$refs.Interact.toggle()
			}
		}
	}
}
</script>

<style lang="scss">
$body-bg: transparent;
$font-family-sans-serif: gravity;

@import '~bootstrap/scss/bootstrap';

@font-face {
	font-family: gravity;
	src: url('fonts/gravity.woff2');
}

@font-face {
	font-family: pricedown;
	src: url('fonts/pricedown.woff2');
}

html,
body {
	width: 100%;
	height: 100%;
	margin: 0;
	overflow: hidden;
}

body > main {
	position: absolute;
	width: 100%;
	height: 100%;
	user-select: none;
	outline: none;

	/* border: 1px solid red; */
}
</style>
