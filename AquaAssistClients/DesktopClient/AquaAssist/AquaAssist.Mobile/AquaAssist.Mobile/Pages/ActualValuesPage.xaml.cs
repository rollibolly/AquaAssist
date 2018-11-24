
using AquaAssist.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AquaAssist.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActualValuesPage : ContentPage
	{
		public ActualValuesPage ()
		{
			InitializeComponent ();

            BindingContext = new ActualValuesViewModel();
		}
	}
}