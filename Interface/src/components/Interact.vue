<template>
	<section class="w-100 h-100 d-flex justify-content-center align-items-center">
		<div :class="{ open: visible }" class="wrapper position-relative">
			<ul class="position-relative text-center">
				<li><a href="#"><font-awesome-icon icon="comment" /></a></li>
				<li><a href="#"><font-awesome-icon icon="map-marker-alt" /></a></li>
				<li><a href="#"><font-awesome-icon icon="user" /></a></li>
				<li><a href="#"><font-awesome-icon icon="hand-paper" /></a></li>
				<li><a href="#"><font-awesome-icon icon="sliders-h" /></a></li>
				<li><a href="#"><font-awesome-icon icon="car" /></a></li>
				<li><a href="#"><font-awesome-icon icon="camera" /></a></li>
				<li><a href="#"><font-awesome-icon icon="mobile-alt" /></a></li>
			</ul>
		</div>
	</section>
</template>

<script>
import FontAwesomeIcon from '@fortawesome/vue-fontawesome'

export default {
	name: 'Interact',

	components: {
		FontAwesomeIcon
	},

	data() {
		return {
			visible: false
		}
	},

	methods: {
		toggle() {
			this.visible = !this.visible
		}
	}
}
</script>

<style lang="scss" scoped>
.wrapper {
	width: 26em;
	height: 26em;
	border-radius: 50%;
	transition: all 0.3s ease;
	transform: scale(0);
}

.wrapper.open {
	transform: scale(1);
}

li {
	position: absolute;
	top: 50%;
	left: 50%;
	width: 10em;
	height: 10em;
	margin-top: -3.6em;
	margin-left: -10em;
	overflow: hidden;
	font-size: 2em;
	transform-origin: 100% 100%;
}

li a {
	position: absolute;
	right: -7.25em;
	bottom: -7.25em;
	width: 14.5em;
	height: 14.5em;
	padding-top: 1.8em;
	color: #fff;
	text-decoration: none;
	border-radius: 50%;
	transform: skew(-45deg) rotate(-70deg);
}

/* for a central angle x, the list items must be skewed by 90-x degrees
in our case x=40deg so skew angle is 50deg
items should be rotated by x, minus (sum of angles - 180)2s (for this demo) */

$items: 8;

@for $i from 0 to $items {
	li:nth-child(#{$i + 1}) {
		transform: rotate(#{(360 / $items) * ($i + 1)}deg) skew(#{(360 / $items)}deg);
	}
}

li:nth-child(odd) a {
	background-color: hsla(0, 88%, 60%, 0.7);
}

li:nth-child(even) a {
	background-color: hsla(0, 88%, 65%, 0.7);
}

li.active a {
	background-color: hsla(0, 88%, 70%, 0.8);
}

li:not(.active) a:hover {
	background-color: hsla(0, 88%, 70%, 0.9);
}

li:not(.active) a:focus {
	position: fixed;
}
</style>
