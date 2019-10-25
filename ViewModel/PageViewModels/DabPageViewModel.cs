using System.Windows.Input;

namespace HelloMonitor
{
    class DabPageViewModel : BaseViewModel, IPageViewModel
    {
        public void animateOut()
        {
            System.Console.WriteLine("DabPageViewModel animateOut");
        }
    }
}
