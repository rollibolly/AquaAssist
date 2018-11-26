using AquaAssist.Mobile.Models;
using AquaAssist.Mobile.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AquaAssist.Mobile.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; }

        public MainViewModel()
        {
            MenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem { Id = 0, Title = "Actual values", Icon = "actual_values.png", TargetType = typeof(ActualValuesPage) },
                new MenuItem { Id = 1, Title = "Statistics", Icon = "statistics_1.png", TargetType = typeof(ChartPage) },
                new MenuItem { Id = 2, Title = "Schedule", Icon = "schedule_1.png", TargetType = typeof(RootMasterDetailPageDetail) },
                new MenuItem { Id = 3, Title = "Control", Icon = "control.png", TargetType = typeof(RootMasterDetailPageDetail) },
                new MenuItem { Id = 4, Title = "Live view", Icon = "live_view.png", TargetType = typeof(RootMasterDetailPageDetail) },
                new MenuItem { Id = 5, Title = "Settings", Icon = "settings.png", TargetType = typeof(RootMasterDetailPageDetail) }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
