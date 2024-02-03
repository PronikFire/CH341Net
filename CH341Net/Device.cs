using System;
using System.Reflection.Metadata;

namespace CH341Net;

public class Device
{
    public string Name { get => CH341.GetDeviceName(index); }
    public ulong PinsStatus { get => CH341.GetStatus(index); }
    public Status DeviceStatus { get; private set; } = Status.Unknow;

    private Handle handle;
    private ulong index;

    public void OpenDevice(ulong index)
    {
        if (DeviceStatus == Status.Opened)
            throw new Exception("Device was already opened.");
        handle = CH341.OpenDevice(index);
        this.index = index;
        DeviceStatus = Status.Opened;
    }

    public void CloseDevice()
    {
        if (DeviceStatus == Status.Closed)
            throw new Exception("Device was already closed.");
        CH341.CloseDevice(index);
        DeviceStatus = Status.Closed;
    }

    public void ResetDevice()
    {
        if (DeviceStatus != Status.Opened)
            throw new Exception("Device was not opened.");

        CH341.ResetDevice(index);
    }

    ~Device()
    {
        CloseDevice();
    }

    public enum Status
    {
        Opened,
        Closed,
        Unknow
    }
}
