using System.Windows;

namespace HelloMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            this.DataContext = new WindowViewModel(this);
        }
    }
}
