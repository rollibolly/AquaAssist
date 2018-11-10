using AquaAssist.Models;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaAssist.ViewModel
{
    public class SensorViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private double lastLecture;
        private SensorModel sensor;
        private List<string> labels;
        private string unit;
        private string description;
        private int maxData = 20;

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string Unit
        {
            get => unit;
            set
            {
                unit = value;
                OnPropertyChanged(nameof(Unit));
            }
        }

        public int MaxData
        {
            get => maxData;
            set => maxData = value;
        }

        public List<string> Labels
        {

            get
            {
                if (labels == null)
                    labels = new List<string>();
                return labels;
            }
            set
            {
                labels = value;
                OnPropertyChanged(nameof(Labels));
            }
        }

              
        public SeriesCollection LastHourSeries { get; set; }
        

        public SensorViewModel()
        {
            LastHourSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>()
                }
            };            
        }

        public double LastLecture
        {
            get { return lastLecture; }
            set
            {
                lastLecture = value;
                OnPropertyChanged(nameof(LastLecture));
            }
        }

        public SensorModel Sensor
        {
            get
            {                
                if (sensor == null)
                    sensor = new SensorModel();
                return sensor;
            }
            set
            {
                sensor = value;                
            }
        }

        public void AddSensorValue(SensorValueModel val)
        {
            if (Sensor.Values.Count() == MaxData)
            {
                Sensor.Values.RemoveAt(0);
                LastHourSeries[0].Values.RemoveAt(0);
                Labels.RemoveAt(0);
            }
            Sensor.Values.Add(val);            
            LastHourSeries[0].Values.Add(new ObservableValue(val.Value));
            Labels.Add(val.Date.ToString("HH:mm:ss"));
            LastLecture = val.Value;
            OnPropertyChanged(nameof(Sensor));
            //OnPropertyChanged(nameof(Labels));
        }        

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
