using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using XamlBrewer.Uwp.DynamicAccentColor;

namespace Mvvm.Services
{
    public static class Navigation
    {
        private static Frame _frame;
        private static readonly EventHandler<BackRequestedEventArgs> _goBackHandler = async (s, e) => await Navigation.GoBack();

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
            var navManager = SystemNavigationManager.GetForCurrentView();
            navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            navManager.BackRequested -= _goBackHandler;
            navManager.BackRequested += _goBackHandler;
        }

        public static void DisableBackButton()
        {
            var navManager = SystemNavigationManager.GetForCurrentView();
            navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            navManager.BackRequested -= _goBackHandler;
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
