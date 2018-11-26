using AquaAssist.ViewModel;
using System.Windows.Controls;

namespace AquaAssist.View
{
    /// <summary>
    /// Interaction logic for MonitorizationView.xaml
    /// </summary>
    public partial class MonitorizationView : UserControl
    {
        public MonitorizationView()
        {
            InitializeComponent();                        
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = new MonitorizationViewModel();
        }
    }
}
