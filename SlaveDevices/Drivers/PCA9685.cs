using System.Threading;
using CH341Net;

namespace SlaveDevices.Drivers;

public class PCA9685 : Slave.I2C
{
    public bool IsSleep
    {
        get => ((Read(0) >> 4) & 1) == 1;
        set => Write(0, (byte)(value ? (Read(0) | (1 << 4)) : (Read(0) & (~(1 << 4)))));
    }

    public PCA9685(ref Device master, byte slaveAddress = 0x40) : base(ref master, slaveAddress) { }

    public void Restart()
    {
        Write(0, (byte)(Read(0) | (1 << 7)));

        if (!IsSleep)
        {
            Thread.Sleep(1);
            return;
        }
        IsSleep = false;
        Thread.Sleep(1);
        IsSleep = true;
    }
}
