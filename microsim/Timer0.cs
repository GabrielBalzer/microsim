using System;

namespace microsim
{
    public class Timer0
    {
        RegArrayHandler regArrayHandler = new RegArrayHandler();
        public void timerCount(uint cycles)
        {
            RegArrayHandler regArrayHandler = new RegArrayHandler();
            if ((regArrayHandler.getRegArray(0x81) & 0b00001000) == 0)
            {
                if ((regArrayHandler.getRegArray(0x81) & 0b00100000) == 0)
                {
                    Console.WriteLine("Timer mit Befehlstakt erhöht!");
                    if ((DataStorage.prescalerCount + cycles ) >= DataStorage.prescalerValue)
                    {
                        if (regArrayHandler.getRegArray(0x01) == 255)
                        {
                            regArrayHandler.setRegArray(0x0B, regArrayHandler.getRegArray(0x0B) | 0b00000100);
                            //triggerInterrupt();
                            regArrayHandler.setRegArray(0x01,0);
                            DataStorage.prescalerCount = 0;
                        }
                        else
                        {
                            regArrayHandler.setRegArray(0x01, (regArrayHandler.getRegArray(0x01) + 1));
                            DataStorage.prescalerCount = 0;
                        }
                    }
                    else
                    {
                        DataStorage.prescalerCount = DataStorage.prescalerCount + (int) cycles;
                    }
                }
                else
                {
                    if (isRightFlank())
                    {
                        Console.WriteLine("Flanke erkannt!");
                        if ((DataStorage.prescalerCount + cycles) >= DataStorage.prescalerValue)
                        {
                            if (regArrayHandler.getRegArray(0x01) == 255)
                            {
                                regArrayHandler.setRegArray(0x0B, regArrayHandler.getRegArray(0x0B) | 0b00000100);
                                //triggerInterrupt();
                                regArrayHandler.setRegArray(0x01, 0);
                                DataStorage.prescalerCount = 0;
                            }
                            else
                            {
                                regArrayHandler.setRegArray(0x01, (regArrayHandler.getRegArray(0x01) + 1));
                                DataStorage.prescalerCount = 0;
                            }
                        }
                        else
                        {
                            DataStorage.prescalerCount = DataStorage.prescalerCount + (int)cycles;
                        }
                    }
                }
            }
            else
            {
                if ((regArrayHandler.getRegArray(0x81) & 0b00100000) == 0)
                {
                    Console.WriteLine("Timer mit Befehlstakt erhöht!");
 
                        if (regArrayHandler.getRegArray(0x01) == 255)
                        {
                            regArrayHandler.setRegArray(0x0B, regArrayHandler.getRegArray(0x0B) | 0b00000100);
                            //triggerInterrupt();
                            regArrayHandler.setRegArray(0x01, 0);
                        }
                        else
                        {
                        regArrayHandler.setRegArray(0x01, (regArrayHandler.getRegArray(0x01) + 1));
                    }
                }
                else
                {
                    if (isRightFlank())
                    {
                        Console.WriteLine("Flanke erkannt!");
                        if (regArrayHandler.getRegArray(0x01) == 255)
                        {
                            regArrayHandler.setRegArray(0x0B, regArrayHandler.getRegArray(0x0B) | 0b00000100);
                            //triggerInterrupt();
                            regArrayHandler.setRegArray(0x01, 0);
                        }
                        else
                        {
                            regArrayHandler.setRegArray(0x01, (regArrayHandler.getRegArray(0x01) + 1));
                        }
                    }
                }
            }
        }

        public bool isRightFlank()
        {
            bool flank;
            if ((((regArrayHandler.getRegArray(0x081) & 0b00010000) > 0) && DataStorage.highLowFlank))
            {
                flank = true;
                DataStorage.highLowFlank = false;
            }
            else if ((((regArrayHandler.getRegArray(0x081) & 0b00010000) == 0) && DataStorage.lowHighFlank))
            {
                flank = true;
                DataStorage.lowHighFlank = false;
            }
            else
            {
                flank = false;
            }
            return flank;
        }
    }
}