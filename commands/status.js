const { SlashCommandBuilder } = require('@discordjs/builders');
const { Embed } = require('discord.js');
const si = require('systeminformation');
const os = require('os-utils');
const diskcfg = require('../disk-config.json');

module.exports = {
	data: new SlashCommandBuilder()
		.setName('status')
		.setDescription('Replies with the status of Devin\'s Raspberry Pi!'),
	async execute(interaction) {
        var cpuUsage;
        var memTotal;
        var memFree;
        await os.cpuUsage(usage => {
            cpuUsage = Math.floor(usage*1000)/10;
            memTotal = Math.floor(os.totalmem()/1000);
            memFree = Math.floor(os.freemem()/100)/10;
        })
        const osInfo = si.osInfo();
        const cpu = si.cpu();
        const uptime = si.time().uptime
        const cpuTemp = si.cpuTemperature();
        var ut_sec = uptime;
        var ut_min = ut_sec/60;
        var ut_hour = ut_min/60;
        var ut_day = ut_hour/24;
        ut_sec = Math.floor(ut_sec)%60;
        ut_min = Math.floor(ut_min)%60;
        ut_hour = Math.floor(ut_hour)%60;
        ut_day = Math.floor(ut_day)%24;
        await si.fsSize().then(data => disks = JSON.parse(JSON.stringify(data))).then(() => {
            disks.forEach(disk =>{
                diskcfg.disks.forEach(a =>{
                    if(a.mountpoint == disk.mount){
                        a.used = disk.used / 100000000;
                        a.size = disk.size  / 100000000;
                    }
                })
            })
        });
        
        var embed = {
            color: 0x00a86b,
            title: 'Devin\'s Raspbery Pi',
            fields: [
                {
                    name: `**Raspberry Pi Hardware**`,
                    value:  `**Model**: ${await(osInfo).model}\n` +
                                `**Processor**: ${await(cpu).brand}\n` +
                                `**Revision**: \`${await(osInfo).version}\``
                            },
                {
                    name: '**CPU Usage**',
                    value: `**${cpuUsage}**%`,
                },
                {
                    name: '**Memory Usage**',
                    value: `**${Math.floor((memTotal-memFree)*10)/10}**GB / **${memTotal}**GB`,
                },
                {
                    name: `**Storage Usage**`,
                    value: ``,
                },
                {
                    name: `**Uptime**`,
                    value: `**${ut_day}** days, **${ut_hour}** hours **${ut_min}** minutes **${ut_sec}** seconds`,
                },
                {
                    name: `**Temparture**`,
                    value: `**${cpuTemp.main}**°C`
                },
                {
                    name: `**Distro Information**`,
                    value: `${await(osInfo).distro} ${await(osInfo).release} ${await(osInfo).codename}`
                }
            ],
            timestamp: new Date(),
            footer: {
                text: 'Some footer text here',
                icon_url: 'https://i.imgur.com/AfFp7pu.png',
            },
        };

        await diskcfg.disks.forEach(disk => {
            embed.fields[3].value += `**${disk.name}** (${disk.mountpoint})\n`+
                                                    `**${Math.floor(disk.used)/10}**GB / **${Math.floor(disk.size)/10}**GB\n`+
                                                    `**${(Math.floor(disk.size-disk.used))/10}**GB remains\n`;
        })

        await console.log(embed);

		await interaction.reply({embeds : [embed]});
	},
};
