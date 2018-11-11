using AquaAssist.CrossCutting.Constants;
using AquaAssist.CrossCutting.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AquaAssist.CrossCutting.Helpers
{
    public static class CurrentValueStatusExtension
    {
        public static SolidColorBrush GetColor(this CurrentValueStatus status)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(Mappings.StatusColorMapping[status]));
        }
    }
}
