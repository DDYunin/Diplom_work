using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBLE.BLE.EventArgsClasses
{
    public class ReadyBLEDataEventArgs : EventArgs
    {
        public float[] Data;
    }
}
