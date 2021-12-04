// Require the necessary discord.js classes
const fs = require('fs');
const { Client, Collection, Intents } = require('discord.js');
const dotenv = require('dotenv');

dotenv.config();

// Create a new client instance
const client = new Client({ intents: [Intents.FLAGS.GUILDS] });

client.commands = new Collection();

const commandFiles = fs.readdirSync('./commands').filter(file => file.endsWith('.js'));

for (const file of commandFiles) {
	const command = require(`./commands/${file}`);
	client.commands.set(command.data.name, command);
}

client.once('ready', () => {
	console.log('Ready!');
});

client.on('interactionCreate', async interaction => {
	if (!interaction.isCommand()) return;

	const command = client.commands.get(interaction.commandName);

	if (!command) return;

	try {
		await command.execute(interaction);
	} catch (error) {
		console.error(error);
		await interaction.reply({ content: 'There was an error while executing this command!', ephemeral: true });
	}

	// const { commandName } = interaction;

	// if (commandName === 'ping') {
	// 	await interaction.reply('Pong!');
	// } else if (commandName === 'beep') {
	// 	await interaction.reply('Boop!');
	// }
});

client.login(process.env.DISCORD_TOKEN);



// // When the client is ready, run this code (only once)
// client.once('ready', () => {
// 	console.log('Ready!');
// });

// client.on('interactionCreate', async interaction => {
// 	if (!interaction.isCommand()) return;

// 	const { commandName } = interaction;

//     switch(commandName){
//         case 'ping':
//             await interaction.reply('Meow');
//             break;
//         case 'status':
//             await interaction.reply('Uhh, not yet');
//             break;
//         default:
//             break;
//     }
// });

// // Login to Discord with your client's token
// client.login(process.env.DISCORD_TOKEN);
