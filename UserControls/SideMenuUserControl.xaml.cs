using System.Windows.Controls;

namespace HelloMonitor
{
    /// <summary>
    /// Interaction logic for SideMenuUserControl.xaml
    /// </summary>
    public partial class SideMenuUserControl : UserControl
    {
        public SideMenuUserControl()
        {
            InitializeComponent();
            this.DataContext = new SideMenuUserControlViewModel();

            // pobierze linki
            // podmieni
            
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
