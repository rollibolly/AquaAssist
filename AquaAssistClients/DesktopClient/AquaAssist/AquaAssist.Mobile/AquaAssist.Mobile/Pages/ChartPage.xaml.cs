using AquaAssist.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AquaAssist.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChartPage : ContentPage
	{
		public ChartPage ()
		{
			InitializeComponent ();
            BindingContext = new ChartViewModel();
        }
	}
}