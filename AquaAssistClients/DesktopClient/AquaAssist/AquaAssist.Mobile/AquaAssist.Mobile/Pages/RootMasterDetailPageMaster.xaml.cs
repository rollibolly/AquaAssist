using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AquaAssist.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public RootMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new RootMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class RootMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<RootMasterDetailPageMenuItem> MenuItems { get; set; }
            
            public RootMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<RootMasterDetailPageMenuItem>(new[]
                {
                    new RootMasterDetailPageMenuItem { Id = 0, Title = "Page 1" },
                    new RootMasterDetailPageMenuItem { Id = 1, Title = "Page 2" },
                    new RootMasterDetailPageMenuItem { Id = 2, Title = "Page 3" },
                    new RootMasterDetailPageMenuItem { Id = 3, Title = "Page 4" },
                    new RootMasterDetailPageMenuItem { Id = 4, Title = "Page 5" },
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}