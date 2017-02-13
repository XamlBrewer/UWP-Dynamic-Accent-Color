using Windows.UI.Xaml.Controls;
using Mvvm.Services;

namespace XamlBrewer.Uwp.DynamicAccentColor
{
    /// <summary>
    /// Class DonkeyPage. This class cannot be inherited.
    /// </summary>
    public sealed partial class DonkeyPage : Page
    {
        public DonkeyPage()
        {
            this.InitializeComponent();

            // To test the unregistration of the handler.
            Navigation.EnableBackButton();
        }
    }
}
