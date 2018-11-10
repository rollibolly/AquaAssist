using AquaAssist.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AquaAssist.ViewModel
{
    public class MonitorizationViewModel
    {
        public SensorViewModel TemperatureSensor { get; set; }
        public SensorViewModel OutsideTemperatureSensor { get; set; }
        public SensorViewModel FlowRateSensor { get; set; }
        public SensorViewModel LightSensor { get; set; }

        public Random rnd = new Random();          

        public MonitorizationViewModel()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += Bw_DoWork;


            TemperatureSensor = new SensorViewModel
            {
                Sensor = new SensorModel()
                {
                    SensorName = "Temperature",
                },
                Unit = "C",
                Description = "Temperature of the aquarium."
            };
            OutsideTemperatureSensor = new SensorViewModel
            {
                Sensor = new SensorModel()
                {
                    SensorName = "Outside Temperature",
                },
                Unit = "C",
                Description = "Temperature of the room."
            };

            bw.RunWorkerAsync();

            FlowRateSensor = new SensorViewModel
            {
                Sensor = new SensorModel { SensorName = "Flow Rate" },
                Unit = "L/h",
                Description = "Flow rate of the canister filter."
            };


            LightSensor = new SensorViewModel
            {
                Sensor = new SensorModel { SensorName = "Light" },
                Unit = "LUX",
                Description = "The light sensor."
            };
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                TemperatureSensor.AddSensorValue(new SensorValueModel { Date = DateTime.Now, Value = rnd.NextDouble() * 10 + 20 });
                FlowRateSensor.AddSensorValue(new SensorValueModel { Date = DateTime.Now, Value = rnd.NextDouble() * 20 + 600 });
                Thread.Sleep(5000);
            }
        }
    }
}
