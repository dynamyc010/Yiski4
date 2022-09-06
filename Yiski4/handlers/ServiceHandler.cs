using System.Diagnostics;

namespace Yiski4.handlers {
    public class ServicesHandler {
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
            try{
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
            catch(System.ArgumentOutOfRangeException){
                return "Unknown";
            }
        }
        
        public string Plex() => $"{GetServiceStatus("plexmediaserver")} \n`{GetServiceVersion("plexmediaserver")}`";

        public string Samba() => $"{GetServiceStatus("smbd")} \n`{GetServiceVersion("samba")}`";
    }
}