using System;

namespace AquaAssist.Mobile.Pages
{

    public class RootMasterDetailPageMenuItem
    {
        public RootMasterDetailPageMenuItem()
        {
            TargetType = typeof(RootMasterDetailPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}