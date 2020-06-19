using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    class Interrupts
    {
        public RegArrayHandler regArrayHandler = new RegArrayHandler();
        public PCL Pcl = new PCL();
        public void checkInterrupt()
        {
            if ((regArrayHandler.getRegArray(0x0B) & 0x80) == 0x80)
            {
                //Timer
                if (((regArrayHandler.getRegArray(0x0B) & 0x20) == 0x20) && ((regArrayHandler.getRegArray(0x0B) & 0x04) == 0x04))
                {
                    DataStorage.stack1.SetValueToStck(Pcl.getPCL());
                    Pcl.setPCL(0x04 - 1);
                    regArrayHandler.setRegArray(0x0B, (regArrayHandler.getRegArray(0x0B) & 0b01111111));
                }
                //RB0
                if (((regArrayHandler.getRegArray(0x0B) & 0x10) == 0x10) && ((regArrayHandler.getRegArray(0x0B) & 0x02) == 0x02))
                {
                    DataStorage.stack1.SetValueToStck(Pcl.getPCL());
                    Pcl.setPCL(0x04 - 1);
                    regArrayHandler.setRegArray(0x0B, (regArrayHandler.getRegArray(0x0B) & 0b01111111));
                }
                if (((regArrayHandler.getRegArray(0x0B) & 0x08) == 0x08) && ((regArrayHandler.getRegArray(0x0B) & 0x01) == 0x01))
                {
                    DataStorage.stack1.SetValueToStck(Pcl.getPCL());
                    Pcl.setPCL(0x04 - 1);
                    regArrayHandler.setRegArray(0x0B, (regArrayHandler.getRegArray(0x0B) & 0b01111111));
                }
            }
        }
    }
}
