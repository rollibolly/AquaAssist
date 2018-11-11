using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AquaAssist.Models
{
    [DataContract]
    public class SensorModel
    {
        private int id;
        private string sensorName;
        private List<SensorValueModel> values;
        private SensorValueLimitsModel sensorValueLimits;
        private string unit;
        private string description;

        [DataMember(Name = "Id")]
        public int Id
        {
            get => id;
            set => id = value;
        }

        [DataMember(Name = "SensorName")]
        public string SensorName
        {
            get => sensorName;
            set => sensorName = value;            
        }

        [DataMember(Name = "Unit")]
        public string Unit
        {
            get => unit;
            set => unit = value;
        }

        [DataMember(Name = "Description")]
        public string Description
        {
            get => description;
            set => description = value;
        }

        [DataMember(Name = "SensorValueLimits")]
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
        [IgnoreDataMember]
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
