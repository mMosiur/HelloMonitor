using System.Windows.Input;

namespace HelloMonitor
{
    class InfoPageViewModel : BaseViewModel, IPageViewModel
    {
        private ICommand _goTo2;

        public ICommand GoTo2 {
            get {
                return _goTo2 ?? (_goTo2 = new RelayCommand(x =>
                {
                    Mediator.Notify("GoTo2Screen", "");
                }));
            }
        }
    }
}
