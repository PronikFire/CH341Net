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
        handle = CH341.OpenDevice(index);
        Console.WriteLine(handle.ToString());
        
        if ((int)handle.Kind == 127)
            throw new Exception("The device could not open.");

        this.index = index;
        OpenDevice();
    }
    public bool TryOpenDevice(ulong index)
    {
        handle = CH341.OpenDevice(index);
        if (handle.IsNil)
            return false;

        this.index = index;
        OpenDevice();
        return true;
    }

    private void OpenDevice()
    {
        DeviceStatus = Status.Opened;
    }

    public void CloseDevice()
    {
        CH341.CloseDevice(index);
        DeviceStatus = Status.Closed;
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
