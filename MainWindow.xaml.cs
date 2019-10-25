using Newtonsoft.Json;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Kinect;
using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HelloMonitor
{
    [CLSCompliant(false)]
    public partial class MainWindow : Window {
        private KinectSensor sensor;
        private Hand hand;
        private DabCounter dabCounter;

        //Eclipse
        private System.Windows.Shapes.Ellipse mouseEllipse;
        ApplicationPageValueConverter ap = new ApplicationPageValueConverter();
        private int slideIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += WindowLoaded;
            this.DataContext = new WindowViewModel(this);

            hand = new Hand(20);
            dabCounter = new DabCounter();
            //System.Windows.Resources.StreamResourceInfo info = Application.GetResourceStream(new Uri("Images/kac.cur", UriKind.Relative));
            //this.Cursor = new Cursor(info.Stream);

            //SetupSlideDispatcher();
            //CreateAnEllipse();
            //SetupEclipseDispatcher();
        }

        public void CreateAnEllipse()
        {
            // Create an Ellipse    
            mouseEllipse = new System.Windows.Shapes.Ellipse();
            mouseEllipse.Height = 50;
            mouseEllipse.Width = 50;
            // Create a blue and a black Brush    
            SolidColorBrush blueBrush = new SolidColorBrush();
            blueBrush.Color = Colors.GhostWhite;
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.DarkGray;
            // Set Ellipse's width and color    
            mouseEllipse.StrokeThickness = 4;
            mouseEllipse.Stroke = blackBrush;
            // Fill rectangle with blue color    
            mouseEllipse.Fill = blueBrush;
            mouseEllipse.Opacity = 0.5;
            // Add Ellipse to the Grid. 

            OverGrid.Children.Add(mouseEllipse);
        }

        /// <summary>
        /// Kinect stuff below
        /// </summary>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                this.sensor.SkeletonStream.Enable();
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {
                System.Diagnostics.Debug.WriteLine("No kinect rdy");
            }
        }
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            bool isScelTracked = false;
            if (skeletons.Length != 0)
            {
                foreach (Skeleton skeleton in skeletons)
                {
                    if (isScelTracked) break;

                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        Updates(skeleton);
                        isScelTracked = true;
                    }
                    else if (skeleton.TrackingState == SkeletonTrackingState.PositionOnly)
                    {

                    }
                }
            }
        }
        private void Updates(Skeleton skeleton)
        {
            //Dab counter
            dabCounter.Update(skeleton);

            Joint handJoint;
            float downLimit;

            if (skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HandRight].Position.Y)
            {
                handJoint = skeleton.Joints[JointType.HandLeft];
                downLimit = skeleton.Joints[JointType.ElbowLeft].Position.Y;
            }
            else
            {
                handJoint = skeleton.Joints[JointType.HandRight];
                downLimit = skeleton.Joints[JointType.ElbowRight].Position.Y;
            }

            //Hand has to be higher then
            if (downLimit < handJoint.Position.Y)
            {
                Point p = SkeletonPointToScreen(handJoint.Position);
                hand.Update(p);

                Hand.Gesture g = hand.CheckForGesture();
                switch (g)
                {
                    case Hand.Gesture.SWIPE_LEFT:
                        //((UserControlAutobus)userControls[0]).label1.Content = "Left";
                        setSlideManual(slideIndex--);

                        break;
                    case Hand.Gesture.SWIPE_RIGTH:
                        //((UserControlAutobus)userControls[0]).label1.Content = "Right";
                        setSlideManual(slideIndex++);
                        break;
                    case Hand.Gesture.SWIPE_UP:
                        //((UserControlAutobus)userControls[0]).label1.Content = "Up";
                        break;
                    case Hand.Gesture.SWIPE_DOWN:
                        //((UserControlAutobus)userControls[0]).label1.Content = "Down";

                        break;
                    default:
                        break;
                }

                Point p2 = hand.LastPoint();
                //int x = (int)(p2.X * 3);
                //int y = (int)(p2.Y * 2.25);
                int x = (int)(p2.X);
                int y = (int)(p2.Y);
                NativeMethods.SetCursorPos(x, y);

                double left = x - (1980);
                double top = y - (1080);

            }
            else
            {
                hand.resetSmallRatius();
            }
        }
        private float remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        private void setSlideAuto()
        {
            if (!dispatcherTimerSlide.IsEnabled)
                dispatcherTimerSlide.Start();
        }

        private void setSlideManual(int index)
        {
            dispatcherTimerSlide.Stop();
        }

        /// <summary>
        /// Dispatchers
        /// </summary>
        System.Windows.Threading.DispatcherTimer dispatcherEclipseTimer = new System.Windows.Threading.DispatcherTimer();
        void SetupEclipseDispatcher()
        {
            dispatcherEclipseTimer.Tick += dispatcherEclipceTimer_Tick;
            dispatcherEclipseTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherEclipseTimer.Start();
        }
        private void dispatcherEclipceTimer_Tick(object sender, EventArgs e)
        {
            //NativeMethods.SetCursorPos(100, 100);
            Point p = NativeMethods.GetMousePosition();
            int x = (int)p.X;
            int y = (int)p.Y;

            double x1 = (x - (mainGrid.ActualHeight / 2)) * 2 - 680 + 40;
            double y2 = (y - (mainGrid.ActualWidth / 2)) * 2 + 410 + 40;

            //double xMargin = mouseEllipse.Margin.Left;
            //double yMargin = mouseEllipse.Margin.Top;

            mouseEllipse.Margin = new Thickness(x1, y2, 0, 0);
        }

        System.Windows.Threading.DispatcherTimer dispatcherTimerSlide = new System.Windows.Threading.DispatcherTimer();
        void SetupSlideDispatcher()
        {
            dispatcherTimerSlide.Tick += dispatcherTimer_SlideTick;
            dispatcherTimerSlide.Interval = new TimeSpan(0, 0, 0, 3, 0);
            //dispatcherTimerSlide.Start();
        }
        private void dispatcherTimer_SlideTick(object sender, EventArgs e)
        {
            //HideAndShowButtons
            //if (ControlsGrid.Width == 0)
            //    ControlsGrid.Width = widthOfControlPanel;
            //else
            //    ControlsGrid.Width = 0;
            setSlideAuto();
        }
    }
}
