using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using XamlBrewer.Uwp.DynamicAccentColor;

namespace Mvvm.Services
{
    public static class Navigation
    {
        private static Frame _frame;

        private static readonly EventHandler<BackRequestedEventArgs> GoBackHandler =
            async (s, e) => await Navigation.GoBack();

        private static readonly EventHandler<BackPressedEventArgs> GoBackPhoneHandler =
            async (s, e) => await Navigation.GoBack(e);

        public static Frame Frame
        {
            get { return _frame; }
            set { _frame = value; }
        }

        public static async Task<bool> Navigate(Type sourcePageType)
        {
            if (_frame.CurrentSourcePageType == sourcePageType)
            {
                return true;
            }

            await InitiateNavigation(sourcePageType);
            var result = _frame.Navigate(sourcePageType);
            CompleteNavigation();

            return result;
        }

        public static void EnableBackButton()
        {
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed -= GoBackPhoneHandler;
                HardwareButtons.BackPressed += GoBackPhoneHandler;

                return;
            }

            var navManager = SystemNavigationManager.GetForCurrentView();
            navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            navManager.BackRequested -= GoBackHandler;
            navManager.BackRequested += GoBackHandler;
        }

        public static void DisableBackButton()
        {
            var navManager = SystemNavigationManager.GetForCurrentView();
            navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            navManager.BackRequested -= GoBackHandler;
        }

        public static async Task GoBack()
        {
            try
            {
                if (!_frame.CanGoBack)
                {
                    return;
                }

                // Just in case there's still a BackGroundPage hanging around.
                lock (typeof(Navigation))
                {
                    if (_frame.BackStack[_frame.BackStackDepth - 1].SourcePageType == typeof(BackgroundPage))
                    {
                        _frame.GoBack();
                    }
                }

                if (_frame.CanGoBack)
                {
                    var type = _frame.BackStack[_frame.BackStackDepth - 1].SourcePageType;
                    await InitiateNavigation(type);
                    CompleteNavigation();

                    _frame.GoBack();
                }
            }
            catch (Exception ex)
            {
                // Ignore, wild clicking going on.
                Debugger.Break();
            }
        }

        /// <summary>
        /// Hardware back button processing.
        /// </summary>
        public static async Task GoBack(BackPressedEventArgs e)
        {
            if (!_frame.CanGoBack)
            {
                // Bail out.
                return;
            }

            // Stay in the app.
            e.Handled = true;
            await GoBack();
        }

        private static void CompleteNavigation()
        {
            lock (typeof(Navigation))
            {
                if (_frame.BackStackDepth > 0)
                {
                    _frame.BackStack.RemoveAt(_frame.BackStackDepth - 1);
                }
            }
        }

        private static async Task InitiateNavigation(Type sourcePageType)
        {
            lock (typeof(Navigation))
            {
                // Apply the page's accent color (or the default)
                Theme.ApplyAccentColor(sourcePageType);

                // Clear native control and page caches so that they accept the new SystemAccentColor.
                // Navigate to an empty page.
                _frame.Navigate(typeof(BackgroundPage));
            }

            // Ye olde VB6 DoEvents.
            await Task.Delay(250);  // Put it to 100 to see the slider's delay.
        }
    }
}
