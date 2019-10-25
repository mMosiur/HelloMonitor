using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace HelloMonitor
{
    /// <summary>
    /// Animation helpers for <see cref="StoryBoard"/>
    /// </summary>
    public static class StoryboardHelpers
    {

        /// <summary>
        /// Adds a slide from left animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="offset">The distance to the left to start from</param>
        /// <param name="decelerationRatio">The rate of deceleration</param>
        public static void AddSlideFromLeft(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f)
        {
            // Create the margin animate from left
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(-offset, 0, offset, 0),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio
            };
            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            // Add this to the storyboard
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a fade in animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="seconds">The time the animation will take</param>
        public static void AddFadeIn(this Storyboard storyboard, float seconds)
        {
            // Create opacity fade in animation
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 0,
                To = 1
            };
            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            // Add this to the storyboard
            storyboard.Children.Add(animation);
        }

        public static void AddFadeInBrush(this Storyboard storyboard, float seconds)
        {
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 0,
                To = 1
            };

            Storyboard.SetTargetName(animation, "MyAnimatedBrush3");
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Brush.Opacity)"));
            storyboard.Children.Add(animation);
        }

        public static void AddFadeOut(this Storyboard storyboard, float seconds)
        {
            // Create opacity fade out animation
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 1,
                To = 0
            };
            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            // Add this to the storyboard
            storyboard.Children.Add(animation);
        }

        public static void AddFadeOutBrush(this Storyboard storyboard, float seconds)
        {
            // Create opacity fade out animation
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 1,
                To = 0
            };

            Storyboard.SetTargetName(animation, "MyAnimatedBrush3");
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Brush.Opacity)"));
            storyboard.Children.Add(animation);
        }
    }
}
