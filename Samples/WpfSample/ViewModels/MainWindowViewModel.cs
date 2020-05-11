using Microsoft.Extensions.Options;
using System;
using System.ComponentModel;

namespace WpfSample.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IService service;
        private readonly IOptions<AppSettings> settings;
        private string message;

        public MainWindowViewModel(IService service, IOptions<AppSettings> settings)
        {
            if (service is null)
                throw new ArgumentNullException(nameof(service));
            if (settings is null)
                throw new ArgumentNullException(nameof(settings));

            this.service = service;
            this.settings = settings;
            this.message = $"Time:'{service.GetTime()}, MySetting:'{settings.Value.MySetting}'";
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
