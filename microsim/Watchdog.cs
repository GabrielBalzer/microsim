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
        public static void checkWatchdog(uint cycles)
        {
            timeNeeded = 0.018;
            if (!DataStorage.watchdogEnabled) return;
            //watchdog hat prescaler
            if ((regArrayHandler.getRegArray(0x81) & 0b00001000) != 0)
            {
                timeNeeded = timeNeeded * DataStorage.prescalerValue;
            }


            if (DataStorage.watchdogValue >= timeNeeded)
            {
                Initializer.otherReset();
                MainWindow.WatchdogReset();
            }
            DataStorage.watchdogValue = DataStorage.watchdogValue + (4.0 / ((double)DataStorage.quarzfreq)) * (double)cycles;
        }
    }
}
