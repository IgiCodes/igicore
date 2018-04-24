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

	'Interface/dist/index.html',
	'Interface/dist/bundle.js',
	'Interface/dist/gravity.woff2',
	'Interface/dist/pricedown.woff2',
}

ui_page 'Interface/dist/index.html'
