﻿using System.Runtime.InteropServices;
using System;
using System.Reflection.Metadata;
using System.Text;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CH341Net;

public class CH341
{
    const string dllName = "CH341DLLA64.DLL";
    const uint maxBufferLength = 0x1000;

    [DllImport(dllName)]
    private static extern uint CH341GetVersion();
    [DllImport(dllName)]
    private static extern uint CH341GetDrvVersion();
    [DllImport(dllName)]
    private static extern bool CH341GetStatus(uint index, ref uint status);
    [DllImport(dllName)]
    private static extern IntPtr CH341GetDeviceName(uint index);
    [DllImport(dllName)]
    private static extern Handle CH341OpenDevice(uint index);
    [DllImport(dllName)]
    private static extern void CH341CloseDevice(uint index);
    [DllImport(dllName)]
    private static extern bool CH341ResetDevice(uint index);
    [DllImport(dllName)]
    private static extern bool CH341GetDeviceDescr(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool CH341GetConfigDescr(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool CH341ReadInter(uint index, ref uint status);
    [DllImport(dllName)]
    private static extern bool CH341AbortInter(uint index);
    [DllImport(dllName)]
    private static extern bool CH341SetParaMode(uint index, uint mode);
    [DllImport(dllName)]
    private static extern bool CH341InitParallel(uint index, uint mode);
    [DllImport(dllName)]
    private static extern bool CH341ReadData0(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341ReadData1(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341AbortRead(uint index);
    [DllImport(dllName)]
    private static extern bool  CH341WriteData0(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341WriteData1(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341AbortWrite(uint index);
    [DllImport(dllName)]
    private static extern bool  CH341ReadI2C(uint index, byte device, byte address, ref byte value);
    [DllImport(dllName)]
    private static extern bool  CH341WriteI2C(uint index, byte device, byte address, byte value);
    [DllImport(dllName)]
    private static extern bool  CH341EppReadData(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341EppReadAddr(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341EppWriteData(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341EppWriteAddr(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341EppSetAddr(uint index, byte address);
    [DllImport(dllName)]
    private static extern bool  CH341MemReadAddr0(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341MemReadAddr1(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341MemWriteAddr0(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341MemWriteAddr1(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341SetExclusive(uint index, bool isExclusive);
    [DllImport(dllName)]
    private static extern bool  CH341SetTimeout(uint index, uint writeTimeout, uint readTimeout);
    [DllImport(dllName)]
    private static extern bool  CH341ReadData(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern bool  CH341WriteData(uint index, byte[] buffer, ref uint length);
    [DllImport(dllName)]
    private static extern uint  CH341GetVerIC(uint index);
    [DllImport(dllName)]
    private static extern bool  CH341FlushBuffer(uint index);
    [DllImport(dllName)]
    private static extern bool  CH341WriteRead(uint index, uint writeLength, byte[] writeBuffer, uint readStep, uint readTimes, ref uint readLength, byte[] readBuffer);
    [DllImport(dllName)]
    private static extern bool CH341SetStream(uint index, uint mode);
    [DllImport(dllName)]
    private static extern bool CH341SetDelaymS(uint index, uint delay);
    [DllImport(dllName)]
    private static extern bool CH341StreamI2C(uint index, uint writeLength, byte[] writeBuffer, uint readLength, byte[] readBuffer);
    [DllImport(dllName)]
    private static extern bool CH341GetInput(uint index, ref uint status);
    [DllImport(dllName)]
    private static extern bool CH341SetDeviceNotify(uint index, IntPtr deviceID, NotifyRoutine? notifyRoutine);
    [DllImport(dllName)]
    private static extern bool CH341SetIntRoutine(uint index, IntRoutine? intRoutine);
    [DllImport(dllName)]
    private static extern bool CH341SetupSerial(uint index, uint parityMode, uint baudRate);


    /// <summary>
    /// Set exclusive use of the current CH341 device.
    /// </summary>
    /// <param name="index">Specify the CH341 device number.</param>
    /// <param name="isExclusive">If the value is False, the device can be shared. If the value is not False, the device can be exclusively used</param>
    public static void SetExclusive(uint index, bool isExclusive)
    {
        if (!CH341SetExclusive(index, isExclusive))
            throw new Exception("Set exclusive error.");
    }

    /// <summary>
    /// Process I2C data stream, 2-wire interface, clock line for SCL pin, data line for SDA pin (quasi-bidirectional I/O), speed of about 56K bytes.
    /// </summary>
    /// <param name="index">Specify the CH341 device number.</param>
    /// <param name="writeLength">Number of bytes of data to write out.</param>
    /// <param name="writeBuffer">Buffer to place data to be written, usually with the I2C device address and read/write direction bits as the first byte.</param>
    /// <param name="readLength">Number of bytes of data to be read.</param>
    /// <param name="readBuffer">Buffer and returns the data read in.</param>
    public static void StreamI2C(uint index, uint writeLength, byte[] writeBuffer, uint readLength, byte[] readBuffer)
    {
        if (CH341StreamI2C(index, writeLength, writeBuffer, readLength, readBuffer))
            throw new Exception("StreamI2C error.");
    }

    /// <summary>
    /// Write data block to port 0#.
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device.</param>
    /// <param name="data">Buffer where the data to be written is placed</param>
    public static void WriteData0(uint index, byte[] data)
    {
        if (data.Length > maxBufferLength)
            throw new Exception("Overflow data write.");

        uint length = (uint)data.Length;
        byte[] buffer = new byte[length];

        if (!CH341WriteData0(index, buffer, ref length))
            throw new Exception("Write data at 0 error.");
    }

    /// <summary>
    /// Read data block from 0# port.
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device.</param>
    /// <param name="data">Buffer large enough to hold the read data.</param>
    public static byte[] ReadData0(uint index)
    {
        uint length = maxBufferLength;
        byte[] buffer = new byte[length];

        if (!CH341ReadData0(index, buffer, ref length))
            throw new Exception("Read data at 0 error.");

        return buffer;
    }

    /// <summary>
    /// Reset and initialize the parallel port, RST# outputs a low-level pulse.
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device.</param>
    /// <param name="mode">Specify parallel port mode: 0 is EPP mode/EPP mode V1.7, 1 is EPP mode V1.9, 2 is MEM mode, >= 0x00000100 keep current mode.</param>
    public static void InitParallel(uint index, ParallelMode mode)
    {
        if (CH341InitParallel(index, (uint)mode))
            throw new Exception("Set parallel port mode error.");
    }

    /// <summary>
    /// Set parallel port mode.
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device.</param>
    /// <param name="mode">Specify parallel port mode: 0 is EPP mode/EPP mode V1.7, 1 is EPP mode V1.9, 2 is MEM mode.</param>
    public static void SetParaMode(uint index, ParallelMode mode)
    {
        if (CH341SetParaMode(index, (uint)mode))
            throw new Exception("Set parallel port mode error.");
    }

    /// <summary>
    /// Abort interrupt data read operation.
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device.</param>
    public static void AbortInter(uint index)
    {
        if (!CH341AbortInter(index))
            throw new Exception("Abort inter error.");
    }

    /// <summary>
    /// Read interrupt data.
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device.</param>
    /// <returns>
    /// Double word location used to store the read interrupt status data, see below.
    /// Bit 7-bit 0 correspond to D7-D0 pins of CH341.
    /// Bit 8 corresponds to err# pin of CH341, bit 9 corresponds to PEMP pin of CH341, bit 10 corresponds to int# pin of CH341, and bit 11 corresponds to SLCT pin of CH341.
    /// </returns>
    public static uint ReadInter(uint index)
    {
        uint status = 0;
        if (!CH341ReadInter(index, ref status))
            throw new Exception("Read inter error.");
        return status;
    }

    /// <summary>
    /// Set interrupt service routine.
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device.</param>
    /// <param name="intRoutine">Specify the interrupt service routine, if it is NULL, the interrupt service will be cancelled, otherwise the routine will be called when interrupted.</param>
    public static void SetIntRoutine(uint index, IntRoutine? intRoutine)
    {
        if (!CH341SetIntRoutine(index, intRoutine))
            throw new Exception("Set int routine error.");
    }

    /// <summary>
    /// Reset USB device.
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device.</param>
    public static void ResetDevice(uint index)
    {
        if (!CH341ResetDevice(index))
            throw new Exception("Reset device error.");
    }

    /// <summary>
    /// Set up the device event notification procedure.
    /// </summary>
    /// <param name="index">Specify the CH341 device number,0 corresponds to the first device.</param>
    /// <param name="notifyRoutine">Specifies a device event callback that cancels event notification if NULL, or is called when an event is detected.</param>
    public static void SetDeviceNotify(uint index, NotifyRoutine? notifyRoutine)
    {
        if (!CH341SetDeviceNotify(index, IntPtr.Zero, notifyRoutine))
            throw new Exception("Set device notify error.");
    }

    /// <summary>
    /// Read one byte of data from the I2C interface.
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device.</param>
    /// <param name="slaveAddress">The lower 7 bits specify the I2C device address.</param>
    /// <param name="register">Address of specified data unit.</param>
    /// <returns>Address of specified data unit.</returns>
    public static byte ReadI2C(uint index, byte slaveAddress, byte register)
    {
        byte result = 0;
        if (!CH341ReadI2C(index, slaveAddress, register, ref result))
            throw new Exception("Read I2C error.");
        return result;
    }

    /// <summary>
    /// Set the hardware asynchronous delay to a specified number of milliseconds before the next stream operation.
    /// </summary>
    /// <param name="index">Specify the CH341 device number.</param>
    /// <param name="delay">Specifies the number of milliseconds to delay.</param>
    public static void SetDelay(uint index, uint delay)
    {
        if (!CH341SetDelaymS(index, delay))
            throw new Exception("Error setting the delay.");
    }

    /// <summary>
    /// Get the DLL version number, return the version number.
    /// </summary>
    public static uint GetDllVersion() => CH341GetVersion();

    /// <summary>
    /// Get the driver version number, return the version number, or return 0 if there is an error.
    /// Will return an error if it was called before the device was opened.
    /// </summary>
    public static uint GetDriverVersion()
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
    public static uint GetStatus(uint index)
    {
        uint status = 0;
        if (!CH341GetStatus(index, ref status))
            throw new Exception("Error getting result");
        return status;
    }

    /// <summary>
    /// Get the CH341 device name.
    /// </summary>
    /// <param name="index">Specify the CH341 device number,0 corresponds to the first device.</param>
    /// <returns>Returns the CH341 device name.</returns>
    public static string GetDeviceName(uint index)
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
    public static Handle OpenDevice(uint index)
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
    public static void CloseDevice(uint index) => CH341CloseDevice(index);

    /// <summary>
    /// Read device descriptor.
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device.</param>
    /// <returns>Descriptor.</returns>
    public static string GetDeviceDescr(uint index)
    {
        uint length = maxBufferLength;
        byte[] buffer = new byte[length];

        if (!CH341GetDeviceDescr(index, buffer, ref length))
            throw new Exception("It is not possible to get the value.");

        return Encoding.Unicode.GetString(buffer, 0, (int)length);
    }

    /// <summary>
    /// Read configuration descriptor.
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device.</param>
    /// <returns>Descriptor.</returns>
    public static string GetConfigDescr(uint index)
    {
        uint length = maxBufferLength;
        byte[] buffer = new byte[length];

        if (!CH341GetConfigDescr(index, buffer, ref length))
            throw new Exception("It is not possible to get the value.");

        return Encoding.Unicode.GetString(buffer, 0, (int)length);
    }

    /// <summary>
    /// Write a byte of data to the I2C interface.
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device.</param>
    /// <param name="slaveAddress">The lower 7 bits specify the I2C device address.</param>
    /// <param name="register">Address of specified data unit.</param>
    /// <param name="value">Byte data to be written.</param>
    public static void WriteI2C(uint index, byte slaveAddress, byte register, byte value)
    {
        if (!CH341WriteI2C(index, slaveAddress, register, value))
            throw new Exception("I2C write error.");
    }

    ///<summary>
    ///0= device invalid,0x10=CH341,0x20=CH341A.
    ///</summary>
    /// <param name="index">Specify the CH341 device number.</param>
    public static DeviceType GetDeviceVersion(uint index)
    {
        var result = CH341GetVerIC(index);
        if (result == 0)
            throw new Exception("Error determining device version.");
        return (DeviceType)result;
    }

    /// <summary>
    /// Using the CH341 to directly enter data and status is more efficient than using the <see cref="GetStatus(uint)"/>
    /// </summary>
    /// <param name="index">Specify the serial number of the CH341 device</param>
    /// <returns>
    /// Bit 7-bit 0 correspond to D7-D0 pins of CH341.
    /// Bit 8 corresponds to ERR# pin of CH341, Bit 9 corresponds to PEMP pin of CH341, Bit 10 corresponds to INT# pin of CH341, Bit 11 corresponds to SLCT pin of CH341, Bit 23 corresponds to SDA pin of CH341.
    /// Bit 13 corresponds to BUSY/WAIT# pin of CH341, Bit 14 corresponds to AUTOFD#/DATAS# pin of CH341, Bit 15 corresponds to SLCTIN#/ADDRS# pin of CH341.
    /// </returns>
    public static uint GetInput(uint index)
    {
        uint status = 0;
        if (!CH341GetInput(index, ref status))
            throw new Exception("Error getting result");

        return status;
    }

    /// <summary>
    /// Clear the buffer of CH341.
    /// </summary>
    /// <param name="index">Specify the CH341 device number.</param>
    public static void ClearBuffer(uint index)
    {
        if (!CH341FlushBuffer(index))
            throw new Exception("Buffer clearing error");
    }

    public enum DeviceType : uint
    {
        CH341A = 0x20,
        CH341A3 = 0x30,
        CH341 = 0x10,
        Unknown
    }

    public enum DeviceStatus : uint
    {
        Arrival = 3,
        Remove_Pend = 1,
        Remove = 0
    }

    public enum ParallelMode : uint
    {
        EPP = 0x00,
        EPP17 = 0x00,
        EPP19 = 0x01,
        MEM = 0x02,
        ECP = 0x03
    }

    public delegate void NotifyRoutine(DeviceStatus deviceStatus);
    public delegate void IntRoutine(uint status);
}
