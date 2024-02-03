using System.Runtime.InteropServices;
using System;
using System.Reflection.Metadata;
using System.Text;
using System.Runtime.CompilerServices;

namespace CH341Net;

public class CH341
{
    const string dllName = "CH341DLLA64.DLL";
    const ulong maxBufferLength = 0x1000;

    [DllImport(dllName)]
    private static extern ulong CH341GetVersion();
    [DllImport(dllName)]
    private static extern ulong CH341GetDrvVersion();
    [DllImport(dllName)]
    private static extern bool CH341GetStatus(ulong index, ref ulong status);
    [DllImport(dllName)]
    private static unsafe extern IntPtr CH341GetDeviceName(ulong index);
    [DllImport(dllName)]
    private static extern Handle CH341OpenDevice(ulong index);
    [DllImport(dllName)]
    private static extern void CH341CloseDevice(ulong index);
    [DllImport(dllName)]
    private static extern bool CH341ResetDevice(ulong index);
    [DllImport(dllName)]
    private static unsafe extern bool CH341GetDeviceDescr(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static unsafe extern bool CH341GetConfigDescr(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool CH341ReadInter(ulong index, ref ulong status);
    [DllImport(dllName)]
    private static extern bool CH341AbortInter(ulong index);
    [DllImport(dllName)]
    private static extern bool CH341SetParaMode(ulong index, ulong mode);
    [DllImport(dllName)]
    private static extern bool CH341InitParallel(ulong index, ulong mode);
    [DllImport(dllName)]
    private static unsafe extern bool CH341ReadData0(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341ReadData1(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341AbortRead(ulong index);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341WriteData0(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341WriteData1(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341AbortWrite(ulong index);
    [DllImport(dllName)]
    private static extern bool  CH341ReadI2C(ulong index, byte device, byte address, ref byte value);
    [DllImport(dllName)]
    private static extern bool  CH341WriteI2C(ulong index, byte device, byte address, byte value);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341EppReadData(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341EppReadAddr(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341EppWriteData(ulong index, void* iBuffer, ref ulong ioLength);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341EppWriteAddr(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341EppSetAddr(ulong index, byte address);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341MemReadAddr0(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341MemReadAddr1(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341MemWriteAddr0(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341MemWriteAddr1(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern bool  CH341SetExclusive(ulong index, bool isExclusive);
    [DllImport(dllName)]
    private static extern bool  CH341SetTimeout(ulong index, ulong writeTimeout, ulong readTimeout);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341ReadData(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341WriteData(ulong index, void* buffer, ref ulong length);
    [DllImport(dllName)]
    private static extern ulong  CH341GetVerIC(ulong index);
    [DllImport(dllName)]
    private static extern bool  CH341FlushBuffer(ulong index);
    [DllImport(dllName)]
    private static unsafe extern bool  CH341WriteRead(ulong index, ulong writeLength, void* writeBuffer, ulong readStep, ulong readTimes, ref ulong readLength, void* readBuffer);
    [DllImport(dllName)]
    private static extern bool CH341SetStream(ulong index, ulong mode);
    [DllImport(dllName)]
    private static extern bool CH341SetDelaymS(ulong index, ulong delay);
    [DllImport(dllName)]
    private static unsafe extern bool CH341StreamI2C(ulong index, ulong writeLength, void* writeBuffer, ulong readLength, void* readBuffer);
    [DllImport(dllName)]
    private static extern bool CH341GetInput(ulong index, ref ulong status);
    [DllImport(dllName)]
    private static extern bool CH341SetOutput(ulong index, ulong enable, ulong setDirOut, ulong setDataOut);
    [DllImport(dllName)]
    private static extern bool CH341SetDeviceNotify(ulong index, IntPtr deviceID, NotifyRoutine notifyRoutine);
    [DllImport(dllName)]
    private static extern bool CH341SetIntRoutine(ulong index, IntRoutine intRoutine);


    public static unsafe byte[] ReadData0(ulong index)
    {
        ulong length = maxBufferLength;
        byte[] buffer = new byte[length];
        if (!CH341ReadData0(index, &buffer, ref length))
            throw new Exception("Read data at 0 error.");
        return buffer;
    }

    public static void InitParallel(ulong index, ParaMode mode)
    {
        if (CH341InitParallel(index, (ulong)mode))
            throw new Exception("Set parallel port mode error.");
    }

    public static void SetParaMode(ulong index, ParaMode mode)
    {
        if (CH341SetParaMode(index, (ulong)mode))
            throw new Exception("Set parallel port mode error.");
    }

    public static void AbortInter(ulong index)
    {
        if (!CH341AbortInter(index))
            throw new Exception("Abort inter error.");
    }

    public static ulong ReadInter(ulong index)
    {
        ulong status = 0;
        if (!CH341ReadInter(index, ref status))
            throw new Exception("Read inter error.");
        return status;
    }

    public static void SetIntRoutine(ulong index, IntRoutine intRoutine)
    {
        if (!CH341SetIntRoutine(index, intRoutine))
            throw new Exception("Set int routine error.");
    }

    public static void ResetDevice(ulong index)
    {
        if (!CH341ResetDevice(index))
            throw new Exception("Reset device error.");
    }

    public static void SetDeviceNotify(ulong index, NotifyRoutine notifyRoutine)
    {
        if (!CH341SetDeviceNotify(index, IntPtr.Zero, notifyRoutine))
            throw new Exception("Set device notify error.");
    }

    public static byte ReadI2C(ulong index, byte slave, byte address)
    {
        byte result = 0;
        if (!CH341ReadI2C(index, slave, address, ref result))
            throw new Exception("Read I2C error.");
        return result;
    }

    /// <summary>
    ///  Set the I/O direction for the CH341 and output data over the CH341.
    /// </summary>
    /// <param name="index">Specify the CH341 device number.</param>
    /// <param name="enable"> Data valid flag, refer to the bit description below.</param>
    /// <param name="setDirOut">To set the I/O direction, pin 0 corresponds to input and pin 1 corresponds to output. In parallel port mode, the default value is 0x000FC000. Refer to the bit description below</param>
    ///<param name="setDataOut">Output data. If the I/O direction is output, then a clear 0 corresponds to pin output low level, and a position 1 corresponds to pin output high level, refer to the bit description below</param>
    public static void SetOutput(ulong index, ulong enable, ulong setDirOut, ulong setDataOut)
    {
        if (!CH341SetOutput(index, enable, setDirOut,setDataOut))
            throw new Exception("SetOutput error.");
    }

    /// <summary>
    /// Set the hardware asynchronous delay to a specified number of milliseconds before the next stream operation.
    /// </summary>
    /// <param name="index">Specify the CH341 device number.</param>
    /// <param name="delay">Specifies the number of milliseconds to delay.</param>
    public static void SetDelay(ulong index, ulong delay)
    {
        if (!CH341SetDelaymS(index, delay))
            throw new Exception("Error setting the delay.");
    }

    /// <summary>
    /// Get the DLL version number, return the version number.
    /// </summary>
    public static ulong GetDllVersion() => CH341GetVersion();
    /// <summary>
    /// Get the driver version number, return the version number, or return 0 if there is an error.
    /// Will return an error if it was called before the device was opened.
    /// </summary>
    public static ulong GetDriverVersion()
    {
        var result = CH341GetVersion();
        if (result == 0)
            throw new Exception("Error getting driver version.");
        return result;
    }
    /// <summary>
    /// Input data and status directly through CH341
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device</param>
    /// <returns>
    /// Bit 7-bit 0 correspond to D7-D0 pins of CH341.
    /// Bit 8 corresponds to ERR# pin of CH341, Bit 9 corresponds to PEMP pin of CH341, Bit 10 corresponds to INT# pin of CH341, Bit 11 corresponds to SLCT pin of CH341, Bit 23 corresponds to SDA pin of CH341.
    /// Bit 13 corresponds to BUSY/WAIT# pin of CH341, Bit 14 corresponds to AUTOFD#/DATAS# pin of CH341, Bit 15 corresponds to SLCTIN#/ADDRS# pin of CH341.
    /// </returns>
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
    /// <returns>Returns the CH341 device name.</returns>
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
    /// <returns>Return the handle, if an error occurs, it will be invalid.</returns>
    public static Handle OpenDevice(ulong index)
    {
        var handle = CH341OpenDevice(index);
        if ((int)handle.Kind == 127)
            throw new Exception("The device could not open.");
        return handle;
    }
    /// <summary>
    /// Close the CH341 device.
    /// </summary>
    /// <param name="index"> Specify the serial number of the CH341 device.</param>
    public static void CloseDevice(ulong index) => CH341CloseDevice(index);
    public static unsafe string GetDeviceDescr(ulong index)
    {
        ulong length = maxBufferLength;
        byte[] buffer = new byte[length];

        if (!CH341GetDeviceDescr(index, &buffer, ref length))
            throw new Exception("It is not possible to get the value.");

        return Encoding.Unicode.GetString(buffer, 0, (int)length);
    }
    public static unsafe string GetConfigDescr(ulong index)
    {
        ulong length = maxBufferLength;
        byte[] buffer = new byte[length];

        if (!CH341GetConfigDescr(index, &buffer, ref length))
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
    /// <summary>
    /// Using the CH341 to directly enter data and status is more efficient than using the <see cref="GetStatus(ulong)"/>
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device</param>
    /// <returns>
    /// Bit 7-bit 0 correspond to D7-D0 pins of CH341.
    /// Bit 8 corresponds to ERR# pin of CH341, Bit 9 corresponds to PEMP pin of CH341, Bit 10 corresponds to INT# pin of CH341, Bit 11 corresponds to SLCT pin of CH341, Bit 23 corresponds to SDA pin of CH341.
    /// Bit 13 corresponds to BUSY/WAIT# pin of CH341, Bit 14 corresponds to AUTOFD#/DATAS# pin of CH341, Bit 15 corresponds to SLCTIN#/ADDRS# pin of CH341.
    /// </returns>
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

    public enum DeviceVersion : ulong
    {
        CH341A = 0x20,
        CH341A3 = 0x30,
        CH341 = 0x10,
        Unknown
    }

    public enum DeviceStatus : ulong
    {
        Arrival = 3,
        Remove_Pend = 1,
        Remove = 0
    }
    public enum ParaMode : ulong
    {
        mCH341_PARA_MODE_EPP = 0x00,
        mCH341_PARA_MODE_EPP17 = 0x00,
        mCH341_PARA_MODE_EPP19 = 0x01,
        mCH341_PARA_MODE_MEM = 0x02,
        mCH341_PARA_MODE_ECP = 0x03
    }

    public delegate void NotifyRoutine(DeviceStatus deviceStatus);
    public delegate void IntRoutine(ulong status);
}
