using Tommy;

namespace yiski4{
    //////////////////////////////////////////////////////////
    // Drives                                               //
    // Reads drives from config and write them to the embed //
    //////////////////////////////////////////////////////////
    // Note: I hate my life lmao
    public class DrivesHandler {
        public string Drives(){
            DriveInfo[] mounts = DriveInfo.GetDrives();
            string drives = "";

            // TODO: Move this off to a config to read from
            List<string> targetNames = new List<string>();
            List<string> targetMounts = new List<string>();
            using(StreamReader r = File.OpenText("drives.toml")){
                TomlTable table = TOML.Parse(r);

                foreach(var a in table["drives"]["targetNames"]){
                    targetNames.Add(a.ToString());
                }
                foreach(var a in table["drives"]["targetMounts"]){
                    targetMounts.Add(a.ToString());
                }
            }

            foreach(var drive in mounts.Where(x => targetMounts.Contains(x.RootDirectory.ToString()))){
                int location = targetMounts.FindIndex(y => y == drive.RootDirectory.ToString());
                string fancyName = "";
                if(targetNames[location] != "") {fancyName = $" - **{targetNames[location]}**";}

                drives = $"{drives}**{drive.Name}**{fancyName}\n**{Math.Round((drive.TotalSize-drive.AvailableFreeSpace)/1024.0/1024/1024,1)}**GB / **{Math.Round(drive.TotalSize/1024.0/1024/1024,1)}**GB\n"+
                    $"**{Math.Round(drive.AvailableFreeSpace/1024.0/1024/1024,1)}**GB remains\n";
                }
            return drives;
        }
    }
}