using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AquaAssist.Models
{
    [DataContract]
    public class SensorValueLimitsModel
    {
        [DataMember(Name = "CriticalHigh")]
        public double CriticalHigh { get; set; } = 30;
        [DataMember(Name = "OptimalHigh")]
        public double OptimalHigh { get; set; } = 26.5;
        [DataMember(Name = "OptimalLow")]
        public double OptimalLow { get; set; } = 24.5;
        [DataMember(Name = "CriticalLow")]
        public double CriticalLow { get; set; } = 22;        
    }
}
