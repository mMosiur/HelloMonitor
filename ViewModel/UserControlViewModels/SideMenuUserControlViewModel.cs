using System.Windows.Input;

namespace HelloMonitor
{
    class SideMenuUserControlViewModel : BaseViewModel, IPageViewModel
    {
        public void animateOut()
        {

        }

        private ICommand _goTo1;
        private ICommand _goTo2;
        private ICommand _goTo3;
        private ICommand _goTo4;

        public ICommand GoTo1 {
            get {
                return _goTo1 ?? (_goTo1 = new RelayCommand(x =>
                {
                    Mediator.Notify("GoTo1Screen", "");
                }));
            }
        }

        public ICommand GoTo2 {
            get {
                return _goTo2 ?? (_goTo2 = new RelayCommand(x =>
                {
                    Mediator.Notify("GoTo2Screen", "");
                }));
            }
        }

        public ICommand GoTo3 {
            get {
                return _goTo3 ?? (_goTo3 = new RelayCommand(x =>
                {
                    Mediator.Notify("GoTo3Screen", "");
                }));
            }
        }

        public ICommand GoTo4 {
            get {
                return _goTo4 ?? (_goTo4 = new RelayCommand(x =>
                {
                    Mediator.Notify("GoTo4Screen", "");
                }));
            }
        }
    }
}