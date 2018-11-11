using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaAssist.Models
{
    public class SensorModel
    {
        private int id;
        private string sensorName;
        private List<SensorValueModel> values;
        private SensorValueLimitsModel sensorValueLimits;
        private string unit;
        private string description;

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string SensorName
        {
            get => sensorName;
            set => sensorName = value;            
        }

        public string Unit
        {
            get => unit;
            set => unit = value;
        }
        public string Description
        {
            get => description;
            set => description = value;
        }
        public SensorValueLimitsModel SensorValueLimits
        {
            get
            {
                if (sensorValueLimits == null)
                    sensorValueLimits = new SensorValueLimitsModel();
                return sensorValueLimits;
            }
            set => sensorValueLimits = value;
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
