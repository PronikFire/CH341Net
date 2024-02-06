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

        PCA9685 pca9685 = new PCA9685(ref device);
        WriteLine("Is sleep: " + pca9685.IsSleep);
        if (pca9685.IsSleep)
        {
            pca9685.IsSleep = false;
            WriteLine("Wakeup");
        }

        pca9685.SetPWM(0, 30);
        pca9685.SetPWM(1, 30);
        pca9685.SetPWM(2, 30);
        pca9685.SetPWM(3, 30);

        /*for (int i = 100; i > 0 ; i--)
            pca9685.SetPWM(15, i);

        for (int i = 100; i > 0; i--)
            pca9685.SetPWM(14, i);

        for (int i = 100; i > 0; i--)
            pca9685.SetPWM(13, i);*/

        WriteLine("\nPress any key for close device");
        ReadLine();
        WriteLine("Return to sleep");
        pca9685.IsSleep = true;
        WriteLine("Closing the device...");
        device.Close();
    }
}
