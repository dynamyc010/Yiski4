const si = require('systeminformation');
const os = require('os-utils');

function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds){
            break;
        }
    }
}

function runCode(){
    os.cpuUsage( function(usage) { 
        
        const cpuUsage = Math.floor(usage*1000)/10;
        const memTotal = Math.floor(os.totalmem()/1000);
        const memFree = Math.floor(os.freemem()/100)/10;
        const memFreePercent = Math.floor(os.freememPercentage()*1000)/10;
        const memUsedPercent = Math.floor((100-memFreePercent)*10)/10;
        var disks = 0;
        si.fsSize().then(data => disks = JSON.parse(JSON.stringify(data))).then(() => {
            var rootfs = 0;
            var ssd = 0;
            disks.forEach(disk => {
                if(disk.mount == "/"){
                    rootfs = JSON.parse(JSON.stringify(disk));
                }
                else if (disk.mount == "/mnt/c" /* "/mnt/SSD" */){
                    ssd = JSON.parse(JSON.stringify(disk))
                }
            });

            if(!rootfs){
                console.log("Cannot find root device! (uh oh!)")
            }
            if(!ssd){
                console.log("Cannot find SSD! (oh no!)")
            }

            // Adjust
            rootfs.used = Math.floor(rootfs.used / 100000000);
            rootfs.size = Math.floor(rootfs.size / 100000000);
            ssd.used = Math.floor(ssd.used / 100000000);
            ssd.size = Math.floor(ssd.size / 100000000);

            console.log(`

            // CPU
            ${cpuUsage}% CPU usage

            // RAM
            ${memUsedPercent}% used RAM / ${memFreePercent}% free RAM
            ${Math.floor((memTotal-memFree)*10)/10}GB used / ${memTotal}GB total

            // Disk Space
            Boot storage
            ${rootfs.used/10}GB used out of ${rootfs.size/10}GB! (${rootfs.use}% full)

            SSD
            ${ssd.used/10}GB used out of ${ssd.size/10}GB! (${ssd.use}% full)
    
            ------------------------------------------------------------------------------------------
            `);
        })
    } )
}


//runCode();

si.networkStats();

setInterval(function() {
    si.networkStats().then(data => {
        console.log(data);
    })
}, 10000)
//si.fsSize().then(data => console.log(data));
//setInterval(runCode, 1000);



// data.forEach(disk => {
//     if(disk.mount == '/'){
//         rootfs = JSON.parse(JSON.stringify(disk));
//     }
//     if(disk.mount == '/mnt/c' /* '/mnt/SSD' */){
//         essd = JSON.parse(JSON.stringify(disk));
//     }
//     // Ignore the rest
// });