using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;
using System.Windows;

namespace WpfSample
{
    public class ViewModelLocator
    {
        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoWireViewModelProperty, value);
        }

        public static readonly DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), new PropertyMetadata(OnAuoWireViewModelChanged));

        private static void OnAuoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
                return;

            bool autoWireViewModel = (bool)e.NewValue;
            if (autoWireViewModel)
            {
                var frameworkElement = d as FrameworkElement;

                if (frameworkElement == null)
                    throw new InvalidOperationException("FrameworkElement required for AutoWireViewModel Attached property");

                ResolveViewModel(frameworkElement);
            }
        }

        private static void ResolveViewModel(FrameworkElement view)
        {
            var viewModelType = ViewModelLocationProvider.ResolveViewModelType(view.GetType());
            if (viewModelType != null)
            {
                var viewModel = ViewModelLocationProvider.CreateViewModelInstance(viewModelType);
                view.DataContext = viewModel;
            }
        }
    }
}
