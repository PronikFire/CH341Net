using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using static CH341Net.CH341;

namespace CH341Net;

public class Device
{
    public uint DriverVersion { get => GetDriverVersion(); }
    public string Name { get => GetDeviceName(index); }
    public string DeviceDescriptor { get => GetDeviceDescr(index); }
    public string ConfigDescriptor { get => GetConfigDescr(index); }
    public Status DeviceStatus { get; private set; }
    public DeviceType DeviceType { get => GetDeviceVersion(index); }
    public uint Index { get => index; }
    public bool IsExclusive 
    {
        get => isExclusive;
        set
        {
            SetExclusive(index, value);
            isExclusive = value;
        }
    }

    public event Action<DeviceStatus>? OnDeviceStatusChange;

    private Handle handle;
    private uint index;
    private bool isExclusive;

    #region Controll 

    public void Close()
    {
        if (DeviceStatus == Status.Closed)
            throw new Exception("Device was already closed.");

        SetDeviceNotify(index, null);
        CloseDevice(index);
        DeviceStatus = Status.Closed;
    }

    public void Reset()
    {
        if (DeviceStatus != Status.Opened)
            throw new Exception("The device is not open.");

        ResetDevice(index);
    }
    #endregion

    public bool GetPinState(uint index)
    {
        if (DeviceStatus != Status.Opened)
            throw new Exception("The device is not open.");
        if (index > 15)
            throw new Exception("There is no pin with this index.");

        return ((GetInput(this.index) >> (int)index) & 1) == 1;
    }

    private void DeviceStatusChange(DeviceStatus status)
    {
        if (OnDeviceStatusChange == null)
            return;

        OnDeviceStatusChange.Invoke(status);
    }

    public Device(uint index, bool isExclusive = false)
    {
        handle = OpenDevice(index);
        this.index = index;
        IsExclusive = isExclusive;
        SetDeviceNotify(index, DeviceStatusChange);
        DeviceStatus = Status.Opened; 
    }

    ~Device() => Close();

    public enum Status
    {
        Opened,
        Closed
    }
}
