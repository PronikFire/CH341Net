using System;
using System.Reflection.Metadata;
using static CH341Net.CH341;

namespace CH341Net;

public class Device
{
    public string Name { get => GetDeviceName(index); }
    public string DeviceDescriptor { get => GetDeviceDescr(index); }
    public string ConfigDescriptor { get => GetConfigDescr(index); }
    public Status DeviceStatus { get; private set; } = Status.Unknow;
    public DeviceType DeviceType { get => GetDeviceVersion(index); }
    public uint Index { get => index; }

    private Handle handle;
    private uint index;

    public void WriteI2C(byte slave, byte register, byte value)
    {
        if (DeviceStatus != Status.Opened)
            throw new Exception("The device is not open.");

        CH341.WriteI2C(index, slave, register, value);
    }

    public byte ReadI2C(byte slave, byte register)
    {
        if (DeviceStatus != Status.Opened)
            throw new Exception("The device is not open.");

        return CH341.ReadI2C(index, slave, register);
    }

    public void Open(uint index)
    {
        if (DeviceStatus == Status.Opened)
            throw new Exception("Device was already opened.");
        handle = OpenDevice(index);
        this.index = index;
        DeviceStatus = Status.Opened;
    }

    public bool GetPinState(uint index)
    {
        if (DeviceStatus != Status.Opened)
            throw new Exception("The device is not open.");
        if (index > 15)
            throw new Exception("There is no pin with this index.");
        return GetInput(index).ToString("B")[(int)index] == '1';
    }

    public void Close()
    {
        if (DeviceStatus == Status.Closed)
            throw new Exception("Device was already closed.");
        CloseDevice(index);
        DeviceStatus = Status.Closed;
    }

    public void Reset()
    {
        if (DeviceStatus != Status.Opened)
            throw new Exception("Device was not opened.");

        ResetDevice(index);
    }

    ~Device() => Close();

    public enum Status
    {
        Opened,
        Closed,
        Unknow
    }
}
