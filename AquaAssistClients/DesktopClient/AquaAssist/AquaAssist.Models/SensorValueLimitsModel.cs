using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaAssist.Models
{
    public class SensorValueLimitsModel
    {
        public double CriticalHigh { get; set; } = 30;
        public double OptimalHigh { get; set; } = 26.5;
        public double OptimalLow { get; set; } = 24.5;
        public double CriticalLow { get; set; } = 22;        
    }
}
