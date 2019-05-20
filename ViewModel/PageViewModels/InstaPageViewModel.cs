using System.Windows.Input;

namespace HelloMonitor
{
    class InstaPageViewModel : BaseViewModel, IPageViewModel
    {
        private ICommand _goTo1;

        public ICommand GoTo1 {
            get {
                return _goTo1 ?? (_goTo1 = new RelayCommand(x =>
                {
                    Mediator.Notify("GoTo1Screen", "");
                }));
            }
        }
    }
}
