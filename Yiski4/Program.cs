using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using EasyLogPlus;
using Newtonsoft.Json;
using Tommy;

namespace yiski4 {
    public class Yiski4Bot {
        Logger log = new Logger(); // hi - devin
        Config cfg = new Config();

        void SetConfig() {
            cfg.ShowDate = true;
            cfg.Console = true;
            cfg.SeperateCriticalLogs = true;

            cfg.UseColon = false;

            cfg.LogPath = Environment.CurrentDirectory + $@"\Application.log";
        }
        public static Task Main(string[] args) => new Yiski4Bot().MainAsync();

        DiscordSocketClient client;
        public async Task MainAsync() {
            log.cfg = cfg;
            SetConfig();
            log.InitLogger();
            
            client = new DiscordSocketClient();
            client.Log += Log;
            client.Ready += Client_Ready;
            client.SlashCommandExecuted += SlashCommandHandler;
            
            using(StreamReader r = File.OpenText("bot.toml")){
                var table = TOML.Parse(r);
                await client.LoginAsync(TokenType.Bot, table["discord"]["token"]);
            }
            await client.StartAsync();

            await Task.Delay(-1);
        }

        async Task Client_Ready() { // it's 1 slash command, how bad can this beeeee - devin
            var slshCmd = new SlashCommandBuilder();

            slshCmd.WithName("status");
            slshCmd.WithDescription("Replies with the status of Devin's Raspberry Pi!");

            try { await client.CreateGlobalApplicationCommandAsync(slshCmd.Build()); }
            catch (HttpException exception) {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented); log.Critical(json); }
        }

        private async Task SlashCommandHandler(SocketSlashCommand cmd) {
            string[] funniFooters = { "hihi", "uwu", "owo", "wazbewwy pwi fwour?? uwu?", "-w-", "hwody fwen!", "fern!- i mean frewn!", "meep", "hihi again!", "hai!" };
            
            var sysEmbed = new EmbedBuilder {
                Title = "Devin's Raspberry Pi 4 - System",
                Color = 0x00a86b
            };
            
            var serviceEmbed = new EmbedBuilder {
                Title = "Devin's Raspberry Pi 4 - Services",
                Color = 0x00a86b
            };

            var spec = new SystemInformationHandler();
            var services = new ServicesHandler();
            var drives = new DrivesHandler();


            sysEmbed.AddField("**Raspberry Pi Hardware**", // sys
                $"**Model**: `{spec.Model}`\n" +
                $"**Processor**: `{spec.Processor()}`\n" +
                $"**Revision**: `{spec.ProcessorRevision()}`\n"+
                $"**Bootloader date**: `{spec.EEPROMDate()}`\n");
            sysEmbed.AddField("Memory Usage", 
                $"**{Math.Round(int.Parse(spec.MemUsed())/1024.0/1024,1)}**GB / **{Math.Round(int.Parse(spec.MemTotal())/1024.0/1024,1)}**GB");
            sysEmbed.AddField("Swap Usage", 
                $"**{Math.Round(int.Parse(spec.SwapUsed())/1024.0/1024,1)}**GB / **{Math.Round(int.Parse(spec.SwapTotal())/1024.0/1024,1)}**GB");
            sysEmbed.AddField("**CPU Usage**",
                $"{spec.ProcessorUsage()}");
            sysEmbed.AddField("**Storage Usage**",$"{drives.Drives()}");

            sysEmbed.AddField("Uptime", 
                $"{spec.Uptime()}");
            sysEmbed.AddField("Temperature",
                $"**{spec.Temperature()}**°C");
            sysEmbed.AddField("Distro", $"{spec.Distro()}\n" + 
                $"**64 Bit**: {Environment.Is64BitOperatingSystem}");
            sysEmbed.WithFooter($"{funniFooters[new Random().Next(0, funniFooters.Length)]}");
            sysEmbed.WithCurrentTimestamp();

            serviceEmbed.AddField("**Plex**",
                $"{services.Plex()}", inline: true);
            serviceEmbed.AddField("**Samba** [NAS]",
                $"{services.Samba()}", inline: true);
            serviceEmbed.WithFooter($"{funniFooters[new Random().Next(0, funniFooters.Length)]}");
            serviceEmbed.WithCurrentTimestamp();

            await cmd.RespondAsync(embed: sysEmbed.Build(), ephemeral: false);
            await cmd.FollowupAsync(embed: serviceEmbed.Build(), ephemeral: false); // THIS IS PROBABLY NOT THE RIGHT SOLUTION BUT OH WELL! - devin
        }

        private Task Log(LogMessage msg) {
            if (msg.Exception is CommandException cmdException) {
                log.Error($"[Command/{msg.Severity}] {cmdException.Command.Aliases.First()}" + $" failed to execute in {cmdException.Context.Channel}.");
            } else {
                switch (msg.Severity) { // look this is the only clean solution i had okay. :|
                                        // looked at this half a day later, still feels like a hack, im so fucking sorry.
                    case LogSeverity.Info:
                        log.Info($"[General] {msg}");
                        break;
                    case LogSeverity.Debug:
                        log.Debug($"[General] {msg}");
                        break;
                    case LogSeverity.Verbose:
                        log.Debug($"[General-Verbose] {msg}");
                        break;
                    case LogSeverity.Warning:
                        log.Warning($"[General] {msg}");
                        break;
                    case LogSeverity.Error:
                        log.Error($"[General] {msg}");
                        break;
                    case LogSeverity.Critical:
                        log.Critical($"[General] {msg}");
                        break;
                }
            }
            
            return Task.CompletedTask;
        }
    }
}