using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaAssist.Models
{
    public class SensorModel
    {
        private string sensorName;
        private List<SensorValueModel> values;

        public string SensorName
        {
            get => sensorName;
            set
            {
                sensorName = value;
            }
        }
        public List<SensorValueModel> Values
        {
            get
            {
                if (values == null)
                    values = new List<SensorValueModel>();
                return values;
            }
            set
            {
                values = value;
            }
        }
    }
}
