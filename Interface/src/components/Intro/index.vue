<template>
	<main>
		<transition name="component-fade">
			<component :is="pages[step - 1]" class="w-100 h-100 text-white" @next="next" @prev="prev" @finish="finish" />
		</transition>
	</main>
</template>

<script>
import { mapGetters } from 'vuex'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import Welcome from './Welcome'
import Rules from './Rules'

export default {
	name: 'Intro',

	components: {
		FontAwesomeIcon,

		Welcome,
		Rules
	},

	data() {
		return {
			step: 1,
			pages: ['Welcome', 'Rules']
		}
	},

	computed: {
		...mapGetters([
			'serverName'
		])
	},

	methods: {
		next() {
			if (this.step >= this.pages.length) {
				this.step = 1
			} else {
				this.step++
			}
		},

		prev() {
			if (this.step > 1) {
				this.step--
			}
		},

		finish() {
			this.$emit('finish')
		}
	}
}
</script>
