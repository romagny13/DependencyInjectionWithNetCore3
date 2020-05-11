using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using WpfSample.ViewModels;
using WpfSample.Views;

namespace WpfSample
{
    public partial class App : Application
    {
        private IHost host;

        public App()
        {
            host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    ConfigureContainer();
                    RegisterServices(services);
                }).Build();
        }

        private void ConfigureContainer()
        {
            ViewModelLocationProvider.SetViewModelFactory(viewType => host.Services.GetRequiredService(viewType));
        }

        private void RegisterServices(IServiceCollection services)
        {
           // Sample
            services.AddSingleton<IService, ServiceA>();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // create shell
            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            // initialize shell
            MainWindow = mainWindow;
            // show shell
            mainWindow.Show();
        }
    }


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


            bool autoWIreViewModel = (bool)e.NewValue;
            if (autoWIreViewModel)
            {
                var frameworkElement = d as FrameworkElement;
                if (frameworkElement == null)
                    throw new InvalidOperationException("FrameworkElement required for AutoWireViewModel Attached property");

                frameworkElement.Loaded += Element_Loaded;
            }
        }

        private static void Element_Loaded(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            frameworkElement.Loaded -= Element_Loaded;
            ResolveViewModel(frameworkElement);
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

    public class ViewModelLocationProvider
    {
        private static Func<Type, Type> defaultViewTypeToViewModelTypeResolver;
        private static Func<Type, object> viewModelFactory;

        static ViewModelLocationProvider()
        {
            // By Type or feature
            defaultViewTypeToViewModelTypeResolver = new Func<Type, Type>(viewType =>
            {
                var viewFullName = viewType.FullName;
                viewFullName = viewFullName.Replace(".Views.", ".ViewModels."); // ignored by feature
                var suffix = viewFullName.EndsWith("View") ? "Model" : "ViewModel";
                var viewModelFullName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", viewFullName, suffix);
                var viewModelType = viewType.Assembly.GetType(viewModelFullName);
                return viewModelType;
            });

            SetViewModelFactoryToDefault();
        }

        public static void SetViewModelFactory(Func<Type, object> viewModelFactory)
        {
            if (viewModelFactory == null)
                throw new ArgumentNullException(nameof(viewModelFactory));

            ViewModelLocationProvider.viewModelFactory = viewModelFactory;
        }

        public static void SetViewModelFactoryToDefault()
        {
            viewModelFactory = new Func<Type, object>(viewModelType => Activator.CreateInstance(viewModelType));
        }

        public static Type ResolveViewModelType(Type viewType)
        {
            if (viewType is null)
                throw new ArgumentNullException(nameof(viewType));

            Type viewModelType = defaultViewTypeToViewModelTypeResolver(viewType);
            return viewModelType;
        }

        public static object CreateViewModelInstance(Type viewModelType)
        {
            if (viewModelType is null)
                throw new ArgumentNullException(nameof(viewModelType));

            var viewModel = viewModelFactory(viewModelType);
            return viewModel;
        }
    }


    public interface IService
    {
        string GetTime();
    }

    public class ServiceA : IService
    {
        public string GetTime()
        {
            return DateTime.Now.ToLongTimeString();
        }
    }
}
