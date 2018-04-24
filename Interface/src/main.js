import Vue from 'vue'
import App from './App'
import Nui from './helpers/Nui'
import FontAwesome from '@fortawesome/fontawesome'
import FontAwesomeRegular from '@fortawesome/fontawesome-free-regular'
import FontAwesomeSolid from '@fortawesome/fontawesome-free-solid'

FontAwesome.library.add(FontAwesomeRegular, FontAwesomeSolid) // TODO: Only loaded needed icons

new Vue({
	el: 'main',
	render: h => h(App)
})

setTimeout(() => {
	Nui.emulate('test', {
		test: true,
		other: 1
	})
}, 2000)
