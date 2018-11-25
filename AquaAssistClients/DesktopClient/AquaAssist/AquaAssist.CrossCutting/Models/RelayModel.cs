using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AquaAssist.CrossCutting.Models
{
    [DataContract]
    public class RelayModel
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "State")]
        public bool State { get; set; }

        [DataMember(Name = "DefaultState")]
        public bool DefaultState { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }        


        private DateTime? lastStatusChange;
        [IgnoreDataMember]
        public DateTime LastStatusChange
        {
            get => lastStatusChange ?? DateTime.Now;
            set
            {
                lastStatusChange = value;
            }
        }
        [DataMember(Name = "LastStatusChange")]
        public string LastStatusChangeStr
        {
            get
            {
                return LastStatusChange.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                LastStatusChange = DateTime.Parse(value);
            }
        }
    }
}
