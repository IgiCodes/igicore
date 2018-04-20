import Control from '../Control.js';

export default class extends Control {
	constructor(name = '', templates = {}) {
		super(name, templates, 'element');
	}

	async load() {
		super.load();

		$('body > main').append(this.container);
	}
}
