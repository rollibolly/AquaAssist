using AquaAssist.CrossCutting.Enum;

namespace AquaAssist.ViewModel
{
    public class MonitorizationViewModel
    {
        public SensorViewModel TemperatureSensor { get; set; }
        public SensorViewModel OutsideTemperatureSensor { get; set; }
        public SensorViewModel FlowRateSensor { get; set; }
        public SensorViewModel LightSensor { get; set; }
        
        public MonitorizationViewModel()
        {            
            TemperatureSensor = new SensorViewModel
            {                
                SensorType = SensorTypes.TemperatureAquarium,                
                
            };
            TemperatureSensor.Initialize();

            OutsideTemperatureSensor = new SensorViewModel
            {
                SensorType = SensorTypes.TemperatureOutside
            };
            OutsideTemperatureSensor.Initialize();

            FlowRateSensor = new SensorViewModel
            {
                SensorType = SensorTypes.FlowRate
            };
            FlowRateSensor.Initialize();

            LightSensor = new SensorViewModel
            {
                SensorType = SensorTypes.Light
            };
            LightSensor.Initialize();
        }        
    }
}
