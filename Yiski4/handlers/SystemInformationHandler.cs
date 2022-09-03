using System.Diagnostics;

namespace yiski4{
    public class SystemInformationHandler {
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
            string revision = RegisterCommand("cat /proc/cpuinfo | grep 'Revision' | head -n 1 | cut -d ':' -f 2").Trim();
            return revision != "" ? revision : "Unknown"; 
        }

        public string EEPROMDate(){
            if(System.IO.File.Exists(@"/usr/bin/vcgencmd")){
                string timeCommand = RegisterCommand("vcgencmd bootloader_version | grep 'timestamp' | cut -d ' ' -f 2").Trim();
                return DateTime.Parse(timeCommand).ToShortDateString();
            }else{
                return "Unknown";
            }
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
            try{
                return RegisterCommand("vcgencmd measure_temp").Substring(5,4);
            }
            catch(System.ArgumentOutOfRangeException){
                return "Unknown";
            }
        }

        public string Distro(){
            return RegisterCommand("lsb_release -ds ; uname -mr");
        }
    }
}