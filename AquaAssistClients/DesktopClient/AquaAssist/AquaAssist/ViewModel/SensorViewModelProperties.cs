using AquaAssist.CrossCutting.Constants;
using AquaAssist.CrossCutting.Enum;
using AquaAssist.CrossCutting.Helpers;
using AquaAssist.CrossCutting.Models;
using LiveCharts;
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
    public class SensorViewModelProperties: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private double currentValue;
        private SensorModel sensor;
        private List<string> labels;
        private int maxData = 12;
        private SensorValueLimitsModel limits;
        private Brush backgroundBrush;
        private double width = Diomensions.SENSOR_VIEW_WIDTH;
        private double height = Diomensions.SENSOR_VIEW_HEIGHT;        
        private bool isMaximized = false;
        private bool isLoading = false;

        private DateTime startDate = DateTime.Now.AddDays(-1);
        private DateTime endDate = DateTime.Now;

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
                OnPropertyChanged(nameof(Sensor));
            }
        }

        public SensorTypes SensorType { get; set; }

        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public bool IsMaximized
        {
            get => isMaximized;
            set
            {
                isMaximized = value;
                OnPropertyChanged(nameof(IsMaximized));
            }
        }

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
            get => Sensor.Description;
            set
            {
                Sensor.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string Unit
        {
            get => Sensor.Unit;
            set
            {
                Sensor.Unit = value;
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


        public SeriesCollection DisplaySeries { get; set; }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
