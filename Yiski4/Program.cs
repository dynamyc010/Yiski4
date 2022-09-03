using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using EasyLogPlus;
using Newtonsoft.Json;

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

            var token = File.ReadAllText("token.txt");

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        async Task Client_Ready() {
            var slshCmd = new SlashCommandBuilder();

            slshCmd.WithName("status");
            slshCmd.WithDescription("Replies with the status of Devin's Raspberry Pi!");

            try { await client.CreateGlobalApplicationCommandAsync(slshCmd.Build()); }
            catch (HttpException exception) {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented); log.Critical(json); }
        }

        private async Task SlashCommandHandler(SocketSlashCommand cmd) {
            var sysEmbed = new EmbedBuilder {
                Title = "Devin's Raspberry Pi 4 - System"
            };
            
            var serviceEmbed = new EmbedBuilder {
                Title = "Devin's Raspberry Pi 4 - Services"
            };

            var rpiModel = System.IO.File.Exists(@"/proc/device-tree/model") ? System.IO.File.ReadAllText(@"/proc/device-tree/model".ToString()) : "Not a Raspberry Pi!";
            string rpiProcessor;

            // foreach (string rpiRevision in File.ReadLines(@"/proc/cpuinfo").Where(rpiRevision => rpiRevision.StartsWith("Revision    : "))) {
            //     return;
            // }
            
            sysEmbed.AddField("**Raspberry Pi Hardware**", // sys
                $"**Model**: {rpiModel}\n" +
                "**Processor**: {rpiRevision}\n" + // i took out the $ at the end because i cant figure it out please help... also we need to do terminal
                                                    // related shenanigans eventually please HELP
                "**Revision**: {`todo`}");
            sysEmbed.AddField("Memory Usage", 
                "{todo}GB / {todo}GB");
            sysEmbed.AddField("**CPU Usage**",
                "{todo}");
            sysEmbed.AddField("**Storage Usage**", 
                "{massively todo oh god}");
            sysEmbed.AddField("Uptime", 
                "{todo}");
            sysEmbed.AddField("Temperature",
                "{todo}");
            sysEmbed.AddField("Distro", "{todo}\n" +
                                        $"**64 Bit**: {Environment.Is64BitOperatingSystem}");
            sysEmbed.WithFooter("Hii!");
            sysEmbed.WithCurrentTimestamp();

            await cmd.RespondAsync(embed: sysEmbed.Build(), ephemeral: false);
        }

        private Task Log(LogMessage msg) {
            if (msg.Exception is CommandException cmdException) {
                log.Error($"[Command/{msg.Severity}] {cmdException.Command.Aliases.First()}" + $" failed to execute in {cmdException.Context.Channel}.");
            } else {
                switch (msg.Severity) { // look this is the only clean solution i had okay. :|
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