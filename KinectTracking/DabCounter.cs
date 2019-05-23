using Emgu.CV;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace HelloMonitor
{
    class DabCounter
    {
        bool rightDabFound;
        bool leftDabFound;
        Skeleton MySkeleton;
        Joint rightHand;
        Joint leftHand;
        Joint rightElbow;
        Joint leftElbow;
        Joint rightShoulder;
        Joint leftShoulder;
        public int dabCounter;
        private int lastDab;

        public DabCounter()
        {
            rightDabFound = false;
            leftDabFound = false;
            dabCounter = 0;
        }

        public void Update(Skeleton skeleton)
        {
            MySkeleton = skeleton;
            rightHand = skeleton.Joints[JointType.HandRight];
            leftHand = skeleton.Joints[JointType.HandLeft];
            rightElbow = skeleton.Joints[JointType.ElbowRight];
            leftElbow = skeleton.Joints[JointType.ElbowLeft];
            rightShoulder = skeleton.Joints[JointType.ShoulderRight];
            leftShoulder = skeleton.Joints[JointType.ShoulderLeft];

            logs();

            //After 10s check for left and right dab
            int sForTwoDabs = 2;
            if((int)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds - lastDab > sForTwoDabs)
            {
                leftDabFound = false;
                rightDabFound = false;
                dabCounter = 0;
            }

            if (!leftDabFound && !rightDabFound)
            {
                checkingForRightDab();
                checkingForLeftDab();
            }
            else if (leftDabFound)
            {
                if (checkingForRightDab())
                {
                    leftDabFound = false;
                    rightDabFound = true;
                }
                dabCounter++;
            }
            else if (rightDabFound)
            {
                if (checkingForLeftDab())
                {
                    leftDabFound = true;
                    rightDabFound = false;
                }
                dabCounter++;
            }
        }

        private void logs()
        {
            //System.Diagnostics.Debug.WriteLine(lastDab);
            /*
            System.Diagnostics.Debug.WriteLine(
                "Przedramie R= " + XYToDegrees(rightHand.Position,  rightElbow.Position) + 
                " BicepsR= "     + XYToDegrees(rightElbow.Position, rightShoulder.Position) +
                " PrzedramieL= " + XYToDegrees(leftHand.Position,   leftElbow.Position) +
                " BicepsL = "    + XYToDegrees(leftElbow.Position,  leftShoulder.Position));*/
        }

        private bool checkingForRightDab()
        {
           bool przedramieR = (XYToDegrees(rightHand.Position, rightElbow.Position) > 290 &&
                XYToDegrees(rightHand.Position, rightElbow.Position) <= 355);
            bool bicepsR = XYToDegrees(rightElbow.Position, rightShoulder.Position) > 290 &&
                XYToDegrees(rightElbow.Position, rightShoulder.Position) < 355;
            bool przedramieL = XYToDegrees(leftHand.Position, leftElbow.Position) > 310 &&
                XYToDegrees(leftHand.Position, leftElbow.Position) < 360;
            bool bicepsL = XYToDegrees(leftElbow.Position, leftShoulder.Position) > 170 &&
                XYToDegrees(leftElbow.Position, leftShoulder.Position) < 280;
            
            if(przedramieL && przedramieR && bicepsL && bicepsR)
            {
                playWow();
                rightDabFound = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkingForLeftDab()
        {
            bool przedramieR = (XYToDegrees(rightHand.Position, rightElbow.Position) > 160 &&
                XYToDegrees(rightHand.Position, rightElbow.Position) <= 220);
            bool bicepsR = XYToDegrees(rightElbow.Position, rightShoulder.Position) > 0 &&
                XYToDegrees(rightElbow.Position, rightShoulder.Position) < 100;
            bool przedramieL = XYToDegrees(leftHand.Position, leftElbow.Position) > 160 &&
                XYToDegrees(leftHand.Position, leftElbow.Position) < 255;
            bool bicepsL = XYToDegrees(leftElbow.Position, leftShoulder.Position) > 150 &&
                XYToDegrees(leftElbow.Position, leftShoulder.Position) < 250;
            if (przedramieL && przedramieR && bicepsL && bicepsR)
            {
                playWow();
                leftDabFound = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        private float XYToDegrees(Microsoft.Kinect.SkeletonPoint p, Microsoft.Kinect.SkeletonPoint origin)
        { 
            double deltaX = origin.X - p.X;
            double deltaY = (origin.Y - p.Y);

            double radAngle = Math.Atan2(deltaY, deltaX);
            double degreeAngle = radAngle * 180.0 / Math.PI;

            return (float)(180.0 - degreeAngle);
        }

        private void playWow()
        {
            System.Diagnostics.Debug.WriteLine("DAB!!!!");
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"F:\Pro\HelloMonitor\Media\wow.wav");
            player.Play();
            lastDab = (int)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
