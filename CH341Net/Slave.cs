using System;
using static CH341Net.Device;
using static CH341Net.CH341;

namespace CH341Net;

public class Slave
{
    protected Device device;
    protected byte slaveAddress;

    protected Slave(ref Device master, byte slaveAddress)
    {
        device = master;
        this.slaveAddress = slaveAddress;
    }

    public class I2C : Slave
    {
        public I2C(ref Device master, byte slaveAddress) : base(ref master, slaveAddress) { }
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
}
