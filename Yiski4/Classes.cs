using System.Diagnostics;

namespace yiski4{
    public class Specifications {
        string RegisterCommand(string command) {
            using (var process = new Process()){
                process.StartInfo.FileName = "/bin/sh";
                process.StartInfo.Arguments = $"-c \"{command}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                return process.StandardOutput.ReadToEnd();
            }
        }

        public string Model = System.IO.File.Exists(@"/proc/device-tree/model") ? System.IO.File.ReadAllText(@"/proc/device-tree/model".ToString()) : "Not a Raspberry Pi!";
        public string Processor() {
            return "owo";
        }
        public string ProcessorRevision(){
            return RegisterCommand("cat /proc/cpuinfo | grep 'Revision' | head -n 1 | cut -d ':' -f 2").Trim(); 
        }

        public DateTime EEPROMDate(){
            return DateTime.Parse(RegisterCommand("vcgencmd bootloader_version | cut -d ' ' -f 2").Trim());
        }
        public string MemUsed(){
            return RegisterCommand("free | grep Mem | awk '{print $3}'").Trim();
        }
        public string MemTotal(){
            return RegisterCommand("free | grep Mem | awk '{print $2}'").Trim();
        }

        public string SwapUsed(){
            return RegisterCommand("free | grep Swap | awk '{print $3}'").Trim();
        }

        public string SwapTotal(){
            return RegisterCommand("free | grep Swap | awk '{print $2}'").Trim();
        }

        // public void GetMemory() {
        //     MemTotal = double.Parse(RegisterCommand("cat /proc/meminfo | grep 'MemTotal' | head -n 1 | cut -d ':' -f 2 | cut -d ' ' -f 2").Trim());
        //     MemFree = double.Parse(RegisterCommand("cat /proc/meminfo | grep 'MemFree' | head -n 1 | cut -d ':' -f 2 | cut -d ' ' -f 2").Trim());
        //     MemShared = double.Parse(RegisterCommand("cat /proc/meminfo | grep 'Shmem' | head -n 1 | cut -d ':' -f 2 | cut -d ' ' -f 2").Trim());
        //     MemAvailable = double.Parse(RegisterCommand("cat /proc/meminfo | grep 'MemAvailable' | head -n 1 | cut -d ':' -f 2 | cut -d ' ' -f 2").Trim());
        //     MemUsed = MemTotal - MemFree;
        // }

        // public void GetSwap() {
        //     SwapTotal = double.Parse(RegisterCommand("cat /proc/meminfo | grep 'SwapTotal' | head -n 1 | cut -d ':' -f 2 | cut -d ' ' -f 2").Trim());
        //     SwapFree = double.Parse(RegisterCommand("cat /proc/meminfo | grep 'SwapFree' | head -n 1 | cut -d ':' -f 2 | cut -d ' ' -f 2").Trim());
        //     SwapUsed = SwapTotal - SwapFree;
        // }

        public string ProcessorUsage(){
            string loadString = RegisterCommand("cat /proc/loadavg").Substring(0,14);
            double[] loadsArray = loadString.Split(" ").Select(double.Parse).ToArray();
            return $"1 min - **{Math.Round(loadsArray[0]/Environment.ProcessorCount*100,1)}**% \n"+
            $"5 min - **{Math.Round(loadsArray[1]/Environment.ProcessorCount*100,1)}**% \n"+
            $"15 min - **{Math.Round(loadsArray[2]/Environment.ProcessorCount*100,1)}**%";
        }

        public string Uptime(){
            return RegisterCommand("uptime -p").TrimStart('u', 'p', ' ');
        }
        public string Temperature(){
            return RegisterCommand("vcgencmd measure_temp").TrimStart('t', 'e', 'm', 'p', '=', '\'').TrimEnd('\'', 'C');
        }

        public string Distro(){
            return RegisterCommand("lsb_release -ds ; uname -mr");
        }
    }

    public class Services {
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
