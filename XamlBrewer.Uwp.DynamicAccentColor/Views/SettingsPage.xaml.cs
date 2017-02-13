using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Mvvm;
using Mvvm.Services;

namespace XamlBrewer.Uwp.DynamicAccentColor
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        public ICommand AboutCommand => new DelegateCommand(async () => await Navigation.Navigate(typeof(AboutPage)));
    }
}
