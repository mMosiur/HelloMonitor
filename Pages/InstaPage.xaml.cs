using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HelloMonitor
{
    /// <summary>
    /// Interaction logic for InstaPage.xaml
    /// </summary>
    public partial class InstaPage : BasePage
    {

        public Storyboard storyboard;
        public InstaPage()
        {
            InitializeComponent();

            mainButton.Click += MainButton_Click;

            //CubicEase cubicEase = new CubicEase();
            //animation.EasingFunction = cubicEase;
            //mainCanvas.BeginAnimation(Canvas.TopProperty, animation);


            //storyboard = new Storyboard();
            //storyboard.Children.Add(animation);
            //Storyboard.SetTargetName(storyboard, "mainGrid.mainCanvas2");
            //Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));

        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {

            //TranslateTransform translateTransform = new TranslateTransform(100, 0);
            //mainCanvas2.RenderTransform = translateTransform;
            //DoubleAnimation animation = new DoubleAnimation(0, 500, TimeSpan.FromSeconds(1.0));
            //translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);

            

            ItemsControl itemsControl = (ItemsControl)mainCanvas2.Children[0];
            ItemCollection items = itemsControl.Items;

            for(int i = 0; i < items.Count; i++)
            {
                Canvas canvas = (Canvas)items[i];
                canvas.CacheMode = new BitmapCache();

                double top = (double)canvas.GetValue(Canvas.TopProperty);

                DoubleAnimation animation = new DoubleAnimation(top, top + 300, TimeSpan.FromSeconds(1.0));
                CubicEase cubicEase = new CubicEase();
                animation.EasingFunction = cubicEase;
                canvas.BeginAnimation(Canvas.TopProperty, animation);

                //canvas.BeginAnimation(Canvas.TopProperty, animation);

                //TranslateTransform translateTransform = new TranslateTransform();
                //canvas.RenderTransform = translateTransform;
                //translateTransform.BeginAnimation(Canvas.TopProperty, animation);

                //TranslateTransform translateTransform = new TranslateTransform(0, 100);
                //canvas.RenderTransform = translateTransform;
                //DoubleAnimation animation = new DoubleAnimation(0, 500, TimeSpan.FromSeconds(1.0));

                //translateTransform.BeginAnimation(TranslateTransform.YProperty, animation);



                // to do 

                //canvas.RenderTransform.BeginAnimation(Canvas.TopProperty, animation);
            }

            //mainCanvas2.BeginAnimation(Canvas.TopProperty, animation);


        }

        private void MainCanvas2_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainButton_Click(sender, e);
        }
    }
}
