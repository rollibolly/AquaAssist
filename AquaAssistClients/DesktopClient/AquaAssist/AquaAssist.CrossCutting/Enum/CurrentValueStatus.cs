using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaAssist.CrossCutting.Enum
{
    public enum CurrentValueStatus
    {
        OVER_CRITICAL,
        OVER_WARNING,
        OPTIMAL_HIGH,
        OPTIMAL,
        OPTIMAL_LOW,
        UNDER_WARNING,
        UNDER_CRITICAL
    }
}
