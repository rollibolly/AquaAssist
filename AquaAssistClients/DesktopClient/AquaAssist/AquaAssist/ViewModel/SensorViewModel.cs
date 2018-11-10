using AquaAssist.CrossCutting.Constants;
using AquaAssist.CrossCutting.Enum;
using AquaAssist.CrossCutting.Helpers;
using AquaAssist.Models;
using AquaAssist.Utils;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private Brush backgroundBrush;
        private double width = Diomensions.SENSOR_VIEW_WIDTH;
        private double height = Diomensions.SENSOR_VIEW_HEIGHT;
        private ICommand expandCommand;

        public bool IsMaximized { get; set; } = false;

        public double Width
        {
            get => width;
            set
            {
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }
        public double Height
        {
            get => height;
            set
            {
                height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private Dictionary<CurrentValueStatus, SolidColorBrush> colorsMap = new Dictionary<CurrentValueStatus, SolidColorBrush>
        {
            { CurrentValueStatus.OVER_CRITICAL, CurrentValueStatus.OVER_CRITICAL.GetColor() },
            { CurrentValueStatus.OVER_WARNING, CurrentValueStatus.OVER_WARNING.GetColor() },
            { CurrentValueStatus.OPTIMAL_HIGH, CurrentValueStatus.OPTIMAL_HIGH.GetColor() },
            { CurrentValueStatus.OPTIMAL, CurrentValueStatus.OPTIMAL.GetColor() },
            { CurrentValueStatus.OPTIMAL_LOW,  CurrentValueStatus.OPTIMAL_LOW.GetColor() },
            { CurrentValueStatus.UNDER_WARNING,  CurrentValueStatus.UNDER_WARNING.GetColor() },
            { CurrentValueStatus.UNDER_CRITICAL,  CurrentValueStatus.UNDER_CRITICAL.GetColor() },
        };

        
        public ICommand ExpandCommand
        {
            get
            {
                return expandCommand ?? (expandCommand = new CommandHandler(() => ResizeView(), true));
            }            
        }


        public Brush BackgroundBrush
        {
            get
            {
                if (backgroundBrush == null)
                    backgroundBrush = CurrentValueStatus.OPTIMAL.GetColor();
                
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

        private void ResizeView()
        {
            if (IsMaximized)
            {
                Width = Diomensions.SENSOR_VIEW_WIDTH;
                Height = Diomensions.SENSOR_VIEW_HEIGHT;
                IsMaximized = false;
            }
            else
            {
                int widthMultiplier = (int)((System.Windows.SystemParameters.PrimaryScreenWidth - 100) / Diomensions.SENSOR_VIEW_WIDTH);
                int heightMultiplier = (int)((System.Windows.SystemParameters.PrimaryScreenHeight - 200) / Diomensions.SENSOR_VIEW_HEIGHT);
                Width = widthMultiplier * Diomensions.SENSOR_VIEW_WIDTH;
                Height = System.Windows.SystemParameters.PrimaryScreenHeight - 200;
                IsMaximized = true;
            }
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
        }        

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
