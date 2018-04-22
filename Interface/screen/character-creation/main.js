import Screen from '../Screen.js';

export default class extends Screen {
	constructor() {
		super('character-creation', {
			container: 'index.html',
			character: 'character.html'
		});

		this.characters = this.characters.bind(this);
	}

	async characters(characters) {
		const cards = $('#character-cards');
		cards.find('.character-card').remove();

		for (let i = 0; i < characters.length; i++) {
			const character = characters[i];

			character.GenderString = character.Gender == 0 ? 'Male' : 'Female';

			const dob = new Date(character.DateOfBirth);
			character.DateOfBirthFormatted = dob.getDate() + ' ' + dob.toLocaleString('en-US', {month: 'long'}) + ' ' + dob.getFullYear();

			cards.prepend(this.templates.character(character));
		}

        $('form', this.container).off('submit').on('submit', async (e) => {
		    console.log("Char Create from submitted");
		    e.preventDefault();
		    $(e.target).prop('disable', true);

		    await post('character-create', objectifyForm($('form').serializeArray()));
		});

		$('.btn-load', this.container).on('click', async (e) => {
			e.preventDefault();
			$(e.target).prop('disable', true);

			await post('character-load', $(e.target).data('id'));

			this.unload();
		});

		$('.btn-delete', this.container).on('click', async (e) => {
			e.preventDefault();
			$(e.target).prop('disable', true);

			await post('character-delete', $(e.target).data('id'));
		});
	}
}
