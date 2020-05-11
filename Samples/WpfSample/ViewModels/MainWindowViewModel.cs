using System;
using System.ComponentModel;

namespace WpfSample.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        private IService service;
        private string message;

        public MainWindowViewModel(IService service)
        {
            if (service is null)
                throw new ArgumentNullException(nameof(service));

            this.service = service;
            this.message = service.GetTime();
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
