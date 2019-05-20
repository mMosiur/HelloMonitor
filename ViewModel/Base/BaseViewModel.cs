using PropertyChanged;
using System.ComponentModel;

namespace HelloMonitor {
    /// <summary>
    /// A base view model that fires Property Changed events as needed
    /// </summary>
    [ImplementPropertyChanged]
    public abstract class BaseViewModel : INotifyPropertyChanged {
        /// <summary>
        /// The event that is fired when any child property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
