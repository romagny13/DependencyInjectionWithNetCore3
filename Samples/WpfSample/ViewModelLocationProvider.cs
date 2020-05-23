using System;
using System.Globalization;

namespace WpfSample
{
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
}
