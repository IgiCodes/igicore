<template>
	<section class="row align-items-center h-100 text-center text-black">
		<div class="col">
			<font-awesome-icon icon="circle-notch" size="10x" spin fixed-width />
		</div>
	</section>
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
section {
	height: 100%;
}

</style>
