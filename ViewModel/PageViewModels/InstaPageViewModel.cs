using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace HelloMonitor
{
    class InstaPageViewModel : BaseViewModel, IPageViewModel
    {

        public class Payload
        {
            public string caption { get; set; }
            public int likes { get; set; }
            public string photo_url { get; set; }
            public BitmapSource source { get; set; }
        }

        public class Info
        {
            public int followers { get; set; }
        }

        public class RootObject
        {
            public List<Payload> payload { get; set; }
            public Info info { get; set; }
            public bool success { get; set; }
        }

        public RootObject rootObject { get; set; }
        public ObservableCollection<Canvas> collection { get; set; }

        int k = 0;
        public InstaPageViewModel(RootObject rootObject)
        {
            this.rootObject = rootObject;
            this.collection = new ObservableCollection<Canvas>();

            //int count = rootObject.payload.Count;
            int count = 1;

            for (int i = 0; i < count; i++)
            {
                Grid grid = new Grid();

                Image image = new Image();
                image.Source = rootObject.payload[i].source;
                
                if (image.Source.CanFreeze)
                {
                    image.Source.Freeze();
                    System.Console.WriteLine("freeze");
                }
                image.Width = 400;

                grid.Children.Add(image);

                grid.Margin = new System.Windows.Thickness(0, 50, 0, 0);

                Canvas canvas = new Canvas();
                canvas.Children.Add(grid);

                Canvas.SetTop(canvas, (double)k);

                collection.Add(canvas);

                k += 200;
            }
        }

        public void animateOut()
        {

        }
    }
}
