using AquaAssist.CrossCutting.Constants;
using AquaAssist.CrossCutting.Enum;
using System.Windows.Media;

namespace AquaAssist.Utils
{
    public static class Extensions
    {
        
        public static SolidColorBrush GetColor(this CurrentValueStatus status)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(Mappings.StatusColorMapping[status]));
        }
        
    }
}
