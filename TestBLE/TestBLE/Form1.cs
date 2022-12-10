using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
////
using TestBLE.BLE;
using TestBLE.BLE.EventArgsClasses;
using TestBLE.ExampleDevices;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

namespace TestBLE
{
    public partial class Form1 : Form
    {
        //private BluetoothLEAdvertisementWatcher watcher = null;
        //private ScannerDevices watcher;

        private string Password = "12345";
        private bool PasswordAcept = false;

        private ScannerOfDevices watcher;

        private BLEDeviceController _device;

        private ESP32 myDevice;
        Timer UpdateTimer = new System.Windows.Forms.Timer();
        
        private DeviceWatcher deviceWatcher = null;

        public BluetoothLEDevice bluetoothLEDevice = null;

        public GattDeviceServicesResult result { get; set; }

        public DeviceInformation device = null;

        public GattCharacteristic selectedCharacteristic = null;

        public GattDeviceService selectedService = null;

        public string Battery_Service_ID = "91bad492-b950-4226-aa2b-4ede9fa42f59";

        //Лист точек
        List<float> pointsY;
        List<float> pointsX;

        private bool dataOnline = true;
        private ulong lastendposs = 0;
        private bool FromOnlinetoAll = true;
        public Form1()
        {
            InitializeComponent();
            watcher = new ScannerOfDevices();
            //_device = new BLEDeviceController();
            myDevice = new ESP32();
            //_device.DataReady += PrintNewData;
            pointsY = new List<float>();
            pointsX = new List<float>();

            const int Scroll = 20;//количество точек до скрола
            chart1.ChartAreas[0].AxisX.Interval = 1; //интервал делений X
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;//скрол над цифрами
            chart1.ChartAreas[0].AxisX.ScaleView.Size = Scroll;//размер скрола
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;//только полоса
            chart1.ChartAreas[0].AxisX.ScrollBar.BackColor = System.Drawing.Color.DarkGray; //цвета
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonColor = System.Drawing.Color.DarkGray; //цвета
            PasswordPanel.Visible = false;

            watcher.NewDeviceDiscovered += (device) =>
            {
                listBox1.Items.Clear();
                listView1.Items.Clear();

                BeginInvoke(new MethodInvoker(delegate
                {
                    List<BLEDeviceInfo> s = watcher.DiscoveredDevices.ToList();
                    if (s.Count > 0)
                    {
                        for (int i = 0; i < s.Count; i++)
                        {
                            string[] s1 = { s[i].Name, s[i].Address.ToString() };
                            string s2 = s[i].ToString();
                            ListViewItem lst = new ListViewItem(s1);
                            listView1.Items.Add(lst);
                            listBox1.Items.Add(s2);
                        }

                    }
                }));
            };

        }

        

        private void PrintNewData(object sender, ResponsedData e)
        {
            lbldata.Invoke(new Action(() =>
            {
                lbldata.Text = e.Data;
            }));

        }

        #region Button_Click_Events

        private void Scan_Click(object sender, EventArgs e)
        {
            Ble_StartScanning();
        }

        private void StopScan_Click(object sender, EventArgs e)
        {
            Ble_StopScanning();
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            PasswordPanel.Visible = true;
            if (PasswordAcept)
            {
                Ble_Connection();
                PasswordPanel.Visible = false;
            }
            //Если подключено, то _device.DataReady += PrintNewData;
        }

        private bool CheckPassword()
        {
            if (InputPassword.Text == Password)
            {
                MessageBox.Show("Доступ разрешён нажмите ещё раз на кнопку подключиться!");
                return true;
            }
            else
            {
                MessageBox.Show("Доступ запрёщен!");
                return false;
            }
        }

