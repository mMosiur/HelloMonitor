using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace HelloMonitor {
    /// <summary>
    /// A base page for all pages to garin base functionality
    /// </summary>
    public class BasePage : Page {

        #region Public Properties

        /// <summary>
        /// The animation to play when te page is first loaded
        /// </summary>
        //public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.FadeIn;
        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.FadeInBrush;
        //public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.None;


        /// <summary>
        /// The animation to play when te page is unloaded
        /// </summary>
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.None;
        //public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.FadeOutBrush;

        /// <summary>
        /// The time any slide animation takes to complete
        /// </summary>
        public float SlideSeconds { get; set; } = 0.8f;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage() {
            // If we are animating in, hide to begin with
            if (this.PageLoadAnimation != PageAnimation.None)
                this.Visibility = Visibility.Collapsed;

            // Listen out for the page loading
            this.Loaded += BasePage_Loaded;
            this.Unloaded += BasePage_Unloaded;
            
        }

        #endregion

        #region Animation Load / Unload

        /// <summary>
        /// Once the page is loaded perform any required animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BasePage_Loaded(object sender, RoutedEventArgs e) {
            // Animate the page in
            await AnimateIn();
        }
        private async void BasePage_Unloaded(object sender, RoutedEventArgs e)
        {
            // Animate the page out
            await AnimateOut();
        }

        /// <summary>
        /// Animates in this page
        /// </summary>
        /// <returns></returns>
        public async Task AnimateIn() {
        switch (this.PageLoadAnimation) {
            case PageAnimation.None:
                return;
            //case PageAnimation.FadeIn:
            //    // Start the animation
            //    await this.FadeIn(this.SlideSeconds);
            //    break;
            case PageAnimation.FadeInBrush:
                await this.FadeInBrush(this.SlideSeconds);
                break;
        }
        }
        public async Task AnimateOut()
        {
            switch (this.PageUnloadAnimation)
            {
                case PageAnimation.None:
                    return;
                case PageAnimation.FadeOut:
                    // Start the animation
                    await this.FadeOut(this.SlideSeconds);
                    break;
                case PageAnimation.FadeOutBrush:
                    await this.FadeOutBrush(this.SlideSeconds);
                    break;
            }
        }

        #endregion

    }
}
