using AquaAssist.Utils;
using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AquaAssist.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public List<AccentColorMenuData> AccentColors { get; set; }
        public List<AccentColorMenuData> AppThemes { get; set; }

        private AccentColorMenuData selectedAccentColor;
        public AccentColorMenuData SelectedAccentColor
        {
            get => selectedAccentColor;
            set
            {
                selectedAccentColor = value;
                var theme = ThemeManager.DetectAppStyle();
                ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(selectedAccentColor.Name), theme.Item1);                
                OnPropertyChanged(nameof(SelectedAccentColor));
            }
        }

        private AccentColorMenuData selectedTheme;
        public AccentColorMenuData SelectedTheme
        {
            get => selectedTheme;
            set
            {
                selectedTheme = value;
                var theme = ThemeManager.DetectAppStyle();
                ThemeManager.ChangeAppTheme(Application.Current, selectedTheme.Name);                
                OnPropertyChanged(nameof(SelectedTheme));
            }
        }

        private bool isSettingsFlyoutOppen;
        public bool IsSettingFlyoutOpen
        {
            get => isSettingsFlyoutOppen;
            set
            {
                isSettingsFlyoutOppen = value;
                OnPropertyChanged(nameof(IsSettingFlyoutOpen));
            }
        }

        private ICommand settingsCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SettingsCommand
        {
            get
            {
                return settingsCommand ?? (settingsCommand = new CommandHandler(() => ShowSettings(), true));
            }
        }

        public MainWindowViewModel()
        {
            this.AccentColors = ThemeManager.Accents.Select(x => new AccentColorMenuData { ColorBrush = x.Resources["AccentBaseColorBrush"] as Brush, Name = x.Name }).ToList();
            this.AppThemes = ThemeManager.AppThemes.Select(x => new AccentColorMenuData { Name = x.Name, BorderColorBrush = x.Resources["BlackColorBrush"] as Brush, ColorBrush = x.Resources["WhiteColorBrush"] as Brush }).ToList();

            var theme = ThemeManager.DetectAppStyle();
            SelectedAccentColor = AccentColors.Where(x => x.Name == theme.Item2.Name).FirstOrDefault();
            SelectedTheme = AppThemes.Where(x => x.Name == theme.Item1.Name).FirstOrDefault();
        }

        private void ShowSettings()
        {
            IsSettingFlyoutOpen = true;
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
