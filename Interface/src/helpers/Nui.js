export default {
	async send(event, data = {}) {
		return await fetch('http://igicore/' + event, {
			method: 'post',
			headers: {
				'Content-type': 'application/json; charset=UTF-8'
			},
			body: JSON.stringify(data)
		})
	},

	emulate(type, data = {}) {
		window.dispatchEvent(new MessageEvent('message', {
			data: {
				type,
				data
			}
		}))
	}
}
