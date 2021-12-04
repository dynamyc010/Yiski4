const fs = require('fs');
const { SlashCommandBuilder } = require('@discordjs/builders');
const { REST } = require('@discordjs/rest');
const { Routes } = require('discord-api-types/v9');
const { clientId, guildId } = require('./config.json');
const dotenv = require('dotenv');

dotenv.config();

// const commands = [
//     new SlashCommandBuilder()
//         .setName('status')
//         .setDescription('Replies with the status of Devin\'s Raspberry Pi!'),
//     new SlashCommandBuilder()
//         .setName('ping')
//         .setDescription('Pong!')
// ]
// 	.map(command => command.toJSON());

const commands= [];

const commandFiles = fs.readdirSync('./commands').filter(file => file.endsWith('.js'));

for (const file of commandFiles) {
	const command = require(`./commands/${file}`);
	commands.push(command.data.toJSON());
}

const rest = new REST({ version: '9' }).setToken(process.env.DISCORD_TOKEN);

rest.put(Routes.applicationGuildCommands(clientId, guildId), { body: commands })
	.then(() => console.log('Successfully registered application commands.'))
	.catch(console.error);
