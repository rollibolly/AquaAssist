
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace AquaAssist.Mobile.Droid
{
    [Activity(
        Label = "AquaAssist", 
        Icon = "@mipmap/icon", 
        Theme = "@style/MainTheme", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();

            LoadApplication(new App());
        }
    }
}