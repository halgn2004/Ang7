using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace Ang7.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        private bool _isNotConnected;

        public bool IsNotConnected
        {
            get => _isNotConnected;
            set => SetProperty(ref _isNotConnected, value);
        }

        public BaseViewModel()
        {
            Connectivity.ConnectivityChanged += ConnectivityChanged;
            IsNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        // IDisposable implementation
        public void Dispose()
        {
            // Unsubscribe from the event to avoid memory leaks
            Connectivity.ConnectivityChanged -= ConnectivityChanged;
            GC.SuppressFinalize(this); // Optional, for better garbage collection
        }

        private void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action? onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
