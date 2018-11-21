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

namespace AquaAssist.ViewModel
{

    public class SensorViewModel:SensorViewModelProperties
    {
        private ICommand expandCommand;
        private ICommand reloadCommand;
        private Timer last24Timer;

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
            ReloadSensorData();

            last24Timer = new Timer(1000);
            last24Timer.Elapsed += Last24Timer_Elapsed;
            last24Timer.AutoReset = true;
            last24Timer.Enabled = true;
        }

        private void Last24Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            BackgroundWorker last24Worker = new BackgroundWorker();
            last24Worker.DoWork += Last24Worker_DoWork;
            last24Worker.RunWorkerCompleted += Last24Worker_RunWorkerCompleted;
            return;
            last24Worker.RunWorkerAsync(new SensorQuery
            {
                Type = SensorType,
                EndDate = DateTime.Now,
                StartDate = DateTime.Now.AddHours(-1),
                MaxData = MaxData
            });
        }

        private void Last24Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Sensor = e.Result as SensorModel;
            foreach (var item in Sensor.Values)
            {
                AddSensorValue(item);
            }
        }

        private void Last24Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            SensorQuery query = e.Argument as SensorQuery;
            
            SensorModel model = RestClient.GetSensorModel(query.Type);
            if (model != null)
            {
                model.Values = RestClient.GetSensorValues(model.Id, query.StartDate, query.EndDate, query.MaxData);
            }
            e.Result = model;
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
            SensorModel sensor = e.Result as SensorModel;
            foreach (var item in sensor.Values)
            {
                AddSensorValue(item);
            }
            IsLoading = false;
        }

        public void AddSensorValue(SensorValueModel val)
        {
            //if (Sensor.Values.Where(x => x.Date == val.Date).FirstOrDefault() == null)
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
                Labels.Add($"{val.Date.ToString("yyyy/MM/dd")}\n{val.Date.ToString("mm:HH:ss")}");
                CurrentValue = val.Value;
            }
        }                
    }
}
