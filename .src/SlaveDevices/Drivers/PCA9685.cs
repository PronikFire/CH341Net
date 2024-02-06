using System.Threading;
using CH341Net;
using System;
using System.Reflection;
using System.ComponentModel;
using System.Threading.Channels;

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

    #region SetChannelStatus
    /// <summary>
    /// Sets the channel status.
    /// </summary>
    /// <param name="index">Channel index.</param>
    /// <param name="status">Сhannel status.</param>
    public void SetChannelStatus(byte index, ChannelStatus status)
    {
        if (index > 15)
            throw new Exception("Wrong index.");

        SetAddressChannelsStatus(index, status);
    }

    /// <summary>
    /// Sets the all channels status.
    /// </summary>
    /// <param name="status">Specified channels status.</param>
    public void SetAllChannelsStatus(ChannelStatus status) => SetAddressChannelsStatus(250, status);
    #endregion

    #region GetChannelStatus
    /// <summary>
    /// Returns the channel status.
    /// </summary>
    /// <param name="index">Channel index</param>
    /// <returns>Returns the status of the specified channel.</returns>
    public ChannelStatus GetChannelStatus(byte index)
    {
        if (index > 15)
            throw new Exception("Wrong index.");

        var on = (Read((byte)(((index * 4) + 6) + 1)) >> 4) == 1;
        var off = (Read((byte)(((index * 4) + 6) + 3)) >> 4) == 1;

        if (on & on == off )
        {
            SetChannelStatus(index, ChannelStatus.PWM);
            return ChannelStatus.PWM;
        }

        if (on)
            return ChannelStatus.AlwaysOn;
        
        if (off)
            return ChannelStatus.AlwaysOff;

        return ChannelStatus.PWM;

    }
    #endregion

    #region SetPWM
    /// <summary>
    /// Set PWM for all channels.
    /// </summary>
    /// <param name="fill">PWM fill percentage. (percentage, maximum 100)</param>
    public void SetPWMAll(float fill)
    {
        if (fill < 0 || fill > 100)
            throw new Exception("Wrong fill percent.");


        if (fill == 100)
        {
            SetAllChannelsStatus(ChannelStatus.AlwaysOn);
            return;
        }

        if (fill == 0)
        {
            SetAllChannelsStatus(ChannelStatus.AlwaysOff);
            return;
        }

        SetAllChannelsStatus(ChannelStatus.PWM);
        SetPWMAll(0, (ushort)(4095 * (fill / 100f)));
    }

    /// <summary>
    /// Set PWM for all channels.
    /// </summary>
    /// <param name="on">Step at which the PWM signal will be HIGH. (maximum 4095)</param>
    /// <param name="off">Step at which the PWM signal will be LOW. (maximum 4095)</param>
    public void SetPWMAll(ushort on, ushort off)
    {
        SetAddressPWM(250, on, off);
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

        if (fill == 100)
        {
            SetChannelStatus(index, ChannelStatus.AlwaysOn);
            return;
        }

        if (fill == 0)
        {
            SetChannelStatus(index, ChannelStatus.AlwaysOff);
            return;
        }

        SetChannelStatus(index, ChannelStatus.PWM);
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

        SetAddressPWM((byte)((index * 4) + 6), on, off);
    }
    #endregion

    #region GetPWM
    /// <summary>
    /// Get values from PWM channel.
    /// </summary>
    /// <param name="index">Channel index.</param>
    /// <param name="on">Step at which the PWM signal will be HIGH. (maximum 4095)</param>
    /// <param name="off">Step at which the PWM signal will be LOW. (maximum 4095)</param>
    public void GetPWM(byte index, out ushort on, out ushort off)
    {
        if (index > 15)
            throw new Exception("Wrong index.");

        on = (ushort)(((Read((byte)(((index * 4) + 6) + 1)) & 0b1111) << 8) | Read((byte)(((index * 4) + 6) + 0)));
        off = (ushort)(((Read((byte)(((index * 4) + 6) + 3)) & 0b1111) << 8) | Read((byte)(((index * 4) + 6) + 2)));
    }
    #endregion

    private void SetAddressPWM(byte startAddress, ushort on, ushort off)
    {
        if (on > 4095)
            throw new Exception("Wrong on.");

        if (off > 4095)
            throw new Exception("Wrong off.");

        Write((byte)(startAddress + 0), (byte)on);
        Write((byte)(startAddress + 1), (byte)(on >> 8));
        Write((byte)(startAddress + 2), (byte)off);
        Write((byte)(startAddress + 3), (byte)(off >> 8));
    }

    private void SetAddressChannelsStatus(byte startAddress, ChannelStatus status)
    {
        switch (status)
        {
            case ChannelStatus.AlwaysOn:
                Write((byte)(startAddress + 1), (byte)(Read((byte)(startAddress + 1)) | (1 << 4)));
                break;

            case ChannelStatus.AlwaysOff:
                Write((byte)(startAddress + 3), (byte)(Read((byte)(startAddress + 3)) | (1 << 4)));
                break;

            case ChannelStatus.PWM:
                Write((byte)(startAddress + 1), (byte)(Read((byte)(startAddress + 1)) & (~(1 << 4))));
                Write((byte)(startAddress + 3), (byte)(Read((byte)(startAddress + 3)) & (~(1 << 4))));
                break;
        }
    }

    public enum ChannelStatus : byte
    {
        AlwaysOn,
        AlwaysOff,
        PWM
    }
}
