using CH341Net;
using System.Linq;
using static System.Console;

public class Program
{
    public static void Main(string[] args)
    {
        WriteLine($"DLL version: {CH341.GetDllVersion()}\n");
        WriteLine("Opening the device at index 0...");
        Device device = new Device();
        device.Open(0);
        WriteLine("Device was opened.");
        WriteLine($"Driver version: {CH341.GetDriverVersion()}\n");

        WriteLine($"Device version: {device.DeviceType}");
        WriteLine($"Device name: {device.Name}");
        WriteLine($"Device descriptor: {device.DeviceDescriptor}");
        WriteLine($"Config descriptor: {device.ConfigDescriptor}");
        WriteLine($"Device status: {CH341.GetInput(0).ToString("B")}");

        WriteLine("Press any key for close device");
        ReadLine();
        WriteLine("\nClosing the device...");
        CH341.CloseDevice(0);
        WriteLine("Device was closed.");
    }
}
