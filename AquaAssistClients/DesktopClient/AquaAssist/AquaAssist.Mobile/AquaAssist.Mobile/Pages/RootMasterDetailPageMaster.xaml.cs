using AquaAssist.Mobile.ViewModels;

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

            BindingContext = new MainViewModel();
            ListView = MenuItemsListView;
        }
    }
}