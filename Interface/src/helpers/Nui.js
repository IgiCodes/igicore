import store from '../store'

export default {
	async send(event, data = {}) {
		console.debug('[NUI SEND]', event, data)

		// HACK: Fake it with a delay
		if (window.emulateServer) {
			return await new Promise((resolve) => setTimeout(resolve, 100))
		}

		return await fetch('http://' + store.getters.resourceName + '/' + event, {
			method: 'post',
			headers: {
				'Content-type': 'application/json; charset=UTF-8'
			},
			body: JSON.stringify(data)
		})
	},

	emulate(type, data = null) {
		window.dispatchEvent(new MessageEvent('message', {
			data: {
				type,
				data
			}
		}))
	}
}
