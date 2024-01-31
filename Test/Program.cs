using CH341Net;
using static System.Console;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        WriteLine($"DLL version: {CH341.GetVersion()}\n");
        WriteLine("Opening the device at index 0...");
        CH341.OpenDevice(0);
        WriteLine("Device was opened.");
        WriteLine($"Driver version: {CH341.GetDriverVersion()}\n");

        WriteLine($"Device version: {CH341.GetDeviceVersion(0)}");
        WriteLine($"Device name: {CH341.GetDeviceName(0)}");
        WriteLine($"Device status: {CH341.GetInput(0)}");

        WriteLine("\nClosing the device...");
        CH341.CloseDevice(0);
        WriteLine("Device was closed.");
        ReadLine();
    }
}
