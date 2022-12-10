using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBLE.BLE;
using TestBLE.BLE.EventArgsClasses;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Automation.Peers;

namespace TestBLE.ExampleDevices
{
    public class ESP32
    {
        #region privateFields
        private BLEDeviceController esp;

        // Uuid характеристик и сервисов
        private string ServiceUUID = "17b5290c-2e8b-11ed-a261-0242ac120002";

        private string xUUID = "17b52c72-2e8b-11eb-a261-0242ac120002";
        private string yUUID = "17b52c72-2e8b-11ef-a261-0242ac120002";
        #endregion

        #region publicFields

        public const int packSize = 100;

        //Массивы хранения данных { get; private set }
        public float[] pointsX { get; private set; } = new float[packSize];
        public float[] pointsY { get; private set; } = new float[packSize];

        #endregion

        #region classInterface

        // Конструктор класса
        public ESP32()
        {
            esp = new BLEDeviceController();
        }

        // Соединиться с устройством
        public async Task<bool> Connect(ulong addres)
        {
            await esp.Connect(addres);
            await esp.GetServices();
            return true;
        }
        // Начать работу
        public async Task<bool> Start()
        {
            await esp.FindServiceCharacteristics(ServiceUUID);
            await esp.SubscribeToCharacteristicIndicate(xUUID, XChanged);
            await esp.SubscribeToCharacteristicIndicate(yUUID, YChanged);
            return true;
        }
        // Получить данные (несколько методов)

        // Прервать соединение с устройством
        public void Disconnect()
        {
            esp.Disconnet();
        }

        // Метод возвращает размер пакета данных
        public int GetPackSize() 
        { 
            return packSize; 
        }

        #endregion

        #region privateMethods

        // Переводы массива байт в нужные значения
        private static byte[] ReadFromBuffer(IBuffer buff)
        {
            var reader = DataReader.FromBuffer(buff);
            byte[] input = new byte[reader.UnconsumedBufferLength];
            reader.ReadBytes(input);
            return input;
        }

        private float[] ToFloatArray(byte[] input)
        {
            //var len = input.Length * sizeof(byte) / sizeof(float);
            //byte[] temp = input[15];
            var len = 100;
            int index = 15;
            float[] result = new float[len];
            for (int i = 0; i < len; i++)
            {
                index += i * sizeof(float);
                result[i] = BitConverter.ToSingle(input, index);
                index = 15;
            }
            return result;

        }



        #endregion

        #region Events

        public EventHandler<ReadyBLEDataEventArgs> PrintingDataReady;
        private void OnPrintingDataReady(ReadyBLEDataEventArgs e)
        {
            EventHandler<ReadyBLEDataEventArgs> handler = PrintingDataReady;
            if (handler != null)
                handler(this, e);
        }

        // События для изменения данных, приходящих с ESP32
        private void XChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {

            //var raw = ReadFromBuffer(args.CharacteristicValue);
            //pointsX = ToFloatArray(raw);

            
        }

        private void YChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {

            var raw = ReadFromBuffer(args.CharacteristicValue);
            var localPointsY = ToFloatArray(raw);
            this.OnPrintingDataReady(new ReadyBLEDataEventArgs() { Data = localPointsY });
        }

        #endregion

    }
}
