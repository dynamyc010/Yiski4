using System.Collections.ObjectModel;
using System.Diagnostics;
using Tommy;

namespace Yiski4.handlers {
    public class ServicesHandler {
        private List<string> _serviceNames = new List<string>();
        public ReadOnlyCollection<string> serviceNames {get => _serviceNames.AsReadOnly();}

        private List<string> _serviceIDs = new List<string>();
        public ReadOnlyCollection<string> serviceIDs {get => _serviceIDs.AsReadOnly();}

        bool IsServiceRunning(string serviceName) {
            using (var process = new Process()) {
                process.StartInfo.FileName = "/bin/sh";
                process.StartInfo.Arguments = $"-c \"systemctl is-active --quiet {serviceName}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                return process.ExitCode == 0;
            }
        }

        string GetServiceStatus(string serviceName) => IsServiceRunning(serviceName) ? "🟩 Running" : "🟥 Stopped";

        string GetServiceVersion(string serviceName) {
            try {
                using (var process = new Process()) {
                    process.StartInfo.FileName = "/bin/sh";
                    process.StartInfo.Arguments = $"-c \"apt-cache show {serviceName} | grep 'Version'\"";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    process.WaitForExit();
                    var output = process.StandardOutput.ReadToEnd().Split('\n')[0].Trim();
                    return $"v{output.Substring(9, output.Length - 9)}";
                }
            }
            catch (ArgumentOutOfRangeException) { return "Unknown"; }
        }

        public ServicesHandler(){
            var services = TOML.Parse(File.OpenText("drives.toml"))["services"];
            foreach(var a in services["serviceNames"]){
                _serviceNames.Add(a.ToString());
            }
            foreach(var a in services["serviceIDs"]){
                _serviceIDs.Add(a.ToString());
            }
        }

        public string ToString(string serviceID) => $"{GetServiceStatus(serviceID)} \n`{GetServiceVersion(serviceID)}`";
        
        // public string Plex() => $"{GetServiceStatus("plexmediaserver")} \n`{GetServiceVersion("plexmediaserver")}`";

        // public string Samba() => $"{GetServiceStatus("smbd")} \n`{GetServiceVersion("samba")}`";
    }
}