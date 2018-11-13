using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AquaAssist.Models
{
    [DataContract]
    public class SensorValueModel
    {
        private DateTime? date;
        private double? value;

        [IgnoreDataMember]
        public DateTime Date
        {
            get => date ?? DateTime.Now;
            set
            {
                date = value;
            }
        }

        //[DataMember(Name = "Date")]
        [IgnoreDataMember]
        public string DateStr
        {
            // "2018-11-11 16:20:00.91647+02"
            get => Date.ToString("yyyy-MM-dd HH:mm:ss.FFFFzz");
            set => date = DateTime.Parse(value);
        }

        [DataMember(Name = "Value")]
        public double Value
        {
            get => value ?? 0;
            set
            {
                this.value = value;
            }
        }
    }
}
