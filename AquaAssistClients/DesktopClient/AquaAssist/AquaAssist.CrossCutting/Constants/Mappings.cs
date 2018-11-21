using AquaAssist.CrossCutting.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AquaAssist.CrossCutting.Constants
{
    public static class Mappings
    {
        public static Dictionary<CurrentValueStatus, string> StatusColorMapping = new Dictionary<CurrentValueStatus, string>
        {            
            { CurrentValueStatus.OVER_CRITICAL, "#FF5722" },
            { CurrentValueStatus.OVER_WARNING, "#FF9800" },
            { CurrentValueStatus.OPTIMAL_HIGH, "#CDDC39" },
            { CurrentValueStatus.OPTIMAL, "#8BC34A" },
            { CurrentValueStatus.OPTIMAL_LOW, "#4CAF50" },
            { CurrentValueStatus.UNDER_WARNING, "#00BCD4" },
            { CurrentValueStatus.UNDER_CRITICAL, "#03A9F4" },
        };

        public static Dictionary<SensorTypes, int> SensorTypeSensorIdMapping = new Dictionary<SensorTypes, int>
        {
            { SensorTypes.TemperatureAquarium, 1 },
            { SensorTypes.FlowRate, 2},
            { SensorTypes.TemperatureOutside, 3},
            { SensorTypes.Light, 4},
        };
    }
}
