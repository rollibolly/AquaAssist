using AquaAssist.Communication;
using AquaAssist.CrossCutting.Enum;
using AquaAssist.CrossCutting.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AquaAssist.Mobile.ViewModels
{
    public class SensorViewModel : INotifyPropertyChanged
    {
        private const int LastValue = 1;
        private SensorModel sensorModel;
        private string actualValue;

        public SensorTypes SensorType { get; set; }

        public SensorModel SensorModel
        {
            get => sensorModel;
            set
            {
                if (value != sensorModel)
                {
                    sensorModel = value;
                    OnPropertyChanged(nameof(SensorModel));
                }
            }
        } 

        public string ActualValue
        {
            get => actualValue;
            set
            {
                if (value != actualValue)
                {
                    actualValue = value;
                    OnPropertyChanged(nameof(ActualValue));
                }
            }
        }

        public SensorViewModel()
        {
            BackgroundWorker requestData = new BackgroundWorker();
            requestData.DoWork += RequestData_DoWork;
            requestData.RunWorkerCompleted += RequestData_RunWorkerCompleted;
            requestData.RunWorkerAsync();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void RequestData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SensorModel = e.Result as SensorModel;

            BackgroundWorker actualValue = new BackgroundWorker();
            actualValue.DoWork += ActualValue_DoWork;
            actualValue.RunWorkerCompleted += ActualValue_RunWorkerCompleted;
            actualValue.RunWorkerAsync(SensorModel.Id);
        }

        private void ActualValue_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ActualValue = $"{Math.Round((e.Result as List<SensorValueModel>).FirstOrDefault().Value, 2).ToString()}";
        }

        private void ActualValue_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = RestClient.GetSensorValues((int)e.Argument, LastValue);
        }

        private void RequestData_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = RestClient.GetSensorModel(SensorType);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
