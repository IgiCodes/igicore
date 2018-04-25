module.exports = {
	processors: ['@mapbox/stylelint-processor-arbitrary-tags'],
	extends: [
		'stylelint-config-standard',
		'stylelint-config-sass-guidelines',
		'stylelint-config-recess-order'
	],
	rules: {
		indentation: 'tab',
		'order/properties-alphabetical-order': null,
		'selector-max-compound-selectors': null,
		'selector-no-qualifying-type': null,
		'no-empty-source': null
	}
}
