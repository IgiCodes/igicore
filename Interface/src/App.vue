<template>
	<main>
		<inventory v-show="showInventory" />
		<interact v-show="showInteract" ref="interact" />
	</main>
</template>

<script>
import Loading from './components/Loading'
import Inventory from './components/Inventory'
import Interact from './components/Interact'

import $ from 'jquery'
import 'bootstrap'


export default {
	name: 'App',

	components: {
		Loading,
		Inventory,
		Interact
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

			console.debug('onMessage', type, data)
		},

		onKeypress(e) {
			console.debug(e.keyCode)
			if (e.keyCode == 9) {
				if (this.showInventory) this.showInventory = false
				else this.showInventory = true
			}

			if (e.keyCode == 101) {
				if (this.showInteract) this.showInteract = false
				else this.showInteract = true

				this.$refs.interact.toggle()
			}
		}

	},

	data() {
		return {
			showInventory: false,
			showInteract: false
		}
	},

	mounted() {
		$(window).on('message', this.onMessage)
		//$(window).on('resize', this.onResize).trigger('resize')
		$(window).on('keypress', this.onKeypress)
	},

	beforeDestroy() {
		//$(window).off('resize', this.onResize)
		$(window).off('message', this.onMessage)
		$(window).off('keypress', this.onKeypress)
	}
}
</script>

<style lang="scss">
$body-bg: transparent;
$font-family-sans-serif: gravity;

@import '~bootstrap/scss/bootstrap';

@font-face {
	font-family: gravity;
	src: url(fonts/gravity.woff2);
}

@font-face {
	font-family: pricedown;
	src: url(fonts/pricedown.woff2);
}

html, body {
	width: 100%;
	height: 100%;
	overflow: hidden;
	margin: 0;
}

body > main {
	width: 100%;
	height: 100%;
	position: absolute;
	user-select: none;
	border: 1px solid red;
}
</style>
