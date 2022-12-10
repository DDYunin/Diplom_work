using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestBLE.BLE;
using Windows.Devices.Bluetooth.Advertisement;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var watcher = new ScannerOfDevices();

            watcher.StartedListening += () =>
            {
                Console.WriteLine("Started Listening");
            };

            watcher.StoppedListening += () =>
            {
                Console.WriteLine("Stopped Listening");
            };

            watcher.NewDeviceDiscovered += (device) =>
            {
                Console.WriteLine($"New Device: {device}");
            };

            watcher.DeviceNameChanged += (device) =>
            {
                Console.WriteLine($"Device name changed: {device}");
            };

            watcher.ScannerStartWork();

            Console.ReadLine();
        }
    }
}
