using System;

namespace CH341Net;

public class Slave
{
    protected private Device device;
    protected private byte slaveAddress;

    protected private Slave(ref Device device, byte slaveAddress)
    {
        this.device = device;
        this.slaveAddress = slaveAddress;
    }
}
