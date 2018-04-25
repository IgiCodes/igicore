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
		'plugin:vue/recommended'
	],
	rules: {
		'vue/no-unused-vars': 'warn',
		'vue/html-indent': ['error', 'tab'],
		'vue/max-attributes-per-line': null,
		'vue/prop-name-casing': ['error', 'camelCase'],
		'vue/html-self-closing': ['error', { html: { void: 'always', normal: 'never', component: 'always' } }],
		'vue/script-indent': ['error', 'tab'],
		'vue/html-closing-bracket-spacing': ['error'],
		'vue/html-closing-bracket-newline': ['error'],
		indent: ['error', 'tab'],
		quotes: ['error', 'single', 'avoid-escape'],
		'linebreak-style': ['error', 'windows'],
		semi: ['error', 'never'],
		'no-trailing-spaces': ['warn'],
		'comma-dangle': ['error'],
		'no-console': ['warn', { allow: ['warn', 'error', 'debug'] }],
		'no-var': ['error'],
		'prefer-const': ['error']
	}
}
