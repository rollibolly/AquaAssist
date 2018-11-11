using AquaAssist.Mobile.ViewModels;
using OxyPlot;
using OxyPlot.Xamarin.Forms;
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