module.exports = {
	env: {
		browser: true,
		es6: true
	},
	parserOptions: {
		ecmaVersion: 8
	},
	extends: [
		'eslint:recommended',
		'plugin:vue/essential'
	],
	rules: {
		'vue/no-unused-vars': 'error',
		indent: ['error', 'tab'],
		quotes: ['error', 'single', 'avoid-escape'],
		'linebreak-style': ['error', 'windows'],
		semi: ['error', 'never'],
		'no-trailing-spaces': ['warn'],
		'comma-dangle': ['error'],
		'no-console': ['warn', { 'allow': ['warn', 'error', 'debug'] }],
		'no-var': ['error'],
		'prefer-const': ['error']
	}
}
