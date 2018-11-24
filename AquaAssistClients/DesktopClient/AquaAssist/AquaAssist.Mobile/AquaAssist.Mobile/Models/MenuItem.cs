using System;

namespace AquaAssist.Mobile.Models
{

    public class MenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }

        public Type TargetType { get; set; }
    }
}