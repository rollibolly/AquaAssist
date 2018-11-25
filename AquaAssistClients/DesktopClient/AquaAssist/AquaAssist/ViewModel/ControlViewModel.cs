using AquaAssist.Communication;
using AquaAssist.CrossCutting.Models;
using AquaAssist.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AquaAssist.ViewModel
{
    public class ControlViewModel:BaseViewModel
    {
        private ObservableCollection<SwitchViewModel> relays;
        public ObservableCollection<SwitchViewModel> Relays
        {
            get
            {
                if (relays == null)
                    relays = new ObservableCollection<SwitchViewModel>();
                return relays;
            }
            set
            {
                relays = value;
                OnPropertyChanged(nameof(Relays));
            }
        }        

        public ControlViewModel()
        {
            Initialize();
        }

        public void Initialize()
        {
            LoadData();
        }

        private void LoadData()
        {
            BackgroundWorker loadWorker = new BackgroundWorker();
            loadWorker.RunWorkerCompleted += LoadWorker_RunWorkerCompleted;
            loadWorker.DoWork += LoadWorker_DoWork;
            IsLoading = true;
            loadWorker.RunWorkerAsync();
        }

        private void LoadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<RelayModel> result = null;
            while (true)
            {
                result = RestClient.GetRelays();
                if (result != null)
                    break;
                IsNetworkError = true;
                Thread.Sleep(1000);
            }
            IsNetworkError = false;
            e.Result = result;
        }

        private void LoadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<RelayModel> result = e.Result as List<RelayModel>;
            if (result != null)
            {
                Relays.Clear();
                foreach (RelayModel model in result)
                {
                    Relays.Add(new SwitchViewModel
                                {
                                    Relay = model
                                });
                }
            }            
        }
    }
}