        private void Disconnect_Click(object sender, EventArgs e)
        {
            //_device.DataReady -= PrintNewData;
            lblStatus.Text = "Disconnection";
            myDevice.Disconnect();
            if (dataOnline)
                myDevice.PrintingDataReady -= PrintDataOnline;
            else
                myDevice.PrintingDataReady -= PrintDataAll;

        }

        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

/*            UpdateTimer.Interval = 5000;
            UpdateTimer.Enabled = true;
            UpdateTimer.Tick += new EventHandler(UpdateValues);*/
        }

        private void UpdateValues(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            for (int i = 0; i < myDevice.GetPackSize(); i++)
            {
                chart1.Series[0].Points.AddXY(myDevice.pointsX[i], Math.Round(myDevice.pointsY[i],3));
            }
        }

        #region Ble_Connection

        private void Ble_StartScanning()
        {
            watcher.ScannerStartWork();
        }

        private void Ble_StopScanning()
        {
            watcher.ScannerStopWork();
        }

        // Я подключаюсь и изменения данных идёт, теперь мне их нужно на экран вывести, что для этого нужно сделать?
        private async void Ble_Connection()
        {
            lblStatus.Text = "Connecting...";
            myDevice.PrintingDataReady += PrintDataOnline;
            ulong uuid = Convert.ToUInt64(listView1.SelectedItems[0].SubItems[1].Text);
            //bool connectionState = await _device.Connect(uuid);
            //if (connectionState)
            //{
            //    _device.Work();
            //}
            //string tem = "s";
            await myDevice.Connect(uuid);
            await myDevice.Start();
        }

        #endregion

        #region GraphVizualization
        private void PrintDataOnline(object sender, ReadyBLEDataEventArgs e)
        {
            chart1.Series[0].Points.Clear();
            float[] printingData = e.Data;

            chart1.Invoke(new Action(() =>
            {
                for (int i = 0; i < printingData.Length; i++)
                {
                    pointsX.Add(Convert.ToSingle(i * Math.PI / 12 + lastendposs)); pointsY.Add(printingData[i]);
                    chart1.Series[0].Points.AddXY(i * Math.PI / 12 + lastendposs, printingData[i]);
                }
                lastendposs = lastendposs + Convert.ToUInt64(Math.PI / 12 * printingData.Length);
            }));

        }

        private void PrintDataAll(object sender, ReadyBLEDataEventArgs e)
        {
            

            if (FromOnlinetoAll) {
                chart1.Series[0].Points.Clear();
                chart1.Invoke(new Action(() =>
                {
                    for (int i = 0; i < pointsY.Count; i++)
                    {
                        chart1.Series[0].Points.AddXY(pointsX[i], pointsY[i]);
                    }
                }));
                FromOnlinetoAll = false;
            }
            float[] printingData = e.Data;

            chart1.Invoke(new Action(() =>
            {
                for (int i = 0; i < printingData.Length; i++)
                {
                    pointsX.Add(Convert.ToSingle(i * Math.PI / 12 + lastendposs)); pointsY.Add(printingData[i]);
                    chart1.Series[0].Points.AddXY(i * Math.PI / 12 + lastendposs, printingData[i]);
                }
                lastendposs = lastendposs + Convert.ToUInt64(Math.PI / 12 * printingData.Length);
            }));
        }

        #endregion

        private void all_data_Click(object sender, EventArgs e)
        {
            all_data.BackColor = System.Drawing.Color.Green;
            online_data.BackColor = System.Drawing.Color.Red;
            myDevice.PrintingDataReady -= PrintDataOnline;
            myDevice.PrintingDataReady += PrintDataAll;
            dataOnline = false;
            FromOnlinetoAll = true;

        }

        

        private void online_data_Click(object sender, EventArgs e)
        {
            all_data.BackColor = System.Drawing.Color.Red;
            online_data.BackColor = System.Drawing.Color.Green;
            myDevice.PrintingDataReady -= PrintDataAll;
            myDevice.PrintingDataReady += PrintDataOnline;
            dataOnline = true;
            
        }

        private void ButtonCheck_Click(object sender, EventArgs e)
        {
            if (CheckPassword())
                PasswordAcept = true;
        }
    }
}
