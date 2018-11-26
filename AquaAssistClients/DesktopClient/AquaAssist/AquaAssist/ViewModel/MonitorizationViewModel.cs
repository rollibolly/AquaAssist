using AquaAssist.Communication;
using AquaAssist.CrossCutting.Enum;
using AquaAssist.CrossCutting.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace AquaAssist.ViewModel
{
    public class MonitorizationViewModel:BaseViewModel
    {        
        public ObservableCollection<SensorViewModel> Sensors { get; set; }
        
        
        public MonitorizationViewModel()
        {
             Sensors = new ObservableCollection<SensorViewModel>();
            LoadSensorDefinitions();
        }        

        private void LoadSensorDefinitions()
        {
            BackgroundWorker loadSensorsWorker = new BackgroundWorker();
            loadSensorsWorker.RunWorkerCompleted += LoadSensorsWorker_RunWorkerCompleted;
            loadSensorsWorker.DoWork += LoadSensorsWorker_DoWork;

            IsLoading = true;
            loadSensorsWorker.RunWorkerAsync();
        }

        private void LoadSensorsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<SensorModel> result = null;
            while (true)
            {
                result = RestClient.GetSensorModels();
                if (result != null)
                    break;
                IsNetworkError = true;
                Thread.Sleep(1000);
            }
            e.Result = result;
        }

        private void LoadSensorsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsLoading = false;
            List<SensorModel> models = e.Result as List<SensorModel>;
            if (models != null)
            {
                foreach (SensorModel model in models)
                {
                    SensorViewModel viewModel = new SensorViewModel
                    {
                        Sensor = model
                    };                    
                    Sensors.Add(viewModel);
                    OnPropertyChanged(nameof(Sensors));
                    viewModel.Initialize();
                }
            }
        }
    }
}
