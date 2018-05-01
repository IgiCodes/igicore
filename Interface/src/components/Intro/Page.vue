<script>
import { mapGetters } from 'vuex'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

export default {
	name: 'Welcome',

	components: {
		FontAwesomeIcon
	},

	computed: {
		...mapGetters([
			'serverName'
		])
	}
}
</script>

<style scoped>
section {
	height: 62%;
	overflow-y: scroll;
	background-color: rgba(0, 0, 0, 0.7);
	border-radius: 0.5rem;
}
</style>
