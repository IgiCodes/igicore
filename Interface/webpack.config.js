const path = require('path')
const StyleLintPlugin = require('stylelint-webpack-plugin')
const FriendlyErrorsWebpackPlugin = require('friendly-errors-webpack-plugin')
const { VueLoaderPlugin } = require('vue-loader')

module.exports = {
	target: 'web',
	entry: './src/main.js',
	output: {
		filename: 'bundle.js',
		path: path.resolve(__dirname, 'dist')
	},
	resolve: {
		extensions: ['.js', '.vue', '.json'],
		alias: {
			'vue$': 'vue/dist/vue.esm.js'
		}
	},
	performance: {
		hints: false
	},
	plugins: [
		new VueLoaderPlugin(),
		new StyleLintPlugin({
			files: ['src/**/*.vue'],
			syntax: 'scss'
		}),
		new FriendlyErrorsWebpackPlugin()
	],
	module: {
		rules: [
			{
				enforce: 'pre',
				test: /\.(js|vue)$/,
				exclude: /node_modules/,
				loader: 'eslint-loader'
			},
			{
				test: /\.css$/,
				use: [
					'vue-style-loader',
					'css-loader'
				]
			},
			{
				test: /\.scss$/,
				use: [
					'vue-style-loader',
					'css-loader',
					'sass-loader'
				]
			},
			{
				test: /\.(woff2|png|jpg)$/,
				use: [{
					loader: 'file-loader',
					options: {
						name: '[name].[ext]'
					}
				}]
			},
			{
				test: /\.vue$/,
				loader: 'vue-loader'
			}
		]
	}
}
