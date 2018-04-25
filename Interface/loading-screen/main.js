// MESSAGE
const messageElement = document.getElementById('message');
const messageSpeed = 1000 * 3; // 3 seconds
const messages = [
	'Adding Hidden Agendas',
	'Adjusting Bell Curves',
	'Aesthesizing Industrial Areas',
	'Aligning Covariance Matrices',
	'Applying Feng Shui Shaders',
	'Applying Theatre Soda Layer',
	'Asserting Packed Exemplars',
	'Attempting to Lock Back-Buffer',
	'Binding Sapling Root System',
	'Breeding Fauna',
	'Building Data Trees',
	'Bureacritizing Bureaucracies',
	'Calculating Inverse Probability Matrices',
	'Calculating Llama Expectoration Trajectory',
	'Calibrating Blue Skies',
	'Charging Ozone Layer',
	'Coalescing Cloud Formations',
	'Cohorting Exemplars',
	'Collecting Meteor Particles',
	'Compounding Inert Tessellations',
	'Compressing Fish Files',
	'Computing Optimal Bin Packing',
	'Concatenating Sub-Contractors',
	'Containing Existential Buffer',
	'Debarking Ark Ramp',
	'Debunching Unionized Commercial Services',
	'Deciding What Message to Display Next',
	'Decomposing Singular Values',
	'Decrementing Tectonic Plates',
	'Deleting Ferry Routes',
	'Depixelating Inner Mountain Surface Back Faces',
	'Depositing Slush Funds',
	'Destabilizing Economic Indicators',
	'Determining Width of Blast Fronts',
	'Deunionizing Bulldozers',
	'Dicing Models',
	'Diluting Livestock Nutrition Variables',
	'Downloading Satellite Terrain Data',
	'Exposing Flash Variables to Streak System',
	'Extracting Resources',
	'Factoring Pay Scale',
	'Fixing Election Outcome Matrix',
	'Flood-Filling Ground Water',
	'Flushing Pipe Network',
	'Gathering Particle Sources',
	'Generating Jobs',
	'Gesticulating Mimes',
	'Graphing Whale Migration',
	'Hiding Willio Webnet Mask',
	'Implementing Impeachment Routine',
	'Increasing Accuracy of RCI Simulators',
	'Increasing Magmafacation',
	'Initializing Rhinoceros Breeding Timetable',
	'Initializing Robotic Click-Path AI',
	'Inserting Sublimated Messages',
	'Integrating Curves',
	'Integrating Illumination Form Factors',
	'Integrating Population Graphs',
	'Iterating Cellular Automata',
	'Lecturing Errant Subsystems',
	'Mixing Genetic Pool',
	'Modeling Object Components',
	'Mopping Occupant Leaks',
	'Normalizing Power',
	'Obfuscating Quigley Matrix',
	'Overconstraining Dirty Industry Calculations',
	'Partitioning City Grid Singularities',
	'Perturbing Matrices',
	'Pixalating Nude Patch',
	'Polishing Water Highlights',
	'Populating Lot Templates',
	'Preparing Sprites for Random Walks',
	'Prioritizing Landmarks',
	'Projecting Law Enforcement Pastry Intake',
	'Realigning Alternate Time Frames',
	'Reconfiguring User Mental Processes',
	'Relaxing Splines',
	'Removing Road Network Speed Bumps',
	'Removing Texture Gradients',
	'Removing Vehicle Avoidance Behavior',
	'Resolving GUID Conflict',
	'Reticulating Splines',
	'Retracting Phong Shader',
	'Retrieving from Back Store',
	'Reverse Engineering Image Consultant',
	'Routing Neural Network Infanstructure',
	'Scattering Rhino Food Sources',
	'Scrubbing Terrain',
	'Searching for Llamas',
	'Seeding Architecture Simulation Parameters',
	'Sequencing Particles',
	'Setting Advisor Moods',
	'Setting Inner Deity Indicators',
	'Setting Universal Physical Constants',
	'Sonically Enhancing Occupant-Free Timber',
	'Speculating Stock Market Indices',
	'Splatting Transforms',
	'Stratifying Ground Layers',
	'Sub-Sampling Water Data',
	'Synthesizing Gravity',
	'Synthesizing Wavelets',
	'Time-Compressing Simulator Clock',
	'Unable to Reveal Current Activity',
	'Weathering Buildings',
	'Zeroing Crime Network'
];

let messageIndex = Math.floor(Math.random() * messages.length);
messageElement.innerHTML = messages[messageIndex];

setInterval(() => {
	messageElement.style.opacity = 0;

	messageIndex = (messageIndex + 1) % messages.length;

	setTimeout(() => {
		messageElement.innerHTML = messages[messageIndex];
		messageElement.style.opacity = 1;
	}, 250);
}, messageSpeed);


// PROGRESS
const states = {};
const types = [
	'INIT_CORE',
	'INIT_BEFORE_MAP_LOADED',
	'MAP',
	'INIT_AFTER_MAP_LOADED',
	'INIT_SESSION'
];
const handlers = {
	startInitFunction(data) {
		if (states[data.type] == null) {
			states[data.type] = {};
			states[data.type].count = 0;
			states[data.type].done = 0;
		}
	},

	startInitFunctionOrder(data) {
		if (states[data.type] != null) {
			states[data.type].count += data.count;
		}
	},

	initFunctionInvoked(data) {
		if (states[data.type] != null) {
			states[data.type].done++;
		}
	},

	startDataFileEntries(data) {
		states['MAP'] = {};
		states['MAP'].count = data.count;
		states['MAP'].done = 0;
	},

	performMapLoadFunction(data) {
		states['MAP'].done++;
	}
};

function GetTypeProgress(type) {
	if (states[type] == null) return 0;
	if (states[type].done < 1 || states[type].count < 1) return 0;

	return (states[type].done / states[type].count) * 100;
}

window.addEventListener('message', (e) => (handlers[e.data.eventName] || (() => {}))(e.data));

setInterval(() => {
	let progress = 0;
	let states = 0;

	for (let i = 1; i < types.length; i++) { // Skip 'INIT_CORE'
		progress += GetTypeProgress(types[i]);
		states++;
	}

	const total = Math.round(progress / states);

	document.getElementById('progress-bar-value').innerHTML = total;
	document.getElementById('progress').value = total;
}, 250);


// BACKGROUND
const backgroundElement = document.getElementById('background');
const backgroundSpeed = 1000 * 5; // 5 seconds
const backgrounds = [
	'img/bg1.jpg',
	'img/bg2.jpg',
	'img/bg3.jpg',
	'img/bg4.jpg',
	'img/bg5.jpg'
];

let backgroundIndex = 0;

setInterval(() => {
	backgroundElement.style.opacity = 0;

	backgroundIndex = (backgroundIndex + 1) % backgrounds.length;

	setTimeout(() => {
		backgroundElement.setAttribute('src', backgrounds[backgroundIndex]);
		backgroundElement.style.opacity = 0.5;
	}, 500);
}, backgroundSpeed);
