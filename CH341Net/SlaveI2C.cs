using System;
using static CH341Net.CH341;
using static CH341Net.Device;

namespace CH341Net;

public class SlaveI2C : Slave
{
    public SlaveI2C(ref Device device, byte slaveAddress) : base(ref device, slaveAddress) { }
    public byte Read(byte register)
    {
        if (device.DeviceStatus != Status.Opened)
            throw new Exception("The device is not open.");

        return ReadI2C(device.Index, slaveAddress, register);
    }

    public void Write(byte register, byte value)
    {
        if (device.DeviceStatus != Status.Opened)
            throw new Exception("The device is not open.");

        WriteI2C(device.Index, slaveAddress, register, value);
    }
}