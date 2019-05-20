﻿using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace HelloMonitor
{
    /// <summary>
    /// Helpers to animate pages in specific ways
    /// </summary>
    public static class PageAnimations
    {
        public static async Task SlideAndFadeInFromLeft(this Page page, float seconds)
        {
            // Create the storyboard
            var sb = new Storyboard();
            // Add slide from left animation
            sb.AddSlideFromLeft(seconds, page.WindowWidth);
            // Add fade in animation
            sb.AddFadeIn(seconds);
            // Start animating
            sb.Begin(page);
            // Make page visible
            page.Visibility = Visibility.Visible;
            //Wait for it to finish
            await Task.Delay((int)(seconds * 1000));
        }
        public static async Task FadeOut(this Page page, float seconds)
        {
            // Create the storyboard
            var sb = new Storyboard();
            // Add fade out animation
            sb.AddFadeOut(seconds);
            // Start animating
            sb.Begin(page);
            // Make page visible
            page.Visibility = Visibility.Visible;
            //Wait for it to finish
            await Task.Delay((int)(seconds * 1000));
        }
    }
}
