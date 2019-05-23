using System.Windows;
using System.Windows.Media;
using Microsoft.Kinect;

namespace HelloMonitor
{
    class SkeletonDraw
    {
        private const float RenderWidth = 640.0f;
        private const float RenderHeight = 480.0f;
        private const double JointThickness = 3;
        private const double BodyCenterThickness = 10;
        private const double ClipBoundsThickness = 10;
        private readonly Brush centerPointBrush = Brushes.Blue;
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
        private readonly Brush inferredJointBrush = Brushes.Yellow;
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        private KinectSensor sensor;
        private System.Windows.Controls.Image Image;
        private DrawingGroup drawingGroup;
        private DrawingImage imageSource;

        public SkeletonDraw(KinectSensor sensor, System.Windows.Controls.Image Image)
        {
            this.sensor = sensor;
            this.Image = Image;
            drawingGroup = new DrawingGroup();
            imageSource = new DrawingImage(drawingGroup);

            Image.Source = imageSource;
        }

        public void Draw(Skeleton[] skeletons)
        {
            if (skeletons.Length != 0)
            {
                using (DrawingContext dc = drawingGroup.Open())
                {
                    dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

                    foreach (Skeleton skel in skeletons)
                    {
                        RenderClippedEdges(skel, dc);
                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            DrawBonesAndJoints(skel, dc);
                        }
                        dc.DrawEllipse(
                        this.centerPointBrush,
                        null,
                        this.SkeletonPointToScreen(skel.Position),
                        BodyCenterThickness,
                        BodyCenterThickness);
                    }
                    
                    drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                }
            }
        }

        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            // Render Torso
            this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

            // Left Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

            // Left Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);

            Brush drawBrush = null;

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
                drawBrush = this.trackedJointBrush;
            }
            //joint.TrackingState == JointTrackingState.Tracked

            if (drawBrush != null)
            {
                drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(handJoint.Position), JointThickness, JointThickness);
            }
        }

        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = this.trackedBonePen;
            }

            drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
        }

        //private void CheckBoxSeatedModeChanged(object sender, RoutedEventArgs e)
        //{
        //    if (null != this.sensor)
        //    {
        //        //System.Windows.Controls.CheckBox
        //        if (this.checkBoxSeatedMode.IsChecked.GetValueOrDefault())
        //        {
        //            this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
        //        }
        //        else
        //        {
        //            this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
        //        }
        //    }
        //}

        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        public static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
        {
            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, RenderHeight));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
            }
        }
    }
}
