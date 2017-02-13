using System.Windows.Input;
using Mvvm;
using Mvvm.Services;

namespace XamlBrewer.Uwp.DynamicAccentColor.ViewModels
{
    public class RabbitPageViewModel : BindableBase
    {
        public ICommand SettingsCommand
        {
            get { return new DelegateCommand(async () => await Navigation.Navigate(typeof(SettingsPage))); }
        }
    }
}
