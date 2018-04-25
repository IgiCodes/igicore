<template>
	<div class="interactScreen">
		<div class="interactContainer">
			<div class="cn-wrapper" id="cn-wrapper">
			<ul>
				<li><a href="#"><span class="icon-picture"></span></a></li>
				<li><a href="#"><span class="icon-headphones"></span></a></li>
				<li><a href="#"><span class="icon-home"></span></a></li>
				<li><a href="#"><span class="icon-facetime-video"></span></a></li>
				<li><a href="#"><span class="icon-envelope-alt"></span></a></li>
				<li><a href="#"><span class="icon-picture"></span></a></li>
				<li><a href="#"><span class="icon-headphones"></span></a></li>
				<li><a href="#"><span class="icon-home"></span></a></li>
			</ul>
			</div>
			<div id="cn-overlay" class="cn-overlay"></div>
		</div>
	</div>
</template>

<script>

import $ from 'jquery'

export default {
	name: 'Interact',
	data() {
		return {
			msg: 'Hello World',
			visible: false
		}
	},
	methods: {
		toggle() {
			if(!this.visible){
				console.debug('Opening Nav')
				this.openNav()
			}
			else{
				console.debug('Closing Nav')
				this.closeNav()
			}
		},
		openNav(){
			this.visible = true
			$('#cn-wrapper').addClass('opened-nav')
		},
		closeNav(){
			this.visible = false
			$('#cn-wrapper').removeClass('opened-nav')
		}
	},
	mounted() {

	}
}
</script>


<style scoped>
@import url(http://netdna.bootstrapcdn.com/font-awesome/2.0/css/font-awesome.css);

* {
  -moz-box-sizing: border-box;
  box-sizing: border-box;
  margin: 0;
  padding: 0;
  list-style: none;
  position: relative
}

html,
body {
  height: 100%;
}

body {
  background: #f06060;
  color: #fff;
}

.interactScreen, .interactContainer {
	display: flex;
	justify-content: center;
	align-items: center;
}

.interactScreen {
	width: 100%;
	height: 100%;
}

.cn-wrapper {
  font-size: 1em;
  width: 26em;
  height: 26em;
  overflow: hidden;
  z-index: 10;
  left: 50%;
  border-radius: 50%;
  margin-left: -13em;
  transform: scale(0.1);
  pointer-events: none;
  transition: all .3s ease;
}

.opened-nav {
  border-radius: 50%;
  pointer-events: auto;
  transform: scale(1);
}

.cn-overlay {
  width: 100%;
  height: 100%;
  background-color: rgba(0,0,0,0.6);
  position: fixed;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  opacity: 0;
  visibility: hidden;
  transition: all .3s ease;
  z-index: 2;
}

.cn-overlay.on-overlay {
  visibility: visible;
  opacity: 1;
}

.cn-wrapper li {
  position: absolute;
  font-size: 1.5em;
  width: 10em;
  height: 10em;
  transform-origin: 100% 100%;
  overflow: hidden;
  left: 50%;
  top: 50%;
  margin-top: -1.3em;
  margin-left: -10em;
  transition: border .3s ease;
}

.cn-wrapper li a {
  display: block;
  font-size: 1.18em;
  height: 14.5em;
  width: 14.5em;
  position: absolute;
  bottom: -7.25em;
  right: -7.25em;
  border-radius: 50%;
  text-decoration: none;
  color: #fff;
  padding-top: 1.8em;
  text-align: center;
  transform: skew(-45deg) rotate(-70deg) scale(1);
  transition: opacity 0.3s, color 0.3s;
}

.cn-wrapper li a span {
  font-size: 1.1em;
  opacity: 0.7;
}

/* for a central angle x, the list items must be skewed by 90-x degrees
in our case x=40deg so skew angle is 50deg
items should be rotated by x, minus (sum of angles - 180)2s (for this demo) */

.cn-wrapper li:first-child {
  transform: skew(45deg);
}

.cn-wrapper li:nth-child(2) {
  transform: rotate(45deg) skew(45deg);
}

.cn-wrapper li:nth-child(3) {
  transform: rotate(90deg) skew(45deg)
}

.cn-wrapper li:nth-child(4) {
  transform: rotate(135deg) skew(45deg);
}

.cn-wrapper li:nth-child(5) {
  transform: rotate(180deg) skew(45deg);
}

.cn-wrapper li:nth-child(6) {
  transform: rotate(225deg) skew(45deg);
}

.cn-wrapper li:nth-child(7) {
  transform: rotate(270deg) skew(45deg);
}

.cn-wrapper li:nth-child(8) {
  transform: rotate(315deg) skew(45deg);
}

.cn-wrapper li:nth-child(odd) a {
  background-color: hsla(0, 88%, 63%, 0.7);
}

.cn-wrapper li:nth-child(even) a {
  background-color: hsla(0, 88%, 65%, 0.7);
}

/* active style */
.cn-wrapper li.active a {
  background-color: hsla(0, 88%, 70%, 0.8);
}


/* hover style */
.cn-wrapper li:not(.active) a:hover,
.cn-wrapper li:not(.active) a:active,
.cn-wrapper li:not(.active) a:focus {
  background-color: hsla(0, 88%, 70%, 0.9);
}
.cn-wrapper li:not(.active) a:focus
{
    position:fixed;
}


@media screen and (max-width:480px) {
  .cn-wrapper {
    font-size: .68em;
  }

  .cn-wrapper li {
    font-size: 1.52em;
  }
}

</style>