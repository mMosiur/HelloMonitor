using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Emgu.CV;
using Emgu.CV.Structure;

namespace HelloMonitor
{
    public class Hand
    {
        List<Point> points;
        public double radiusSmall;
        public double radiusBig;
        double minDistance;
        double rate;
        public bool tracking;
        bool wasTracking;

        public enum Gesture { SWIPE_LEFT, SWIPE_RIGTH, SWIPE_DOWN, SWIPE_UP, NULL };
        
        public Hand(double radius = 5, double rate = 1, double minDistance = 100)
        {
            points = new List<Point>();
            points.Add(new Point(0, 0));
            radiusSmall = 0;
            radiusBig = radius;
            this.rate = rate;
            this.minDistance = minDistance;
            tracking = false;
            wasTracking = false;
        }

        public Point LastPoint()
        {
            return points[points.Count - 1];
        }

        public System.Drawing.Point ConvertPoint(Point p)
        {
            return new System.Drawing.Point((int)p.X, (int)p.Y);
        }

        static public double distance(Point p1, Point p2)
        {
            return (Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public void resetSmallRatius()
        {
            radiusSmall = 0;
        }

        public void Update(Point p)
        {
            double dist = distance(p, LastPoint());
            if(dist <= minDistance)
            {
                if (radiusSmall < radiusBig)
                {
                    radiusSmall += rate;
                }
                else
                {
                    radiusSmall = radiusBig;
                    tracking = true;
                }
            }
            else
            {
                wasTracking = tracking;
                tracking = false;
                radiusSmall -= rate * 2.5;
                if (radiusSmall < 0)
                    radiusSmall = 0;
            }
            points.Add(p);
            points[points.Count - 1] = ArithmeticAverage2(3);
            
        }

        public Point ArithmeticAverage2(int val = 3)
        {
            int amount = 0;
            double sumX = 0;
            double sumY = 0;
            for (int i = points.Count - 1; i >= 0 && i >= points.Count - 1 - val; i--)
            {

                sumX += points[i].X;
                sumY += points[i].Y;
                amount++;
            }
            return new Point(sumX / amount, sumY / amount);
        }

        public Point ArithmeticAverage(int val = 3)
        {
            int amount = 0;
            double sumX = 0;
            double sumY = 0;
            for (int i = points.Count - 1; i >= 0 && i >= points.Count - 1 - val; i--)
            {
                
                sumX += points[i].X - points[i - 1].X;
                sumY += points[i].Y - points[i - 1].Y;
                amount++;
            }
            return new Point(sumX / amount, sumY / amount);
        }

        private float XYToDegrees(Point xy, Point origin)
        {
            double deltaX = origin.X - xy.X;
            double deltaY = origin.Y - xy.Y;

            double radAngle = Math.Atan2(deltaY, deltaX);
            double degreeAngle = radAngle * 180.0 / Math.PI;

            return (float)(180.0 - degreeAngle);
        }

        bool checkingForGesture = false;
        int gestureCounter = 0;
        public Gesture CheckForGesture()
        {
            if (!tracking)
            {
                if (wasTracking)
                {
                    checkingForGesture = true;
                }
            }
            if(gestureCounter >= 5)
            {
                gestureCounter = 0;
                checkingForGesture = false;

                double angle = XYToDegrees(LastPoint(), points[points.Count - 5]);
                if (angle <= 180+45 && angle > 180-45)
                {
                    return Gesture.SWIPE_LEFT;
                }
                else if(angle <= 45 || angle > 360 - 45)
                {
                    return Gesture.SWIPE_RIGTH;
                }
                else if (angle > 90 - 45 && angle <= 90 + 45)
                {
                    return Gesture.SWIPE_UP;
                }
                else if (angle > 270 - 45 && angle <= 270 + 45)
                {
                    return Gesture.SWIPE_DOWN;
                }
            }

            if (checkingForGesture && !tracking)
            {
                gestureCounter++;
            }

            return Gesture.NULL;
        }

        //[CLSCompliant(false)]
        public void Draw(Image<Gray, byte> image)
        {
            CvInvoke.Circle(image, ConvertPoint( LastPoint() ), (int)radiusBig, new MCvScalar(150, 150, 150));
            if(tracking)
            {
                CvInvoke.Circle(image, ConvertPoint(LastPoint()), (int)radiusSmall, new MCvScalar(255, 255, 255), -1);
            }
            else
            {
                CvInvoke.Circle(image, ConvertPoint( LastPoint() ), (int)radiusSmall, new MCvScalar(150, 150, 150), -1);
            }
        }
    }
}
