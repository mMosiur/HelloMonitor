
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace HelloMonitor
{
    /// <summary>
    /// Logika interakcji dla klasy InfoPage.xaml
    /// </summary>
    public partial class InfoPage : BasePage {



        public InfoPage() {
            InitializeComponent();

            //scrollButton.Click += ScrollButton_Click;


            mainStackPanel.MaxHeight = 800;

            //mainStackPanel.SetVerticalOffset(1000);

            //for (int i = 0; i < rootObject.payload.Count; i++)
            //{
            //    var news = rootObject.payload;
            //    Grid grid = new Grid();

            //    ColumnDefinition cd1 = new ColumnDefinition();
            //    cd1.Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
            //    ColumnDefinition cd2 = new ColumnDefinition();
            //    cd2.Width = new System.Windows.GridLength(2, System.Windows.GridUnitType.Star);
            //    grid.ColumnDefinitions.Add(cd1);
            //    grid.ColumnDefinitions.Add(cd2);

            //    RowDefinition rd1 = new RowDefinition();
            //    rd1.Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
            //    RowDefinition rd2 = new RowDefinition();
            //    rd2.Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
            //    grid.RowDefinitions.Add(rd1);
            //    grid.RowDefinitions.Add(rd2);

            //    Image image = new Image();
            //    //BitmapImage bitmapImage = new BitmapImage(new Uri(news[i].url));
            //    //bitmapImage.DownloadCompleted += (sender, e) => BitmapImage_DownloadCompleted(sender, e, image);

            //    Binding bind = new Binding(String.Format("rootObject.payload[{0}].source", i));
            //    image.SetBinding(Image.SourceProperty, bind);
            //    image.Width = 100;
            //    image.SetValue(Grid.RowProperty, 0);
            //    image.SetValue(Grid.ColumnProperty, 0);
            //    image.SetValue(Grid.RowSpanProperty, 2);

            //    grid.Children.Add(image);

            //    TextBlock title = new TextBlock();
            //    title.Text = news[i].title;
            //    title.SetValue(Grid.ColumnProperty, 1);
            //    title.SetValue(Grid.RowProperty, 0);
            //    grid.Children.Add(title);

            //    TextBlock description = new TextBlock();
            //    description.Width = 250;
            //    description.TextWrapping = System.Windows.TextWrapping.WrapWithOverflow;
            //    description.Text = "test"; //news[i].Description;
            //    description.SetValue(Grid.ColumnProperty, 1);
            //    description.SetValue(Grid.RowProperty, 2);
            //    grid.Children.Add(description);
            //    grid.Margin = new System.Windows.Thickness(0, 100, 0, 0);

            //    mainStackPanel.Children.Add(grid);
            //}

            //Binding bind = new Binding("panel");
            //ItemsControl itemsControl = new ItemsControl();
            //itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, bind);
            //mainStackPanel.Children.Add(itemsControl);

            //string URL = "http://212.182.24.47:3001/news";
            //using (var webClient = new System.Net.WebClient())
            //{
            //    var json = webClient.DownloadString(URL);
            //    // Now parse with JSON.Net
            //    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(json);
            //}
        }

        private void ScrollButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mainStackPanel.SetVerticalOffset(500);
            //mainScrollViewer.ScrollToVerticalOffset(100);
            //mainScrollViewer.

            //DoubleAnimation verticalAnimation = new DoubleAnimation();

            //verticalAnimation.From = mainScrollViewer.VerticalOffset;
            //verticalAnimation.To = 300;
            //verticalAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            //Storyboard storyboard = new Storyboard();
            //storyboard.Children.Add(verticalAnimation);
            //Storyboard.SetTarget(verticalAnimation, mainScrollViewer);
            //Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(ScrollAnimationBehavior.VerticalOffsetProperty));
            //storyboard.Begin();

            //mainScrollViewer.BeginAnimation(ScrollViewer.ContentVerticalOffsetProperty, verticalAnimation);
        }

        private void ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            Console.WriteLine("1");
        }

        private void BitmapImage_DownloadCompleted(object sender, EventArgs e, Image image)
        {
            BitmapImage bitmapImage = (BitmapImage)sender;
            Grid parentGrid = (Grid)image.Parent;
            DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.4));
            image.BeginAnimation(Image.OpacityProperty, animation);
        }

        private void Image_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Image image = (Image)sender;
            Grid par = (Grid)image.Parent;
        }

        public void changeImage(string url)
        {

        }

        private void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
