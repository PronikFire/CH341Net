using CH341Net;
using static CH341Net.CH341;
using static System.Console;

namespace CH341_Test;

public class Program
{
    public static void Main(string[] args)
    {
        WriteLine($"DLL version: {GetDllVersion()}\n");
        WriteLine("Opening the device at index 0...");
        Device device = new Device(0);
        device.OnDeviceStatusChange += (s) => WriteLine(s);
        WriteLine("Device was opened.");
        WriteLine($"Driver version: {GetDriverVersion()}\n");

        WriteLine($"Device version: {device.DeviceType}");
        WriteLine($"Device name: {device.Name}");
        WriteLine($"Device descriptor: {device.DeviceDescriptor}");
        WriteLine($"Config descriptor: {device.ConfigDescriptor}");
        WriteLine($"Device status: {GetInput(0).ToString("B")}\n");

        WriteLine("Pins state:");
        for (uint i = 0; i <= 15; i++)
            WriteLine($"\t{i} : {device.GetPinState(i)}");

        WriteLine("\nPress any key for close device");
        Read();
        WriteLine("Closing the device...");
        device.Close();
    }
}
