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
    using Emgu.CV;
    using Emgu.CV.Structure;
    using Microsoft.Kinect;
    //using Microsoft.Samples.Kinect.SkeletonBasics;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    [CLSCompliant(false)]
    public partial class MainWindow : Window
    {
        private KinectSensor sensor;
        private Hand hand;
        private DabCounter dabCounter;

        float lastHandZ = 0;

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

            SetupSlideDispatcher();
            CreateAnEllipse();
        }

        public void CreateAnEllipse()
        {
            // Create an Ellipse    
            mouseEllipse = new System.Windows.Shapes.Ellipse();
            mouseEllipse.Height = 50;
            mouseEllipse.Width = 50;
            // Create a blue and a black Brush    
            SolidColorBrush ghostWhiteBrush = new SolidColorBrush();
            ghostWhiteBrush.Color = Colors.GhostWhite;
            SolidColorBrush darkGrayBrush = new SolidColorBrush();
            darkGrayBrush.Color = Colors.DarkGray;
            // Set Ellipse's width and color    
            mouseEllipse.StrokeThickness = 4;
            mouseEllipse.Stroke = darkGrayBrush;
            // Fill rectangle with blue color    
            mouseEllipse.Fill = ghostWhiteBrush;
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

            Boolean isLeftHand = false;
            //Get higher hand left else right
            if (skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HandRight].Position.Y)
            {
                handJoint = skeleton.Joints[JointType.HandLeft];
                downLimit = skeleton.Joints[JointType.ElbowLeft].Position.Y;
                isLeftHand = true;
            }
            else
            {
                handJoint = skeleton.Joints[JointType.HandRight];
                downLimit = skeleton.Joints[JointType.ElbowRight].Position.Y;
            }

            //Hand has to be higher then
            if (handJoint.Position.Y > downLimit)
            {
                Point p = SkeletonPointToScreen(handJoint.Position);
                hand.Update(p);

                Hand.Gesture g = hand.CheckForGesture();
                switch (g)
                {
                    case Hand.Gesture.SWIPE_LEFT:
                        System.Diagnostics.Debug.WriteLine("Left");
                        setSlideManual(slideIndex--);

                        break;
                    case Hand.Gesture.SWIPE_RIGTH:
                        System.Diagnostics.Debug.WriteLine("SWIPE_RIGTH");
                        setSlideManual(slideIndex++);
                        break;
                    case Hand.Gesture.SWIPE_UP:
                        System.Diagnostics.Debug.WriteLine("SWIPE_UP");
                        break;
                    case Hand.Gesture.SWIPE_DOWN:
                        System.Diagnostics.Debug.WriteLine("SWIPE_DOWN");
                        break;
                    default:
                        break;
                }

                //Setting mouse position with scaling 
                Point headPoint = SkeletonPointToScreen(skeleton.Joints[JointType.Head].Position);
                Point p2 = hand.LastPoint();
                int x;
                if (isLeftHand)
                {
                    x = (int)(p2.X * 3 * 3) - 500;
                }
                else
                {
                    x = (int)(p2.X * 3 * 3) - (int)(headPoint.X * 3 * 2) - 500;
                }

                int y = (int)(p2.Y * 2.25 * 2) - (int)(headPoint.Y * 2.25 ) ;
                
                NativeMethods.SetCursorPos(x, y);

                //Moving ellipse with mouse 
                Point p3 = Mouse.GetPosition(OverGrid);
                int x3 = (int)p3.X;
                int y3 = (int)p3.Y;

                double x1 = (x3) * 2 - 1250;
                double y2 = (y3) * 2 - 630;

                mouseEllipse.Margin = new Thickness(x1, y2, 0, 0);

                //Animation of ellipse
                double procentageRadius = hand.radiusSmall / hand.radiusBig;
                double maxThickness = 30;
                mouseEllipse.StrokeThickness = procentageRadius > 1 ? maxThickness : procentageRadius < 0.1 ? 0.1 * maxThickness : procentageRadius * maxThickness;

                if(procentageRadius >= 1)
                {
                    mouseEllipse.Fill.Opacity = 0.3;
                    System.Diagnostics.Debug.WriteLine(handJoint.Position.Z - lastHandZ);
                    if (lastHandZ - handJoint.Position.Z > 0.015)
                    {
                        LeftMouseClick(x, y);
                    }

                    lastHandZ = handJoint.Position.Z;
                }
                else
                {
                    mouseEllipse.Fill.Opacity = 0.3;
                }

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
            //Point p = NativeMethods.GetMousePosition();

            Point p = Mouse.GetPosition(OverGrid);
            int x = (int)p.X;
            int y = (int)p.Y;

            double x1 = (x) *2 - 1250;
            double y2 = (y) *2 - 630;   
            
            mouseEllipse.Margin = new Thickness(x1, y2, 0, 0);
        }

        System.Windows.Threading.DispatcherTimer dispatcherTimerSlide = new System.Windows.Threading.DispatcherTimer();
        void SetupSlideDispatcher()
        {
            dispatcherTimerSlide.Tick += dispatcherTimer_SlideTick;
            dispatcherTimerSlide.Interval = new TimeSpan(0, 0, 0, 3, 0);
            dispatcherTimerSlide.Start();
        }
        private void dispatcherTimer_SlideTick(object sender, EventArgs e)
        {
            //HideAndShowButtons
            //if (ControlsGrid.Width == 0)
            //    ControlsGrid.Width = widthOfControlPanel;
            //else
            //    ControlsGrid.Width = 0;
            //setSlideAuto();
        }

        public System.Drawing.Point ConvertPoint(Point p)
        {
            return new System.Drawing.Point((int)p.X, (int)p.Y);
        }

        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

    }
    
}
