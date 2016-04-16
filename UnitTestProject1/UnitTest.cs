using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Networking.Sockets;
using Windows.Networking;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            try
            {

                for (int i = 0; i < 10; i++)
                {
                    await NewMethod(); 
                }

                Assert.IsTrue(true);

            }
            catch (Exception x)
            {

                Assert.Fail(DateTime.Now.ToString() + " " + x.Message);
            }
        }

        private static async System.Threading.Tasks.Task NewMethod()
        {
               using (StreamSocket s = new StreamSocket())
                {
            //StreamSocket s = new StreamSocket();
                await s.ConnectAsync(new HostName("127.0.0.1"), "502");


                Modbus.Device.ModbusIpMaster p = Modbus.Device.ModbusIpMaster.CreateIp(s);

                for (ushort i = 0; i < 100; i++)
                {
                    p.WriteSingleRegister(0, i);
                    System.Diagnostics.Debug.WriteLine(i);
                    await Task.Delay(1000);
                }
            } 
        }
    }
}
