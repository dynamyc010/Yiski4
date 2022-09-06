using System.Diagnostics;
using System.Globalization;
using Yiski4.utils;

namespace Yiski4.handlers {
    public class SystemInformationHandler {
        public string Model = File.Exists(@"/proc/device-tree/model") ? File.ReadAllText(@"/proc/device-tree/model") : "Not a Raspberry Pi!";
        string RegisterCommand(string command) {
            using (var process = new Process()) {
                process.StartInfo.FileName = "/bin/sh";
                process.StartInfo.Arguments = $"-c \"{command}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                return process.StandardOutput.ReadToEnd();
            }
        }
        public string Processor() {
            var models = new RPiSoCModelsDictionary().RPiSoCModels();
            var revisionString = ProcessorRevision();
            if (revisionString == "Unknown") { return "Unknown"; }

            try {
                var revision = int.Parse(revisionString, NumberStyles.HexNumber);
                if (!models.ContainsKey(revision)) { return "Unknown"; }

                return models.Where(x => x.Key == revision).First().Value;
            }
            catch (FormatException err) {
                Yiski4Bot.log.Error(err);
                return "An error occured, please check Yiski4's logs.";
            }
        }

        public string ProcessorRevision() {
            var revision = RegisterCommand("cat /proc/cpuinfo | grep 'Revision' | head -n 1 | cut -d ':' -f 2").Trim();
            return revision != "" ? revision : "Unknown";
        }

        public string EEPROMDate() {
            if (File.Exists(@"/usr/bin/vcgencmd")) {
                var timeCommand = RegisterCommand("vcgencmd bootloader_version | grep 'timestamp' | cut -d ' ' -f 2").Trim();
                var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timeCommand));
                return dateTimeOffset.ToString();
            }

            return "Unknown";
        }
        public string MemUsed() => RegisterCommand("free | grep Mem | awk '{print $3}'").Trim();
        public string MemTotal() => RegisterCommand("free | grep Mem | awk '{print $2}'").Trim();

        public string SwapUsed() => RegisterCommand("free | grep Swap | awk '{print $3}'").Trim();

        public string SwapTotal() => RegisterCommand("free | grep Swap | awk '{print $2}'").Trim();

        public string ProcessorUsage() {
            var loadString = RegisterCommand("cat /proc/loadavg").Substring(0, 14);
            var loadsArray = loadString.Split(" ").Select(double.Parse).ToArray();
            return $"1 min - **{Math.Round(loadsArray[0] / Environment.ProcessorCount * 100, 1)}**% \n" +
                   $"5 min - **{Math.Round(loadsArray[1] / Environment.ProcessorCount * 100, 1)}**% \n" +
                   $"15 min - **{Math.Round(loadsArray[2] / Environment.ProcessorCount * 100, 1)}**%";
        }

        public string Uptime() => RegisterCommand("uptime -p").TrimStart('u', 'p', ' ');
        public string Temperature() {
            try { return RegisterCommand("vcgencmd measure_temp").Substring(5, 4); }
            catch (ArgumentOutOfRangeException) { return "Unknown"; }
        }

        public string Distro() => RegisterCommand("lsb_release -ds ; uname -mr");
    }
}