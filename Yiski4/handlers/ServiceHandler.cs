using System.Diagnostics;

namespace yiski4 {
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
            using (var process = new Process()) {
                process.StartInfo.FileName = "/bin/sh";
                process.StartInfo.Arguments = $"-c \"apt-cache show {serviceName} | grep 'Version'\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                var output = process.StandardOutput.ReadToEnd().Trim();
                return output.Substring(9, output.Length - 9);
            }
        }

        public string Plex() => $"{GetServiceStatus("plexmediaserver")} \n`v{GetServiceVersion("plexmediaserver")}`";

        public string Samba() => $"{GetServiceStatus("smbd")} \n`v{GetServiceVersion("samba")}`";
    }
}