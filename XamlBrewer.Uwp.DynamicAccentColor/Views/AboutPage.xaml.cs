using Windows.UI.Xaml.Controls;
using Mvvm.Services;

namespace XamlBrewer.Uwp.DynamicAccentColor
{
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            this.InitializeComponent();

            // Just to test
            Navigation.DisableBackButton();
        }
    }
}
