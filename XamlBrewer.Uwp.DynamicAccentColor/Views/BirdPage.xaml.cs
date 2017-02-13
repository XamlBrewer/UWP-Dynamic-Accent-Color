using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Mvvm.Services;

namespace XamlBrewer.Uwp.DynamicAccentColor
{
    public sealed partial class BirdPage : Page
    {
        public BirdPage()
        {
            this.InitializeComponent();
        }

        private async void RabbitButton_OnClick(object sender, RoutedEventArgs e)
        {
            await Navigation.Navigate(typeof(RabbitPage));
        }
    }
}
