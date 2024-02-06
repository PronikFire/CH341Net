using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using static CH341Net.CH341;

namespace CH341Net;

/// <summary>
/// Device CH341.
/// </summary>
public class Device
{
    /// <summary>
    /// Returns the driver version.
    /// </summary>
    public uint DriverVersion { get => GetDriverVersion(); }
    /// <summary>
    /// Returns the device name.
    /// </summary>
    public string Name { get => GetDeviceName(index); }
    /// <summary>
    /// Returns the device descriptor.
    /// </summary>
    public string DeviceDescriptor { get => GetDeviceDescr(index); }
    /// <summary>
    /// Returns the config descriptor.
    /// </summary>
    public string ConfigDescriptor { get => GetConfigDescr(index); }
    /// <summary>
    /// Return the device status.
    /// </summary>
    public Status DeviceStatus { get; private set; }
    /// <summary>
    /// Returns the device type
    /// </summary>
    public DeviceType DeviceType { get => GetDeviceVersion(index); }
    /// <summary>
    /// Returns the device index.
    /// </summary>
    public uint Index { get => index; }
    /// <summary>
    /// Sets the exclusivity of the device.
    /// If True, other processes will not be able to access this device.
    /// </summary>
    public bool IsExclusive 
    {
        get => isExclusive;
        set
        {
            SetExclusive(index, value);
            isExclusive = value;
        }
    }

    /// <summary>
    /// Called when the status of the device changes.
    /// (For some reason it doesn't work).
    /// </summary>
    [Obsolete]
    public event Action<DeviceStatus>? OnDeviceStatusChange;

    private Handle handle;
    private uint index;
    private bool isExclusive;

    #region Controll 
    /// <summary>
    /// Close device.
    /// </summary>
    public void Close()
    {
        if (DeviceStatus == Status.Closed)
            throw new Exception("Device was already closed.");

        SetDeviceNotify(index, null);
        CloseDevice(index);
        DeviceStatus = Status.Closed;
    }
    /// <summary>
    /// Restarting the device.
    /// </summary>
    public void Reset()
    {
        if (DeviceStatus != Status.Opened)
            throw new Exception("The device is not open.");

        ResetDevice(index);
    }
    #endregion

    /// <summary>
    /// Returns the pin status.
    /// </summary>
    /// <param name="index">Pin index.</param>
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
    /// <summary>
    /// Open device CH341.
    /// </summary>
    /// <param name="index">The index by which the device will be opened. Starts from 0.</param>
    /// <param name="isExclusive">Will the device be exclusive to this process.</param>
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
