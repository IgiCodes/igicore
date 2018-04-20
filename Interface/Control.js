export default class {
	constructor(name, templates, type) {
		this._name = name;
		this._templates = {};
		this._templateMap = templates;
		this._type = type;
		this._container = $('<div>').attr('id', this._type + '-' + this._name + '-container').hide();

		this.load = this.load.bind(this);
		this.unload = this.unload.bind(this);
		this.show = this.show.bind(this);
		this.hide = this.hide.bind(this);
	}

	get name() {
		return this._name;
	}

	get templates() {
		return this._templates;
	}

	get container() {
		return this._container;
	}

	async load() {
		await this.unload();

		for (let template in this._templateMap) {
			const resp = await $.get(this._type + '/' + this._name + '/' + this._templateMap[template]);
			this._templates[template] = _.template(resp);
		}

		$('head').append($('<link />').attr('id', this._type + '-' + this._name + '-css-main').attr('type', 'text/css').attr('rel', 'stylesheet').attr('href', this._type + '/' + this._name + '/main.css'));

		this.container.html(this.templates.container());
	}

	async unload() {
		$('head link#' + this._type + '-' + this._name + '-main').remove();

		this.container.remove();
	}

	async show() {
		this.container.show();
	}

	async hide() {
		this.container.hide();
	}
}
