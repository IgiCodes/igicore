<template>
	<font-awesome-icon class="loading position-fixed text-light" icon="circle-notch" size="10x" spin fixed-width />
</template>

<script>
import FontAwesomeIcon from '@fortawesome/vue-fontawesome'

export default {
	name: 'Loading',

	components: {
		FontAwesomeIcon
	}
}
</script>

<style scoped>
.loading {
	top: 50%;
	left: 50%;
	margin-top: -0.625em;
	margin-left: -0.625em;
}
</style>
