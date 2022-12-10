using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBLE.BLE
{
    // Информация о поступившем BLE устройстве
    public class BLEDeviceInfo
    {

        #region Public Properties
        
        // время широковещательного рекламного сообщения устройства
        public DateTimeOffset BroadcastTime { get; }

        // Адресс устройства
        public ulong Address { get; }

        // Имя устройства
        public string Name { get; }

        // Мощность/Сила сигнала, измеряемая в Дб
        public short SignalStrengthInDB { get; }

        #endregion

        #region Constuctor
        
        public BLEDeviceInfo(ulong address, string name, short rssi, DateTimeOffset broadcastTime)
        {
            Address = address;
            if (string.IsNullOrEmpty(name))
                Name = "[No Name]";
            else
                Name = name;
            SignalStrengthInDB = rssi;
            BroadcastTime = broadcastTime;
        }

        #endregion

        // Возвращает информацию о устройстве (Имя и адрес) в формате строки
        public override string ToString()
        {
            return $"Name: { (string.IsNullOrEmpty(Name) ? "[No Name]" : Name) } Adress: {Address}";
        }
    }

}
