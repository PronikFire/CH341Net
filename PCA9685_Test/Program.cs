using CH341Net;
using SlaveDevices.Drivers;
using static CH341Net.CH341;
using static System.Console;

namespace PCA9685_Test;

public class Program
{
    public static void Main(string[] args)
    {
        WriteLine($"DLL version: {GetDllVersion()}\n");
        WriteLine("Opening the device at index 0...");
        Device device = new Device(0);
        WriteLine("Device was opened.");
        WriteLine($"Driver version: {GetDriverVersion()}\n");

        WriteLine($"Device version: {device.DeviceType}");
        WriteLine($"Device name: {device.Name}");
        WriteLine($"Device descriptor: {device.DeviceDescriptor}");
        WriteLine($"Config descriptor: {device.ConfigDescriptor}\n");

        var pca9685 = new PCA9685(ref device);
        WriteLine("Is sleep: " + pca9685.IsSleep);
        if (pca9685.IsSleep)
        {
            pca9685.IsSleep = false;
            WriteLine("Wakeup");
        }

        pca9685.SetPWMAll(0, 1228);

        WriteLine("\nPress any key for close device");
        ReadLine();
        WriteLine("Return to sleep");
        pca9685.IsSleep = true;
        WriteLine("Closing the device...");
        device.Close();
    }
}
