using System.Diagnostics;

namespace yiski4{
    public class ServicesHandler {
        bool IsServiceRunning(string serviceName) {
            using (var process = new Process()){
                process.StartInfo.FileName = "/bin/sh";
                process.StartInfo.Arguments = $"-c \"systemctl is-active --quiet {serviceName}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                return process.ExitCode == 0;
            }
        }

        string GetServiceStatus(string serviceName) {
            return IsServiceRunning(serviceName) ? "🟩 Running" : "🟥 Stopped";
        }

        string GetServiceVersion(string serviceName) {
            using (var process = new Process()){
                process.StartInfo.FileName = "/bin/sh";
                process.StartInfo.Arguments = $"-c \"apt-cache show {serviceName} | grep 'Version'\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd().Trim();
                return output.Substring(9,output.Length-9);
            }
        }

        public string Plex() {
            return $"{GetServiceStatus("plexmediaserver")} \n`v{GetServiceVersion("plexmediaserver")}`";
        }

        public string Samba() {
            return $"{GetServiceStatus("smbd")} \n`v{GetServiceVersion("samba")}`";
        }
    }
}
