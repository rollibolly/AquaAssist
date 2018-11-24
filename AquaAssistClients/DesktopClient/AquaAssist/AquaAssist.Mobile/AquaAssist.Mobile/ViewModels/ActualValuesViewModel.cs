using AquaAssist.Communication;
using AquaAssist.CrossCutting.Models;
using System.Collections.Generic;

namespace AquaAssist.Mobile.ViewModels
{
    public class ActualValuesViewModel
    {
        public List<SensorModel> SensorModels;

        public ActualValuesViewModel()
        {
            SensorModels = RestClient.GetSensorModels();
        }
    }
}
