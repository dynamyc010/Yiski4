const { SlashCommandBuilder } = require('@discordjs/builders');
const { Embed } = require('discord.js');
const si = require('systeminformation');
const diskcfg = require('../disk-config.json');

request = {
    system: 'model, version',
    osInfo: 'distro, release, codename',
    mem: 'free, total',
    cpu:'brand',
    cpuTemperature: 'main',
    currentLoad: 'currentLoad'
}

module.exports = {
	data: new SlashCommandBuilder()
		.setName('status')
		.setDescription('Replies with the status of Devin\'s Raspberry Pi!'),
	async execute(interaction) {
        await interaction.deferReply();
        const uptime = si.time().uptime
        var ut_sec = uptime;
        var ut_min = ut_sec/60;
        var ut_hour = ut_min/60;
        var ut_day = Math.floor(ut_hour/24);
        ut_sec = Math.floor(ut_sec)%60;
        ut_min = Math.floor(ut_min)%60;
        ut_hour = Math.floor(ut_hour)%60;
        await si.fsSize().then(x => disks = JSON.parse(JSON.stringify(x))).then(() => {
            disks.forEach(disk =>{
                diskcfg.disks.forEach(a =>{
                    if(a.mountpoint == disk.mount){
                        a.used = disk.used / 107000000;
                        a.size = disk.size  / 107000000;
                    }
                })
            })
        });
        var embed;
        await si.get(request).then(data => {
            embed = {
                color: 0x00a86b,
                title: 'Devin\'s Raspbery Pi',
                fields: [
                    {
                        name: `**Raspberry Pi Hardware**`,
                        value:  `**Model**: ${data.system.model}\n` +
                                    `**Processor**: ${data.cpu.brand}\n` +
                                    `**Revision**: \`${data.system.version}\``
                                },
                    {
                        name: '**CPU Usage**',
                        value: `**${Math.floor(data.currentLoad.currentLoad*100)/100}**%`,
                    },
                    {
                        name: '**Memory Usage**',
                        value: `**${Math.floor(data.mem.free/107000000)/10}**GB / **${Math.floor(data.mem.total/107000000)/10}**GB`,
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
                        value: `**${data.cpuTemperature.main}**°C`
                    },
                    {
                        name: `**Distro Information**`,
                        value: `${data.osInfo.distro} ${data.osInfo.release} ${data.osInfo.codename}`
                    }
                ],
                timestamp: new Date(),
                footer: {
                    text: 'Hii!'
                },
            };
            diskcfg.disks.forEach(disk => {
                embed.fields[3].value += `**${disk.name}** (${disk.mountpoint})\n`+
                                                        `**${Math.floor(disk.used)/10}**GB / **${Math.floor(disk.size)/10}**GB\n`+
                                                        `**${(Math.floor((disk.size-disk.used)))/10}**GB remains\n`;
            })
        })
        await interaction.editReply({embeds : [embed]});
	},
};
