using AquaAssist.Communication;
using AquaAssist.CrossCutting.Models;
using AquaAssist.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AquaAssist.ViewModel
{
    public class SwitchViewModel : BaseViewModel
    {
        private RelayModel relay;
        public RelayModel Relay
        {
            get { return relay; }
            set
            {
                relay = value;
                OnPropertyChanged(nameof(Relay));
            }
        }

        private ICommand changeStateCommand;
        public ICommand ChangeStateCommand
        {
            get
            {
                return changeStateCommand ?? (changeStateCommand = new CommandHandler(() => ChangeRelayState(), true));
            }
        }

        private void ChangeRelayState()
        {
            BackgroundWorker changeStateWorker = new BackgroundWorker();
            changeStateWorker.RunWorkerCompleted += ChangeStateWorker_RunWorkerCompleted;
            changeStateWorker.DoWork += ChangeStateWorker_DoWork;
            IsLoading = true;
            changeStateWorker.RunWorkerAsync(Relay);            
        }

        private void ChangeStateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            RelayModel relay = e.Argument as RelayModel;
            if (relay != null)
            {
                relay.State = !relay.State;
                relay = RestClient.UpdateRelay(relay);
            }
            e.Result = relay;
        }

        private void ChangeStateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsLoading = false;
            RelayModel result = e.Result as RelayModel;
            if (result != null)
            {
                Relay = result;
            }
        }
    }
}
