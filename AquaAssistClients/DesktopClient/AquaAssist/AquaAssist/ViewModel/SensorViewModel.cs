using AquaAssist.Communication;
using AquaAssist.CrossCutting.Constants;
using AquaAssist.CrossCutting.Enum;
using AquaAssist.CrossCutting.Helpers;
using AquaAssist.CrossCutting.Models;
using AquaAssist.Utils;
using Hangfire;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private int maxData = 25;
        private SensorValueLimitsModel limits;
        private Brush backgroundBrush;
        private double width = Diomensions.SENSOR_VIEW_WIDTH;
        private double height = Diomensions.SENSOR_VIEW_HEIGHT;
        private ICommand expandCommand;
        private ICommand reloadCommand;
        private bool isMaximized = false;
        private bool isLoading = false;

        private DateTime startDate = DateTime.Now.AddDays(-1);
        private DateTime endDate = DateTime.Now;

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

        
        public ICommand ExpandCommand
        {
            get
            {
                return expandCommand ?? (expandCommand = new CommandHandler(() => ResizeView(), true));
            }            
        }

        public ICommand ReloadCommand
        {
            get
            {
                return reloadCommand ?? (reloadCommand = new CommandHandler(() => ReloadSensorData(), true));
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
                OnPropertyChanged(nameof(Sensor));
            }
        }
        
        public void Initialize()
        {
            ReloadSensorData();
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("Hello"), Cron.Minutely);
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

        private void ReloadSensorData()
        {
            BackgroundWorker initWorker = new BackgroundWorker();
            initWorker.DoWork += InitWorker_DoWork;
            initWorker.RunWorkerCompleted += InitWorker_RunWorkerCompleted;

            IsLoading = true;
            initWorker.RunWorkerAsync(new object[] { SensorType, StartDate, EndDate });
        }

        // Loads sensor definition from REST API
        private void InitWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = e.Argument as object[];

            SensorTypes type = (SensorTypes)args[0];
            DateTime start = (DateTime)args[1];
            DateTime end = (DateTime)args[2];
            SensorModel model = RestClient.GetSensorModel(type);
            if (model != null)
            {
                model.Values = RestClient.GetSensorValues(model.Id, start, end, MaxData);
            }
            e.Result = model;
        }

        // Sets sensor definition
        private void InitWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Sensor = e.Result as SensorModel;
            foreach (var item in Sensor.Values)
            {
                AddSensorValue(item);
            }
            IsLoading = false;
        }

        public void AddSensorValue(SensorValueModel val)
        {
            if (LastHourSeries[0].Values.Count == MaxData)
            {                
                LastHourSeries[0].Values.RemoveAt(0);
                Labels.RemoveAt(0);
            }            
            LastHourSeries[0].Values.Add(new ObservableValue(val.Value));
            Labels.Add($"{val.Date.ToString("yyyy/MM/dd")}\n{val.Date.ToString("mm:HH:ss")}");
            CurrentValue = val.Value;            
        }        



        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
