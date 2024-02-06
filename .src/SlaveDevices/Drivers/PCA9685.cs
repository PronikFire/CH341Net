using System.Threading;
using CH341Net;
using System;
using System.Reflection;

namespace SlaveDevices.Drivers;

/// <summary>
/// PWM driver PCA9685.
/// </summary>
public class PCA9685 : Slave.I2C
{
    /// <summary>
    /// Sleep mode.
    /// Reduces current consumption.
    /// </summary>
    public bool IsSleep
    {
        get => ((Read(0) >> 4) & 1) == 1;
        set => Write(0, (byte)(value ? (Read(0) | (1 << 4)) : (Read(0) & (~(1 << 4)))));
    }

    public PCA9685(ref Device master, byte slaveAddress = 0x40) : base(ref master, slaveAddress) { }

    /// <summary>
    /// To restart all of the previously active PWM channels.
    /// </summary>
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

    /// <summary>
    /// Set PWM for all channels.
    /// </summary>
    /// <param name="on">Step at which the PWM signal will be HIGH. (maximum 4095)</param>
    /// <param name="off">Step at which the PWM signal will be LOW. (maximum 4095)</param>
    public void SetPWMAll(ushort on, ushort off)
    {
        if (on > 4095)
            throw new Exception("Wrong on.");

        if (off > 4095)
            throw new Exception("Wrong off.");

        Write(250, (byte)on);
        Write(251, (byte)(on >> 8));
        Write(252, (byte)off);
        Write(253, (byte)(off >> 8));
    }

    /// <summary>
    /// Sets PWM for the channel.
    /// </summary>
    /// <param name="index">PWM channel index. (maximum 15)</param>
    /// <param name="fill">PWM fill percentage. (percentage, maximum 100)</param>
    public void SetPWM(byte index, float fill)
    {
        if (fill < 0 || fill > 100)
            throw new Exception("Wrong fill percent.");

        SetPWM(index, 0, (ushort)(4095 * (fill / 100f)));
    }

    /// <summary>
    /// Sets PWM for the channel.
    /// </summary>
    /// <param name="on">Step at which the PWM signal will be HIGH. (maximum 4095)</param>
    /// <param name="off">Step at which the PWM signal will be LOW. (maximum 4095)</param>
    public void SetPWM(byte index, ushort on, ushort off)
    {
        if (index > 15)
            throw new Exception("Wrong index.");

        if (on > 4095)
            throw new Exception("Wrong on.");

        if (off > 4095)
            throw new Exception("Wrong off.");

        Write((byte)((index * 4) + 6 + 0), (byte)on);
        Write((byte)((index * 4) + 6 + 1), (byte)(on >> 8));
        Write((byte)((index * 4) + 6 + 2), (byte)off);
        Write((byte)((index * 4) + 6 + 3), (byte)(off >> 8));
    }
}
