namespace Yiski4.utils {
    public class RPiSoCModelsDictionary {
        public IDictionary<Int32, String> RPiSoCModels() {
            IDictionary<Int32, String> RPiSoCModels = new Dictionary<Int32, string>();
            /* --- Start of BCM2835 Boards --- */
            // ref: https://www.raspberrypi.com/documentation/computers/raspberry-pi.html#old-style-revision-codes
            // old styled codes
            RPiSoCModels.Add(0x0002, "BCM2835"); // RPi 1B - Revision 1.0 - 256MB
            RPiSoCModels.Add(0x0003, "BCM2835"); // RPi 1B - Revision 1.0 - 256MB
            RPiSoCModels.Add(0x0004, "BCM2835"); // RPi 1B - Revision 2.0 - 256MB
            RPiSoCModels.Add(0x0005, "BCM2835"); // RPi 1B - Revision 2.0 - 256MB
            RPiSoCModels.Add(0x0006, "BCM2835"); // RPi 1B - Revision 2.0 - 256MB
            RPiSoCModels.Add(0x0007, "BCM2835"); // RPi 1A - Revision 2.0 - 256MB
            RPiSoCModels.Add(0x0008, "BCM2835"); // RPi 1A - Revision 2.0 - 256MB
            RPiSoCModels.Add(0x0009, "BCM2835"); // RPi 1A - Revision 2.0 - 256MB
            RPiSoCModels.Add(0x000d, "BCM2835"); // RPi 1B - Revision 2.0 - 512MB
            RPiSoCModels.Add(0x000e, "BCM2835"); // RPi 1B - Revision 2.0 - 512MB
            RPiSoCModels.Add(0x000f, "BCM2835"); // RPi 1B - Revision 2.0 - 512MB
            RPiSoCModels.Add(0x0010, "BCM2835"); // RPi 1B+ - Revision 1.2 - 512MB
            RPiSoCModels.Add(0x0011, "BCM2835"); // RPi CM1 - Revision 1.0 - 512MB
            RPiSoCModels.Add(0x0012, "BCM2835"); // RPi 1A+ - Revision 1.1 - 256MB
            RPiSoCModels.Add(0x0013, "BCM2835"); // RPi 1B+ - Revision 1.2 - 512MB
            RPiSoCModels.Add(0x0014, "BCM2835"); // RPi CM1 - Revision 1.0 - 512MB
            RPiSoCModels.Add(0x0015, "BCM2835"); // RPi 1A+ - Revision 1.1 - 256MB/512MB

            // ref: https://www.raspberrypi.com/documentation/computers/raspberry-pi.html#new-style-revision-codes-in-use
            // for boards that use new styled codes
            RPiSoCModels.Add(0x900021, "BCM2835"); // RPi 1A+ - Revision 1.1 - 512MB
            RPiSoCModels.Add(0x900032, "BCM2835"); // RPi 1B+ - Revision 1.2 - 512MB
            RPiSoCModels.Add(0x900092, "BCM2835"); // RPi Zero - Revision 1.2 - 512MB
            RPiSoCModels.Add(0x900093, "BCM2835"); // RPi Zero - Revision 1.3 - 512MB
            RPiSoCModels.Add(0x9000c1, "BCM2835"); // RPi Zero W - Revision 1.1 - 512MB
            RPiSoCModels.Add(0x920092, "BCM2835"); // RPi Zero - Revision 1.2 - 512MB
            RPiSoCModels.Add(0x920093, "BCM2835"); // RPi Zero - Revision 1.3 - 512MB
            RPiSoCModels.Add(0x900061, "BCM2835"); // RPi CM1 - Revision 1.1 - 512MB
            /* ---- End of BCM2835 Boards ---- */
            
            /* --- Start of BCM2836 Boards --- */
            RPiSoCModels.Add(0xa01040, "BCM2836"); // RPi 2B - Revision 1.0 - 1GB
            RPiSoCModels.Add(0xa01041, "BCM2836"); // RPi 2B - Revision 1.1 - 1GB
            RPiSoCModels.Add(0xa21041, "BCM2836"); // RPi 2B - Revision 1.1 - 1GB
            /* ---- End of BCM2836 Boards ---- */
            
            /* --- Start of BCM2837 Boards --- */
            RPiSoCModels.Add(0xa02042, "BCM2837"); // RPi 2B BCM2837 Edition - Revision 1.2 - 1GB 
            RPiSoCModels.Add(0xa22042, "BCM2837"); // RPi 2B BCM2837 Edition - Revision 1.2 - 1GB 
            RPiSoCModels.Add(0xa02082, "BCM2837"); // RPi 3B - Revision 1.2 - 1GB
            RPiSoCModels.Add(0xa22082, "BCM2837"); // RPi 3B - Revision 1.2 - 1GB
            RPiSoCModels.Add(0xa32082, "BCM2837"); // RPi 3B - Revision 1.2 - 1GB
            RPiSoCModels.Add(0xa52082, "BCM2837"); // RPi 3B - Revision 1.2 - 1GB
            RPiSoCModels.Add(0xa22083, "BCM2837"); // RPi 3B - Revision 1.3 - 1GB
            RPiSoCModels.Add(0xa020a0, "BCM2837"); // RPi CM3 - Sony UK *and* Embest - Revision 1.0 - 1GB
            /* ---- End of BCM2837 Boards ---- */
            
            /* -- Start of BCM2837B0 Boards -- */
            RPiSoCModels.Add(0x9020e0, "BCM2837B0"); // RPi 3A+ - Revision 1.0 - 1GB
            RPiSoCModels.Add(0xa020d3, "BCM2837B0"); // RPi 3B+ - Revision 1.3 - 1GB
            RPiSoCModels.Add(0xa02100, "BCM2837B0"); // RPi CM3+ - Revision 1.0 - 1GB
            /* --- End of BCM2837B0 Boards --- */

            /* --- Start of BCM2711 Boards --- */
            RPiSoCModels.Add(0xa03111, "BCM2711"); // RPi 4B - Revision 1.1 - 1GB
            RPiSoCModels.Add(0xb03111, "BCM2711"); // RPi 4B - Revision 1.1 - 2GB
            RPiSoCModels.Add(0xb03112, "BCM2711"); // RPi 4B - Revision 1.2 - 2GB
            RPiSoCModels.Add(0xb03114, "BCM2711"); // RPi 4B - Revision 1.4 - 2GB
            RPiSoCModels.Add(0xb03115, "BCM2711"); // RPi 4B - Revision 1.5 - 2GB
            RPiSoCModels.Add(0xc03111, "BCM2711"); // RPi 4B - Revision 1.1 - 4GB
            RPiSoCModels.Add(0xc03112, "BCM2711"); // RPi 4B - Revision 1.2 - 4GB
            RPiSoCModels.Add(0xc03114, "BCM2711"); // RPi 4B - Revision 1.4 - 4GB
            RPiSoCModels.Add(0xc03115, "BCM2711"); // RPi 4B - Revision 1.5 - 4GB
            RPiSoCModels.Add(0xd03114, "BCM2711"); // RPi 4B - Revision 1.4 - 8GB
            RPiSoCModels.Add(0xd03115, "BCM2711"); // RPi 4B - Revision 1.5 - 8GB
            RPiSoCModels.Add(0xc03130, "BCM2711"); // RPi 400 - Revision 1.0 - 14B
            RPiSoCModels.Add(0xa03140, "BCM2711"); // RPi CM4 - Revision 1.0 - 1GB
            RPiSoCModels.Add(0xb03140, "BCM2711"); // RPi CM4 - Revision 1.0 - 2GB
            RPiSoCModels.Add(0xc03140, "BCM2711"); // RPi CM4 - Revision 1.0 - 4GB
            RPiSoCModels.Add(0xd03140, "BCM2711"); // RPi CM4 - Revision 1.0 - 8GB
            /* ---- End of BCM2711 Boards ---- */
            
            /* ---- Start of RP3A0 Boards ---- */
            RPiSoCModels.Add(0x902120, "RP3A0"); // RPi Zero 2 W - Revision 1.0 - 512MB
            /* ----- End of RP3A0 Boards ----- */

            return RPiSoCModels;
        }
    }
}