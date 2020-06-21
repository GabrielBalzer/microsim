using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace microsim
{
    class Watchdog
    {
        static RegArrayHandler regArrayHandler = new RegArrayHandler();
        private static double timeNeeded = 0.018;
        public static void checkWatchdog()
        {
            timeNeeded = 0.018;
            if (DataStorage.watchdogEnabled)
            {
                //watchdog hat prescaler
                if ((regArrayHandler.getRegArray(0x81) & 0b00001000) != 0)
                {
                    timeNeeded = timeNeeded * DataStorage.prescalerValue;
                }


                if (DataStorage.timeSpent > timeNeeded)
                {
                    MainWindow.WatchdogReset();
                    Initializer.otherReset();
                }

            }
        }
    }
}
