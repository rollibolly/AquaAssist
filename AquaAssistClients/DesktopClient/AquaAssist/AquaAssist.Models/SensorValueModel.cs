using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaAssist.Models
{
    public class SensorValueModel
    {
        private DateTime? date;
        private double? value;

        public DateTime Date
        {
            get => date ?? DateTime.Now;
            set
            {
                date = value;
            }
        }
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
