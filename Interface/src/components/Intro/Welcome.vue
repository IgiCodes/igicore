<template>
	<main>
		<h1 class="display-1 mb-5">Welcome</h1>

		<section class="p-4">
			<h2 class="mb-4">Welcome to {{ serverName }}!</h2>

			<p>Lorem ipsum dolor sit amet, at equidem repudiandae eam, per cu qualisque definiebas interesset. Id dicunt vocent usu, an quod intellegam pro. Duo cu utamur tacimates theophrastus, nec no expetendis ullamcorper definitiones, odio wisi recusabo qui in. Eam te dicam oblique. Ne has ignota alterum insolens, porro menandri mei et, eam numquam epicurei ex. Diam choro platonem vis ea, tamquam assentior moderatius ad vix. Ei sit quaeque detraxit, duis fastidii intellegam et his.</p>
			<p>Ex quo quem eligendi, ex sit erant feugiat. Id ius patrioque efficiantur, pro ad omnis ridens debitis. Pri an etiam complectitur, te mel ridens periculis. No pro labitur tibique, vix eu diceret splendide. Cum animal saperet assueverit ex.</p>
			<p>Eu usu amet iuvaret feugait. Vel aliquid propriae ad, vel nulla interesset ne, mea ad blandit apeirian vituperata. Ut nam eius meis, id nam purto posse. Pri postea salutandi deterruisset an, sint partem expetenda per eu. Mea ad clita dissentias, liber dolorum luptatum ea nam, facilisi intellegat eos an. Mel cu lorem audire tacimates.</p>
			<p>Lorem ipsum dolor sit amet, at equidem repudiandae eam, per cu qualisque definiebas interesset. Id dicunt vocent usu, an quod intellegam pro. Duo cu utamur tacimates theophrastus, nec no expetendis ullamcorper definitiones, odio wisi recusabo qui in. Eam te dicam oblique. Ne has ignota alterum insolens, porro menandri mei et, eam numquam epicurei ex. Diam choro platonem vis ea, tamquam assentior moderatius ad vix. Ei sit quaeque detraxit, duis fastidii intellegam et his.</p>
			<p>Ex quo quem eligendi, ex sit erant feugiat. Id ius patrioque efficiantur, pro ad omnis ridens debitis. Pri an etiam complectitur, te mel ridens periculis. No pro labitur tibique, vix eu diceret splendide. Cum animal saperet assueverit ex.</p>
			<p>Eu usu amet iuvaret feugait. Vel aliquid propriae ad, vel nulla interesset ne, mea ad blandit apeirian vituperata. Ut nam eius meis, id nam purto posse. Pri postea salutandi deterruisset an, sint partem expetenda per eu. Mea ad clita dissentias, liber dolorum luptatum ea nam, facilisi intellegat eos an. Mel cu lorem audire tacimates.</p>
			<p>Lorem ipsum dolor sit amet, at equidem repudiandae eam, per cu qualisque definiebas interesset. Id dicunt vocent usu, an quod intellegam pro. Duo cu utamur tacimates theophrastus, nec no expetendis ullamcorper definitiones, odio wisi recusabo qui in. Eam te dicam oblique. Ne has ignota alterum insolens, porro menandri mei et, eam numquam epicurei ex. Diam choro platonem vis ea, tamquam assentior moderatius ad vix. Ei sit quaeque detraxit, duis fastidii intellegam et his.</p>
			<p>Ex quo quem eligendi, ex sit erant feugiat. Id ius patrioque efficiantur, pro ad omnis ridens debitis. Pri an etiam complectitur, te mel ridens periculis. No pro labitur tibique, vix eu diceret splendide. Cum animal saperet assueverit ex.</p>
			<p>Eu usu amet iuvaret feugait. Vel aliquid propriae ad, vel nulla interesset ne, mea ad blandit apeirian vituperata. Ut nam eius meis, id nam purto posse. Pri postea salutandi deterruisset an, sint partem expetenda per eu. Mea ad clita dissentias, liber dolorum luptatum ea nam, facilisi intellegat eos an. Mel cu lorem audire tacimates.</p>
		</section>

		<button type="button" class="btn btn-success btn-lg mt-5 float-right" @click="$emit('next')">Next <font-awesome-icon icon="chevron-right" fixed-width /></button>
	</main>
</template>

<script>
import Page from './Page'

export default {
	name: 'Welcome',
	extends: Page
}
</script>
