using AquaAssist.CrossCutting.Enum;

namespace AquaAssist.Mobile.ViewModels
{
    public class ActualValuesViewModel
    {
        public SensorViewModel TempAquariumViewModel { get; set; }
        public SensorViewModel TempOutsideViewModel { get; set; }
        public SensorViewModel FlowRateViewModel { get; set; }
        public SensorViewModel LightViewModel { get; set; }

        public ActualValuesViewModel()
        {
            TempAquariumViewModel = new SensorViewModel() { SensorType = SensorTypes.TemperatureAquarium};
            TempOutsideViewModel = new SensorViewModel() { SensorType = SensorTypes.TemperatureOutside };
            FlowRateViewModel = new SensorViewModel() { SensorType = SensorTypes.FlowRate };
            LightViewModel = new SensorViewModel() { SensorType = SensorTypes.Light };
        }
    }
}
