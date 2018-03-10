resource_manifest_version "05cfa83c-a124-4cfa-a768-c24a5811d8f9"
description "IgiCore - Player Stuff"
version "0.0.1"

files {
	"System.ComponentModel.DataAnnotations.dll",
	"Newtonsoft.Json.dll",
	"EntityFramework.dll",
}

server_scripts {
	"Core.net.dll",
	"Server.net.dll",
}

client_scripts {
	"Core.net.dll",
	"Client.net.dll"
}