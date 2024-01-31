using System.Runtime.InteropServices;
using System;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CH341Net;

public class CH341
{
    const string dllName = "CH341DLLA64.DLL";

    [DllImport(dllName)]
    private static extern ulong CH341GetVersion();
    [DllImport(dllName)]
    private static extern ulong CH341GetDrvVersion();
    [DllImport(dllName)]
    private static extern bool CH341GetStatus(ulong index, ref ulong status);
    [DllImport(dllName)]
    private static extern IntPtr CH341GetDeviceName(ulong index);
    [DllImport(dllName)]
    private static extern Handle CH341OpenDevice(ulong index);
    [DllImport(dllName)]
    private static extern void CH341CloseDevice(ulong index);
    [DllImport(dllName)]
    private static extern bool CH341ResetDevice(ulong index);
    [DllImport(dllName)]
    private static extern bool CH341GetDeviceDescr(ulong index, IntPtr buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool CH341GetConfigDescr(ulong index, IntPtr buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool CH341ReadInter(ulong index, ref ulong status);
    [DllImport(dllName)]
    private static extern bool CH341AbortInter(ulong index);
    [DllImport(dllName)]
    private static extern bool CH341SetParaMode(ulong index, ulong mode);
    [DllImport(dllName)]
    private static extern bool CH341InitParallel(ulong index, ulong mode);
    [DllImport(dllName)]
    private static extern bool CH341ReadData0(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341ReadData1(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341AbortRead(ulong index);
    [DllImport(dllName)]
    private static extern bool  CH341WriteData0(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341WriteData1(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341AbortWrite(ulong index);
    [DllImport(dllName)]
    private static extern bool  CH341ReadI2C(ulong index, byte device, byte address, ref byte value);
    [DllImport(dllName)]
    private static extern bool  CH341WriteI2C(ulong index, byte device, byte address, byte value);
    [DllImport(dllName)]
    private static extern bool  CH341EppReadData(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341EppReadAddr(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341EppWriteData(ulong index, ref object iBuffer, ref ulong ioLength);
    [DllImport(dllName)]
    private static extern bool  CH341EppWriteAddr(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341EppSetAddr(ulong index, byte address);
    [DllImport(dllName)]
    private static extern bool  CH341MemReadAddr0(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341MemReadAddr1(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341MemWriteAddr0(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341MemWriteAddr1(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341SetExclusive(ulong index, bool isExclusive);
    [DllImport(dllName)]
    private static extern bool  CH341SetTimeout(ulong index, ulong writeTimeout, ulong readTimeout);
    [DllImport(dllName)]
    private static extern bool  CH341ReadData(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341WriteData(ulong index, ref object buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern ulong  CH341GetVerIC(ulong index);
    [DllImport(dllName)]
    private static extern bool  CH341FlushBuffer(ulong index);
    [DllImport(dllName)]
    private static extern bool  CH341WriteRead(ulong index, ulong writeLength, ref object writeBuffer, ulong readStep, ulong readTimes, ref ulong readLength, ref object readBuffer);
    [DllImport(dllName)]
    private static extern bool CH341SetStream(ulong index, ulong mode);
    [DllImport(dllName)]
    private static extern bool CH341SetDelaymS(ulong index, ulong delay);
    [DllImport(dllName)]
    private static extern bool CH341StreamI2C(ulong index, ulong writeLength, IntPtr writeBuffer, ulong readLength, IntPtr readBuffer);
    [DllImport(dllName)]
    private static extern bool CH341GetInput(ulong iIndex, ref ulong iStatus);


    public static ulong GetVersion() => CH341GetVersion();
    public static ulong GetDriverVersion() => CH341GetDrvVersion();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public static ulong GetStatus(ulong index)
    {
        ulong status = 0;
        if (!CH341GetStatus(index, ref status))
            throw new Exception("Error getting result");
        return status;
    }
    /// <summary>
    /// Get the CH341 device name.
    /// </summary>
    /// <param name="index">Specify the CH341 device number,0 corresponds to the first device.</param>
    /// <returns>Returns the CH341 device name, or NULL on error.</returns>
    public static string GetDeviceName(ulong index)
    {
        var name = Marshal.PtrToStringUTF8(CH341GetDeviceName(index));
        if (name is null)
            throw new Exception("Error getting device name.");
        return name;
    }

    /// <summary>
    /// Open the CH341 device.
    /// </summary>
    /// <param name="index">Specify the device serial number of CH341, 0 corresponds to the first device</param>
    /// <returns>Return the handle, if an error occurs, it will be -1.</returns>
    public static Handle OpenDevice(ulong index) => CH341OpenDevice(index);
    public static void CloseDevice(ulong index) => CH341CloseDevice(index);
    public static string GetDeviceDescr(ulong index)
    {
        ulong length = 1024;
        byte[] buffer = new byte[length];

        if (!CH341GetDeviceDescr(index, Marshal.GetIUnknownForObject(buffer), ref length))
            throw new Exception("It is not possible to get the value.");

        return Encoding.Unicode.GetString(buffer, 0, (int)length);
    }
    public static string GetConfigDescr(ulong index)
    {
        ulong length = 1024;
        byte[] buffer = new byte[length];

        if (!CH341GetConfigDescr(index, Marshal.GetIUnknownForObject(buffer), ref length))
            throw new Exception("It is not possible to get the value.");

        return Encoding.Unicode.GetString(buffer, 0, (int)length);
    }
    public static void WriteI2C(ulong index, byte device, byte address, byte value)
    {
        if (!CH341WriteI2C(index, device, address, value))
            throw new Exception("I2C write error.");
    }

    public static DeviceVersion GetDeviceVersion(ulong index)
    {
        var result = CH341GetVerIC(index);
        if (result == 0)
            throw new Exception("Error determining device version.");
        return (DeviceVersion)result;
    }

    public static ulong GetInput(ulong index)
    {
        ulong status = 0;
        if (!CH341GetInput(index, ref status))
            throw new Exception("Error getting result");

        return status;
    }

    public static void ClearBuffer(ulong index)
    {
        if (!CH341FlushBuffer(index))
            throw new Exception("Buffer clearing error");
    }

    public enum DeviceVersion
    {
        CH341A = 0x20,
        CH341A3 = 0x30,
        CH341 = 0x10,
        Unknown
    }
}
