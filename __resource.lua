resource_manifest_version '05cfa83c-a124-4cfa-a768-c24a5811d8f9'
description 'IgiCore Framework'
version '0.0.1'

server_scripts {
	'Core.net.dll',
	'Server.net.dll',
}

client_scripts {
	'Core.net.dll',
	'Client.net.dll',
}

files {
	'System.ComponentModel.DataAnnotations.dll',
	'Newtonsoft.Json.dll',
	'EntityFramework.dll',
	'NativeUI.dll',

	'Interface/index.html',
	'Interface/main.js',
	'Interface/main.css',

	'Interface/fonts/gravity.woff2',
	'Interface/fonts/pricedown.woff2',

	'Interface/libs/jquery.min.js',
	'Interface/libs/lodash.min.js',
	'Interface/libs/bootstrap.min.css',
	'Interface/libs/bootstrap.bundle.min.js',
	'Interface/libs/fontawesome-all.min.js',

	'Interface/loading-screen/index.html',
	'Interface/loading-screen/main.js',
	'Interface/loading-screen/main.css',
	'Interface/loading-screen/img/logo.png',
	'Interface/loading-screen/img/bg1.jpg',
	'Interface/loading-screen/img/bg2.jpg',
	'Interface/loading-screen/img/bg3.jpg',
	'Interface/loading-screen/img/bg4.jpg',
	'Interface/loading-screen/img/bg5.jpg',

	'Interface/Control.js',
	'Interface/screen/Screen.js',
	'Interface/element/Element.js',

	'Interface/screen/character-creation/index.html',
	'Interface/screen/character-creation/character.html',
	'Interface/screen/character-creation/main.js',
	'Interface/screen/character-creation/main.css',

	'Interface/element/driving/index.html',
	'Interface/element/driving/main.js',
	'Interface/element/driving/main.css',
}

loadscreen 'Interface/loading-screen/index.html'
ui_page 'Interface/index.html'
