using AquaAssist.CrossCutting.Constants;
using AquaAssist.CrossCutting.Enum;
using AquaAssist.CrossCutting.Helpers;
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
using System.Windows.Media;

namespace AquaAssist.ViewModel
{
    public class SensorViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private double currentValue;
        private SensorModel sensor;
        private List<string> labels;
        private string unit;
        private string description;

        private int maxData = 15;
        private SensorValueLimitsModel limits;

        private Dictionary<CurrentValueStatus, SolidColorBrush> colorsMap = new Dictionary<CurrentValueStatus, SolidColorBrush>
        {
            { CurrentValueStatus.OVER_CRITICAL, new SolidColorBrush((Color)ColorConverter.ConvertFromString(Mappings.StatusColorMapping[CurrentValueStatus.OVER_CRITICAL])) },
            { CurrentValueStatus.OVER_WARNING, new SolidColorBrush((Color)ColorConverter.ConvertFromString(Mappings.StatusColorMapping[CurrentValueStatus.OVER_WARNING])) },
            { CurrentValueStatus.OPTIMAL_HIGH,new SolidColorBrush((Color)ColorConverter.ConvertFromString(Mappings.StatusColorMapping[CurrentValueStatus.OPTIMAL_HIGH]))},
            { CurrentValueStatus.OPTIMAL, new SolidColorBrush((Color)ColorConverter.ConvertFromString(Mappings.StatusColorMapping[CurrentValueStatus.OPTIMAL]))},
            { CurrentValueStatus.OPTIMAL_LOW, new SolidColorBrush((Color)ColorConverter.ConvertFromString(Mappings.StatusColorMapping[CurrentValueStatus.OPTIMAL_LOW])) },
            { CurrentValueStatus.UNDER_WARNING, new SolidColorBrush((Color)ColorConverter.ConvertFromString(Mappings.StatusColorMapping[CurrentValueStatus.UNDER_WARNING])) },
            { CurrentValueStatus.UNDER_CRITICAL, new SolidColorBrush((Color)ColorConverter.ConvertFromString(Mappings.StatusColorMapping[CurrentValueStatus.UNDER_CRITICAL])) },
        };

        private Brush backgroundBrush;
        public Brush BackgroundBrush
        {
            get
            {
                if (backgroundBrush == null)
                    backgroundBrush = new SolidColorBrush(Color.FromArgb(200, 0, 244, 0));
                
                return backgroundBrush;
            }
            set
            {
                backgroundBrush = value;
                OnPropertyChanged(nameof(BackgroundBrush));
            }
        }

        public SensorValueLimitsModel Limits
        {
            get
            {
                if (limits == null)
                    limits = new SensorValueLimitsModel();
                return limits;
            }
            set
            {
                limits = value;
                OnPropertyChanged(nameof(Limits));
            }
        }

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

        public double CurrentValue
        {
            get { return currentValue; }
            set
            {
                currentValue = value;
                OnPropertyChanged(nameof(CurrentValue));
                                
                BackgroundBrush = colorsMap[Limits.GetStatusFromValue(currentValue)];                
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
            CurrentValue = val.Value;
            OnPropertyChanged(nameof(Sensor));
            //OnPropertyChanged(nameof(Labels));
        }        

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
