using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    class Timer0
    {
        // TMR0-Register

        // variables for TMR0
        public static uint[] timer0 = new uint[8];
        public static List<PrescalerTimer0> prescTimer0 = new List<PrescalerTimer0>(8);

        public class PrescalerTimer0
        {
            private uint Timer0Value;
            private uint prescalerValue;
        }

        public Timer0()
        {
            InitTimer0(); // init timer0 
            SetInterrupt(); // enable interrupt setting
            SelectClockSource(0); // enable clock source for timer0
        }

        private void InitTimer0()
        {
            //Console.WriteLine("HIER HALLO: " + DataStorage.regArray[0x81]);

            // clear T0CS in OPTION[5] bit
            DataStorage.regArray[0x81] = DataStorage.regArray[0x81] ^ 0x20; // 32 as dec

            // reset prescaler to 1:2 rate
            DataStorage.regArray[0x81] = DataStorage.regArray[0x81] & 0xF8;

            // reset timer0
            ResetTimer();

            // enable interrupts T0IE
            DataStorage.regArray[0x0B] = DataStorage.regArray[0x0B] | 0x20; //  32 as dec
            DataStorage.regArray[0x8B] = DataStorage.regArray[0x8B] | 0x20; //  32 as dec

            // enable global interrupt GIE
            DataStorage.regArray[0x0B] = DataStorage.regArray[0x0B] | 0x80; // 128 as dec
            DataStorage.regArray[0x8B] = DataStorage.regArray[0x8B] | 0x80; // 128 as dec

            //Console.WriteLine("HIER HALLO: " + DataStorage.regArray[0x81]);
        }

        public void SelectClockSource(uint newSource)
        {
            // set clock source depending on parameter "newSource"
            switch (newSource)
            {
                case 0: // PSA is set for Timer0 (= 0)
                    DataStorage.regArray[0x81] ^= 0x08;
                    Console.WriteLine("PSA IS NOW SET FOR TIMER 0");
                    break;
                case 1: // PSA is set for Watchdog (= 1)
                    DataStorage.regArray[0x81] |= 0x08;
                    Console.WriteLine("PSA IS NOW SET FOR WATCHDOG");
                    break;
                default:
                    Console.WriteLine("PSA NOT SET CORRECTLY");
                    break;
            }
        }

        private void ResetPrescaler()
        {
            //
        }

        private void SetPrescaler(uint newPrescaler)
        {
            //
            switch (newPrescaler)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }

        public void WriteTimer0()
        {
            //
        }

        public void ReadTimer0()
        {
            //
        }

        private void SetInterrupt()
        {
            uint localT0IF = 0x04;
            // if T0IF is set
            if (((DataStorage.regArray[0x0B] & localT0IF) == localT0IF) || ((DataStorage.regArray[0x8B] & localT0IF) == localT0IF))
            {
                // do something e.g. toggle RB0
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] ^ 0x10;

                // clear the interrupt
                ResetInterrupt();

                Console.WriteLine("INTERRUPT WAS FOUND AND IS RESET!");
                return;
            }
        
            Console.WriteLine("INTERRUPT WAS SET TO BE ACTIVE");
            return;
        }

        private void ResetInterrupt()
        {
            // reset T01F
            uint localT0IF = 0x04;
            DataStorage.regArray[0x0B] = DataStorage.regArray[0x0B] ^ localT0IF;
            DataStorage.regArray[0x8B] = DataStorage.regArray[0x8B] ^ localT0IF;
        }

        private void ResetTimer()
        {
            // reset array to zero
            timer0 = new uint[8];

            Console.WriteLine("TIMER 0 IS RESET");
        }
    }
}
