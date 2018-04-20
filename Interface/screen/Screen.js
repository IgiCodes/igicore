import Control from '../Control.js';

export default class extends Control {
	constructor(name = '', templates = {}) {
		super(name, templates, 'screen');
	}

	async load() {
		super.load();

		$('body > main').html(this.container);
	}
}
