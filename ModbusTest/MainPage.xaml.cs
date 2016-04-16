using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ModbusTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();


            try
            {
                Connect();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void Connect()
        {
            try
            {
                StreamSocket s = new StreamSocket();
                await s.ConnectAsync(new HostName("127.0.0.1"), "502");


                Modbus.Device.ModbusIpMaster p = Modbus.Device.ModbusIpMaster.CreateIp(s);

                for (ushort i = 0; i < 100; i++)
                {
                    p.WriteSingleRegister(0, i);
                    textBox.Text = p.ReadHoldingRegisters(0, 1)[0].ToString();
                    System.Diagnostics.Debug.WriteLine(p.ReadHoldingRegisters(0,1)[0]);
                    await Task.Delay(1000);
                }
                
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
