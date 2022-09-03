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
                process.StartInfo.Arguments = $"-c \"apt show {serviceName} | grep 'Version' | head -n 1 | cut -d ':' -f 2\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                return process.StandardOutput.ReadToEnd().Trim();
            }
        }

        public string Plex() {
            return $"{GetServiceStatus("plexmediaserver")} \n{GetServiceVersion("plexmediaserver")}";
        }

        public string Samba() {
            return $"{GetServiceStatus("smbd")} \n{GetServiceVersion("samba")}";
        }
        
    }
}
