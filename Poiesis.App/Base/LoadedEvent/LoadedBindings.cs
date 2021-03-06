﻿using System.Windows;
using System.Windows.Controls;

namespace Poiesis.App.Base.LoadedEvent
{
    public class LoadedBindings
    {
        public static readonly DependencyProperty LoadedEnabledProperty =
            DependencyProperty.RegisterAttached(
                "LoadedEnabled",
                typeof(bool),
                typeof(LoadedBindings),
                new PropertyMetadata(false, new PropertyChangedCallback(OnLoadedEnabledPropertyChanged)));

        public static bool GetLoadedEnabled(DependencyObject sender) => (bool)sender.GetValue(LoadedEnabledProperty);
        public static void SetLoadedEnabled(DependencyObject sender, bool value) => sender.SetValue(LoadedEnabledProperty, value);

        private static void OnLoadedEnabledPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ContentControl contentControl)
            {
                bool newEnabled = (bool)e.NewValue;
                bool oldEnabled = (bool)e.OldValue;

                if (oldEnabled && !newEnabled)
                {
                    contentControl.Loaded -= MyContentControlLoaded;
                }
                else if (!oldEnabled && newEnabled)
                {
                    contentControl.Loaded += MyContentControlLoaded;
                }
            }
        }

        private static void MyContentControlLoaded(object sender, RoutedEventArgs e)
        {
            ILoadedAction loadedAction = GetLoadedAction((ContentControl)sender);
            loadedAction?.ContentControlLoaded();
        }

        public static readonly DependencyProperty LoadedActionProperty =
            DependencyProperty.RegisterAttached(
                "LoadedAction",
                typeof(ILoadedAction),
                typeof(LoadedBindings),
                new PropertyMetadata(null));

        public static ILoadedAction GetLoadedAction(DependencyObject sender) => (ILoadedAction)sender.GetValue(LoadedActionProperty);
        public static void SetLoadedAction(DependencyObject sender, ILoadedAction value) => sender.SetValue(LoadedActionProperty, value);
    }
}
