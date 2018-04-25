import Vue from 'vue'
import Store from './Store'
import App from './App'
import Nui from './helpers/Nui'
import FontAwesome from '@fortawesome/fontawesome'
import FontAwesomeRegular from '@fortawesome/fontawesome-free-regular'
import FontAwesomeSolid from '@fortawesome/fontawesome-free-solid'

FontAwesome.library.add(FontAwesomeRegular, FontAwesomeSolid) // TODO: Only loaded needed icons

new Vue({
	el: 'main',
	store: Store,
	render: h => h(App)
})


// Emulate server NUI messages with a slight delay
// Terrible hack to skip emulation if running inside FiveM
if (!navigator.appVersion.includes('66.0.3359.0')) {
	Nui.emulate('screen:character-creation:characters', [{
		Inventory: null,
		Style: {
			Id: '851fd152-bf7c-49e2-b1d3-e78d6f1a5933',
			Face: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Head: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Hair: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Torso: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Torso2: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Legs: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Hands: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Shoes: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Special1: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Special2: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Special3: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Textures: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Hat: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Glasses: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			EarPiece: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Unknown3: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Unknown4: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Unknown5: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Watch: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Wristband: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Unknown8: {
				Type: 0,
				Index: 0,
				Texture: 0
			},
			Unknown9: {
				Type: 0,
				Index: 0,
				Texture: 0
			}
		},
		Id: '9aac1f6c-1c78-40cb-86f5-4c451cacd12a',
		Forename: 'John',
		Middlename: '',
		Surname: 'Smith',
		DateOfBirth: '1999-01-01T00:00:00',
		Gender: 0,
		Alive: true,
		Health: 10000,
		Armor: 0,
		Ssn: '123-45-6789',
		PosX: -1038.12,
		PosY: -2738.28,
		PosZ: 20.1693,
		Model: null,
		WalkingStyle: null,
		LastPlayed: '0001-01-01T00:00:00',
		Created: '2018-04-12T00:38:59'
	}])

	setTimeout(() => {
		Nui.emulate('screen:character-creation:show', null)
	}, 1000)
}
