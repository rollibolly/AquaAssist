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
        public bool Description { get; set; }
        [DataMember(Name = "LastStatusChange")]
        public DateTime LastStatusChange { get; set; }
    }
}
