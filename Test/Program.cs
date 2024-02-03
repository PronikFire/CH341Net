using CH341Net;
using static System.Console;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

public class Program
{
    public static bool isRun = true;
    public static void Main(string[] args)
    {
        WriteLine($"DLL version: {CH341.GetDllVersion()}\n");
        WriteLine("Opening the device at index 0...");
        CH341.OpenDevice(0);
        WriteLine("Device was opened.");
        WriteLine($"Driver version: {CH341.GetDriverVersion()}\n");

        WriteLine($"Device version: {CH341.GetDeviceVersion(0)}");
        WriteLine($"Device name: {CH341.GetDeviceName(0)}");
        WriteLine($"Device status: {CH341.GetInput(0).ToString("B")}");
        //var handler = new Thread(ReadHandler);
        //handler.IsBackground = true;
        //handler.Start();
        WriteLine("Write i2C");
        CH341.WriteI2C(0, 0x40, 7, 8);
             
        WriteLine("Press any key for close device");
        ReadLine();
        //isRun = false;
        //while (handler.ThreadState != ThreadState.Stopped) { }
        WriteLine("\nClosing the device...");
        CH341.CloseDevice(0);
        WriteLine("Device was closed.");
    }

    public static void DeviceEvent(ulong status)
    {
        WriteLine(status);
    }

    public static void ReadHandler()
    {
        while (isRun)
        {
            WriteLine("Input: " + CH341.ReadI2C(0, 0x40, 251));
        }
    }
}
