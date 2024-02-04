using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH341Net;

public class Slave
{
    private Device device;

    public Slave(ref Device device)
    {
        this.device = device;
    }
}
