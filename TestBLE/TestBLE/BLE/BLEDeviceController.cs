using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBLE.BLE.EventArgsClasses;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace TestBLE.BLE
{

    public class BLEDeviceController
    {
        

        public EventHandler<ResponsedData> DataReady;

        private void OnDataReady(ResponsedData e)
        {
            EventHandler<ResponsedData> handler = DataReady;
            if (handler != null)
                handler(this, e);
        }


        // Множество сервисов устройства
        private GattDeviceServicesResult services;

        // Все характеристики
        private GattCharacteristicsResult characteristics;

        // Само устройство
        private BluetoothLEDevice bluetoothLEDevice;

        // Статус подключения устройства
        private bool isConnection = false;

        //Статус получения сервисов устройства
        private bool isServices = false;

        //public string Battery_Service_ID = "91bad492-b950-4226-aa2b-4ede9fa42f59";

        //private readonly Guid Battery_Service_ID = new Guid("91bad492-b950-4226-aa2b-4ede9fa42f59");



        public string str;


        // Events
        public event Action DataChanged = () => { };
        #region Constructor

        public BLEDeviceController()
        {

        }

        #endregion

        #region Interface

        public async Task<bool> Connect(ulong addres)
        {
            //return Task.Run(async () =>
            //{
                bluetoothLEDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(addres);
                if (bluetoothLEDevice.ConnectionStatus == BluetoothConnectionStatus.Connected)
                {
                    isConnection = true;
                    return true;
                }
                isConnection = false;
                return false;
            //});
        }

        public async Task<bool> GetServices()
        {
            //return Task.Run(async () =>
            //{
                services = await bluetoothLEDevice.GetGattServicesAsync();
                if (services.Status == GattCommunicationStatus.Success)
                {
                    isServices = true;
                    return true;
                }
                isServices = false;
                return false;
            //});
        }

        public bool IsConnected()
        {
            return isConnection;
        }

        public async Task<bool> FindServiceCharacteristics(string uuidService)
        {
            foreach(var service in services.Services)
            {
                if(service.Uuid.ToString().StartsWith(uuidService))
                {
                    characteristics = await service.GetCharacteristicsAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> SubscribeToCharacteristic(string uuidCharacteristic, TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> yourEvent)
        {
            if (characteristics.Status == GattCommunicationStatus.Success)
            {
                var characteristic = FindCharacteristic(characteristics.Characteristics, uuidCharacteristic);
                var flags = characteristic.CharacteristicProperties;
                if (flags.HasFlag(GattCharacteristicProperties.Notify))
                {
                    GattCommunicationStatus status = await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                    if (status == GattCommunicationStatus.Success)
                    {
                        characteristic.ValueChanged += yourEvent;
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> SubscribeToCharacteristicIndicate(string uuidCharacteristic, TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> yourEvent)
        {
            if (characteristics.Status == GattCommunicationStatus.Success)
            {
                var characteristic = FindCharacteristic(characteristics.Characteristics, uuidCharacteristic);
                var flags = characteristic.CharacteristicProperties;
                if (flags.HasFlag(GattCharacteristicProperties.Indicate))
                {
                    GattCommunicationStatus status = await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Indicate);
                    if (status == GattCommunicationStatus.Success)
                    {
                        characteristic.ValueChanged += yourEvent;
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<byte[]> ReadRawAsync(string uuidCharacteristic)
        {
            byte[] result;
            var characteristic = FindCharacteristic(characteristics.Characteristics, uuidCharacteristic);
            result = await ReadBufferToByteArrayAsync(characteristic);
            if (result == null)
                return null;
            return result;
        }

        public static byte[] ReadFromBuffer(IBuffer buff)
        {
            var reader = DataReader.FromBuffer(buff);
            byte[] input = new byte[reader.UnconsumedBufferLength];
            reader.ReadBytes(input);
            return input;
        }

        public void Disconnet()
        {
            isConnection = false;
            isServices = false;
            characteristics = null;
            services = null;
            bluetoothLEDevice.Dispose();
        }

        #endregion

        #region PrivateMethods

        private GattCharacteristic FindCharacteristic(IReadOnlyList<GattCharacteristic> characteristics, string uuidCharacteristic)
        {
            foreach (var characterictic in characteristics)
            {
                if (characterictic.Uuid.ToString().StartsWith(uuidCharacteristic))
                    return characterictic;
            }
            return null;
        }

        private async Task<byte[]> ReadBufferToByteArrayAsync(GattCharacteristic yourCharacteristic)
        {
            var flags = yourCharacteristic.CharacteristicProperties;
            if (flags.HasFlag(GattCharacteristicProperties.Read))
            {
                GattReadResult gattResult = await yourCharacteristic.ReadValueAsync();
                if (gattResult.Status == GattCommunicationStatus.Success)
                {
                    var reader = DataReader.FromBuffer(gattResult.Value);
                    byte[] input = new byte[reader.UnconsumedBufferLength];
                    reader.ReadBytes(input);
                    return input;

                }
            }
            return null;
        }

        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            DataReader reader = DataReader.FromBuffer(args.CharacteristicValue);
            byte[] byteArray = new byte[reader.UnconsumedBufferLength];

            reader.ReadBytes(byteArray);

            str = Encoding.ASCII.GetString(byteArray);

            ResponsedData data = new ResponsedData() { Data = str };
            OnDataReady(data);
        }

        #endregion
    }
}
