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
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;

namespace AquaAssist.ViewModel
{

    public class SensorViewModel:SensorViewModelProperties
    {
        private ICommand expandCommand;
        private ICommand reloadCommand;
        private System.Timers.Timer last24Timer;

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
                return reloadCommand ?? (reloadCommand = new CommandHandler(() => InitSensorData(), true));
            }
        }

        public SensorViewModel()
        {            
            DisplaySeries = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>()
                }
            };            
        }        
                
        public void Initialize()
        {
            InitSensorData();            
        }

        private void InitSensorData()
        {
            BackgroundWorker initWorker = new BackgroundWorker();
            initWorker.DoWork += InitWorker_DoWork;
            initWorker.RunWorkerCompleted += InitWorker_RunWorkerCompleted;

            IsLoading = true;
            initWorker.RunWorkerAsync(SensorType);
        }

        // Loads sensor definition from REST API
        private void InitWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SensorTypes type = (SensorTypes)e.Argument;
            SensorModel model = null;
            while (true)
            {
                model = RestClient.GetSensorModel(type);
                if (model != null)
                    break;
                IsNetworkError = true;
                Thread.Sleep(1000);

            }
            IsNetworkError = false;
            e.Result = model;
        }

        // Sets sensor definition
        private void InitWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            Sensor = e.Result as SensorModel;
            
            Last24Timer_Elapsed(null, null);
            last24Timer = new System.Timers.Timer(1000);
            last24Timer.Elapsed += Last24Timer_Elapsed;
            last24Timer.AutoReset = true;
            last24Timer.Enabled = true;
            IsLoading = false;
        }

        private void Last24Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            BackgroundWorker last24Worker = new BackgroundWorker();
            last24Worker.DoWork += Last24Worker_DoWork;
            last24Worker.RunWorkerCompleted += Last24Worker_RunWorkerCompleted;
            
            last24Worker.RunWorkerAsync(new SensorQuery
            {
                Id = Sensor.Id,                
                MaxData = MaxData
            });
        }

        private void Last24Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            SensorQuery query = e.Argument as SensorQuery;
            List<SensorValueModel> result = null;

            while (true)
            {
                result = RestClient.GetSensorValues(query.Id, query.MaxData);
                if (result != null)
                    break;
                IsNetworkError = true;
                Thread.Sleep(1000);
            }
            IsNetworkError = false;
            e.Result = result;
        }

        private void Last24Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<SensorValueModel> s = e.Result as List<SensorValueModel>;
            s.Reverse();
            foreach (var item in s)
            {
                AddSensorValue(item);
            }
            LastUpdateTs = DateTime.Now;
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

        

        public void AddSensorValue(SensorValueModel val)
        {
            if (Sensor.Values.Where(x => x.Date == val.Date && x.Value == val.Value).FirstOrDefault() == null)
            {
                ObservableValue newValue = new ObservableValue(val.Value);

                if (DisplaySeries[0].Values.Count == MaxData)
                {
                    DisplaySeries[0].Values.RemoveAt(0);
                    Sensor.Values.RemoveAt(0);
                    Labels.RemoveAt(0);
                }
                DisplaySeries[0].Values.Add(newValue);
                Sensor.Values.Add(val);
                Labels.Add($"{val.Date.ToString("hh:mm:ss")}");
                CurrentValue = val.Value;
            }
        }                
    }
}
