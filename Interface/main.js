const page = $('body');
const container = $('body > main');
const screens = {};
const elements = {};

window.post = async (url, data) => {
	// TODO: await
	fetch('http://igicore/' + url, {
		method: 'post',
		headers: {
			'Content-type': 'application/json; charset=UTF-8'
		},
		body: JSON.stringify(data)
	});
};

const loadScreen = async (cls) => {
	const instance = new cls();
	screens[instance.name] = instance;
	await screens[instance.name].load();
}

const loadElement = async (cls) => {
	const instance = new cls();
	elements[instance.name] = instance;
	await elements[instance.name].load();
}

addEventListener('message', (e) => {
	const [type, name, event] = _.split(e.data.type, ':', 3);
	const data = e.data.data;

	let fn = null;

	if (type == 'screen') {
		fn = screens[name][event];
	} else if (type == 'element') {
		fn = elements[name][event];
	}

	if (typeof fn === 'function') {
		fn(data);
	} else {
		console.log('[MESSAGE] Unhandled event "' + type + ':' + name + ':' + event + '": ' + JSON.stringify(data));
	}
});

// TODO: Get safezone from client
$(window).on('resize', () => {
	// Setup safezone
	const safezone = 0.03; // 97%

	const x = (page.width() * safezone) / 2;
	const width = page.width() - (x * 2);
	const y = (page.height() * safezone) / 2;
	const height = page.height() - (y * 2);

	container.css({
		'left': x,
		'width': width,
		'top': y,
		'height': height
	});
}).trigger('resize');


import CharacterCreationScreen from './screen/character-creation/main.js';
import DrivingElement from './element/driving/main.js';

$(async () => {
	// Screens
	await loadScreen(CharacterCreationScreen);

	// Elements
	await loadElement(DrivingElement);


	await elements['driving'].show();
});
