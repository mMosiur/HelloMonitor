using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace HelloMonitor
{
    public class InfoPageViewModel : BaseViewModel, IPageViewModel
    {
        public class Payload
        {
            public string color { get; set; }
            public string type { get; set; }
            public string title { get; set; }
            public string url { get; set; }

            public BitmapSource source { get; set; }

            public void Update()
            {

            }
        }

        public class RootObject
        {
            public bool success { get; set; }
            public DateTime last_updated { get; set; }
            public List<Payload> payload { get; set; }

            public void Update()
            {
                payload.ForEach((Payload p) => p.Update());
            }
        }

        public RootObject rootObject { get; set; }

        public StackPanel panel { get; set; }

        public ObservableCollection<Grid> collection { get; set; }

        public void animateOut()
        {

        }


        public InfoPageViewModel(RootObject rootObject)
        {
            //this.children = new UIElementCollection(null, null);
            //this.panel = new StackPanel();
            this.collection = new ObservableCollection<Grid>();

            this.rootObject = rootObject;

            for (int i = 0; i < rootObject.payload.Count; i++)
            {
                var news = rootObject.payload;
                Grid grid = new Grid();
                grid.Width = 1500;

                ColumnDefinition cd1 = new ColumnDefinition();
                cd1.Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                ColumnDefinition cd2 = new ColumnDefinition();
                cd2.Width = new System.Windows.GridLength(2, System.Windows.GridUnitType.Star);
                grid.ColumnDefinitions.Add(cd1);
                grid.ColumnDefinitions.Add(cd2);

                RowDefinition rd1 = new RowDefinition();
                rd1.Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                RowDefinition rd2 = new RowDefinition();
                rd2.Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                grid.RowDefinitions.Add(rd1);
                grid.RowDefinitions.Add(rd2);

                Image image = new Image();
                //BitmapImage bitmapImage = new BitmapImage(new Uri(news[i].url));
                //bitmapImage.DownloadCompleted += (sender, e) => BitmapImage_DownloadCompleted(sender, e, image);

                // souce = payload.source
                //Binding bind = new Binding(String.Format("rootObject.payload[{0}].source", i));
                //image.SetBinding(Image.SourceProperty, bind);

                image.Source = rootObject.payload[i].source;
                image.Width = 250;
                image.SetValue(Grid.RowProperty, 0);
                image.SetValue(Grid.ColumnProperty, 0);
                image.SetValue(Grid.RowSpanProperty, 2);
                //image.Margin = new System.Windows.Thickness(5, 0, 0, 0);

                grid.Children.Add(image);

                TextBlock title = new TextBlock();
                title.Text = news[i].title;
                title.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                title.FontSize = 30.0;
                title.SetValue(Grid.ColumnProperty, 1);
                title.SetValue(Grid.RowProperty, 0);
                grid.Children.Add(title);

                TextBlock description = new TextBlock();
                description.Width = 250;
                description.TextWrapping = System.Windows.TextWrapping.WrapWithOverflow;
                description.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                description.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(news[i].color));
                description.Text = news[i].type;
                description.SetValue(Grid.ColumnProperty, 1);
                description.SetValue(Grid.RowProperty, 2);
                grid.Children.Add(description);
                grid.Margin = new System.Windows.Thickness(0, 100, 0, 0);

                this.collection.Add(grid);
                //panel.Children.Add(grid);
                //children.Add(grid);
                //mainStackPanel.Children.Add(grid);
            }
        }
    }
}