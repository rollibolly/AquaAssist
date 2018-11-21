using AquaAssist.CrossCutting.Enum;
using AquaAssist.CrossCutting.Models;

namespace AquaAssist.CrossCutting.Helpers
{
    public static class SensorValueLimitExtension
    {
        public static CurrentValueStatus GetStatusFromValue(this SensorValueLimitsModel limits, double value)
        {
            if (value > limits.OptimalHigh)
            {
                double half = (limits.CriticalHigh - limits.OptimalHigh) / 2;
                if (value >= limits.CriticalLow + half)
                    return CurrentValueStatus.OVER_CRITICAL;
                return CurrentValueStatus.OVER_WARNING;
            }
            if (value >= limits.OptimalLow && value <= limits.OptimalHigh)
            {
                double third = (limits.OptimalHigh - limits.OptimalLow) / 3;
                double midLow = limits.OptimalLow + third;
                double midHigh = midLow + third;

                if (value <= midLow)
                    return CurrentValueStatus.OPTIMAL_LOW;
                if (value <= midHigh)
                    return CurrentValueStatus.OPTIMAL;
                return CurrentValueStatus.OPTIMAL_HIGH;
            }
            if (value < limits.OptimalLow)
            {
                double half = (limits.OptimalLow - limits.CriticalLow) / 2;
                if (value <= limits.CriticalLow + half)
                    return CurrentValueStatus.UNDER_CRITICAL;
                return CurrentValueStatus.UNDER_WARNING;
            }
            return CurrentValueStatus.OPTIMAL;
        }
    }
}
