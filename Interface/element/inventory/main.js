import Element from '../Element.js';

export default class extends Element {
    constructor() {
        super('inventory', {
            container: 'index.html'
        });
    }

    async load() {
    	console.log("inv load()");
        await super.load();

        window.onkeyup = async (e) => {
        	const key = e.keyCode;

        	console.log("Key pressed: " + key);

        	if (key == 40) await post('inventory-hide', {});
        };
    }
}
