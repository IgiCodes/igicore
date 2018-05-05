<template>
	<transition name="component-fade">
		<component v-show="visible" :is="screen" @finish="screenFinished(screen)" @select="select" @disconnect="disconnect" />
	</transition>
</template>

<script>
import $ from 'jquery'
import Blank from './components/Blank'
import Loading from './components/Loading'
import Intro from './components/Intro'
import CharacterSelect from './components/CharacterSelect'
import Inventory from './components/Inventory'
import Interact from './components/Interact'
import Nui from './helpers/Nui'

export default {
	name: 'App',

	components: {
		// Screens
		Blank,
		Loading,
		Intro,
		CharacterSelect,

		// Menus
		Inventory,
		Interact
	},

	data() {
		return {
			visible: true,
			screen: 'Blank'
		}
	},

	mounted() {
		// Fake background
		if (window.emulateServer) {
			$('body').css({
				'background-image': 'url(../loading-screen/img/bg1.jpg)',
				'background-attachment': 'fixed',
				'background-size': 'cover'
			})
		}

		$(window).on('message', this.onMessage)
		$(window).on('keypress', this.onKeypress)
	},

	beforeDestroy() {
		$(window).off('message', this.onMessage)
		$(window).off('keypress', this.onKeypress)
	},

	methods: {
		onMessage(e) {
			if (e.originalEvent.data === undefined) return
			if (e.originalEvent.data.type === undefined) return

			const type = e.originalEvent.data.type
			const data = e.originalEvent.data.data || null

			console.debug('[NUI RECV]', type, data)

			if (type == 'client') {
				this.$store.commit('setEnvironment', data)

				this.screen = 'Loading'
			}

			if (type == 'user') {
				this.$store.commit('setUser', data)

				// If new user then show Intro
				if (!this.$store.getters.user.AcceptedRules) {
					this.screen = 'Intro'
				}
			}

			if (type == 'characters') {
				this.$store.commit('setCharacters', data)

				if (this.screen != 'Intro') this.screen = 'CharacterSelect'
			}
		},

		onKeypress(e) {
			if (e.key == 'e') {
				// TODO
			}
		},

		async screenFinished(screen) {
			console.debug('[SCRN DONE]', screen)

			if (screen == 'Intro') {
				// Store rules agreed
				await Nui.send('rules-agreed')

				this.screen = 'CharacterSelect'
			}

			if (screen == 'CharacterSelect') {
				this.screen = 'Intro'
			}
		},

		async select() {
			this.visible = false
			this.screen = 'Blank'
		},

		async disconnect() {
			this.screen = 'Loading'

			await Nui.send('disconnect')
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
	width: 100vw;
	height: 100vh;
	margin: 0;
	overflow: hidden;
	font-size: 1.5vh;
	user-select: none;
	outline: none;
}

body > main {
	width: 80vw;
	height: 90vh;
	margin: 5vh auto;
	overflow: hidden;
}

h1 {
	margin-left: 0.15vh;
	font-family: pricedown, sans-serif;
	-webkit-text-stroke: 0.15vh #000;
	color: #fff;
	text-shadow:
		0.4vh 0.4vh 0 #000,
		-0.15vh -0.15vh 0 #000,
		0.15vh -0.15vh 0 #000,
		-0.15vh 0.15vh 0 #000,
		0.15vh 0.15vh 0 #000;
	letter-spacing: -0.3vh;
}

.component-fade-enter-active {
	transition: all 0.5s ease;
}

.component-fade-leave-active {
	transition: all 0.1s;
}

.component-fade-enter,
.component-fade-leave-to {
	opacity: 0;
}

::-webkit-scrollbar {
	width: 1vw;
	height: 1vw;
}

::-webkit-scrollbar-thumb {
	background-color: #000;
	border-radius: 0.5rem;
}
</style>
