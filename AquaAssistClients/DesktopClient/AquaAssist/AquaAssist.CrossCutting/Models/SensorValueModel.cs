using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace AquaAssist.CrossCutting.Models
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
        [DataMember(Name ="Date")]
        public string DateStr
        {
            get
            {
                return Date.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                Date = DateTime.Parse(value);
            }
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
