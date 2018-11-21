using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaAssist.CrossCutting.Helpers
{
    public static class DateTimeExtensions
    {
        public static string ToAquaAssistDateTimeString(this DateTime thisValue)
        {
            return thisValue.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
