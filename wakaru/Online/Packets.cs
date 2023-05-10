using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wakaru.Online
{
    enum Packets
    {
        MESSAGE = 1,
        REFRESH_SCHEDULE = 2,
        RECONNECT = 3,
        RING_CLASS_BEGIN = 4,
        RING_CLASS_OVER = 5
    };
}
