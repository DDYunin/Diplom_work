using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;

//ХОРОШАЯ ИДЕЯ ВЗЯТЬ ИЗ ТОГО ВИДЕО ИДЕЮ ТОГО, ЧТОБЫ УДАЛЯЛИСЬ НЕАКТИВНЫЕ УСТРОЙСТВА! НО ЭТО ЯВЛЯЕТСЯ ДОПОМ.

namespace TestBLE.BLE
{
    public class ScannerOfDevices
    {
        #region Private_Members

        private readonly BluetoothLEAdvertisementWatcher _watcher;

        // Есть смысл сделать его словарём, поскольку адресс устройства ял-ся уникальным и по нему осуществляется доступ к информации всего ус-ва
        //список открытых устройств
        private readonly Dictionary<ulong, BLEDeviceInfo> _discoveredDevices;

        //объект блокировки потока в этом классе
        private readonly object _threadLock = new object();

        #endregion

        #region Public Properties

        //указывает если watcher слушает рекламу
        public bool isWork => _watcher.Status == BluetoothLEAdvertisementWatcherStatus.Started;

        public IReadOnlyCollection<BLEDeviceInfo> DiscoveredDevices
        {
            get
            {
                //Для решения проблемы чтения изменяемого списка
                lock (_threadLock)
                {
                    return _discoveredDevices.Values.ToList().AsReadOnly();
                }
            }
        }

        #endregion

        #region Public Events

        // Сигнализирует, когда watcher перестаёт слушать
        public event Action StoppedListening = () => {};

        // Сигнализирует, когда watcher начинает слушать
        public event Action StartedListening = () => { };

        // Сигнализирует, когда поменялось имя
        public event Action<BLEDeviceInfo> DeviceNameChanged = (device) => { };

        //Сигнализирует, когда появилось устройство
        public event Action<BLEDeviceInfo> DeviceDiscovered = (device) => { };

        //Сигнализирует, когда появилось нвоое устройство
        public event Action<BLEDeviceInfo> NewDeviceDiscovered = (device) => { };
        
        #endregion

        #region Constuctor
        public ScannerOfDevices()
        {
            _discoveredDevices = new Dictionary<ulong, BLEDeviceInfo>();

            _watcher = new BluetoothLEAdvertisementWatcher()
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };

            //Прослушиваем новую рекламу
            _watcher.Received += WatcherAdvertisementReceived;

            //Прослушиваем, когда watcher перестаёт слушать
           _watcher.Stopped += (watcher, e) =>
            {
                StoppedListening();
            };
        }

        #endregion

        #region Private Methods

        private void WatcherAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            BLEDeviceInfo device = null;

            // Проверяем есть ли подобранное устройство в словаре
            var newDiscovery = !_discoveredDevices.ContainsKey(args.BluetoothAddress);

            // Поменялось имя?
            // Данное устройство уже существует
            var nameChanged = !newDiscovery 
                // Имя этого устройства не пустое
                && !string.IsNullOrEmpty(args.Advertisement.LocalName) &&
                // Сохранённое имя устройства не совпадает с текущим
                _discoveredDevices[args.BluetoothAddress].Name != args.Advertisement.LocalName;

            lock (_threadLock)
            {
                var _name = args.Advertisement.LocalName;

                //Защита от повторного изменения имени, когда устройство пришло с пустым, а на самом деле у него есть не пустое имя и оно не новое
                if (string.IsNullOrEmpty(_name) && !newDiscovery)
                    _name = _discoveredDevices[args.BluetoothAddress].Name;

                device = new BLEDeviceInfo
                (
                    //Адресс
                    address: args.BluetoothAddress,
                    //Имя
                    name: _name,
                    //Broadcast
                    broadcastTime: args.Timestamp,
                    //Сила сигнала
                    rssi: args.RawSignalStrengthInDBm

                );

                // Добавить/обновить устройство в словаре
                _discoveredDevices[args.BluetoothAddress] = device;
            }

            DeviceDiscovered(device);

            //Если имя изменилось
            if (nameChanged)
                DeviceNameChanged(device);

            // Если новое устройство
            if (newDiscovery)
                // сигнализируем слушателю
                NewDeviceDiscovered(device);
        
        }

        #endregion

        #region Public Methods

        public void ScannerStartWork()
        {
            if (isWork)
                return;
            _watcher.Start();
            StartedListening();
        }

        public void ScannerStopWork()
        {
            if (!isWork)
                return;
            _watcher.Stop();

            lock (_threadLock)
            {

                //Очищаем все устройства
                _discoveredDevices.Clear();

            }
        }
        #endregion
    }
}
