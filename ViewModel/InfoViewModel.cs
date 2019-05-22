using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace HelloMonitor {
    /// <summary>
    /// The View Model for the info screen
    /// </summary>
    public class InfoViewModel : BaseViewModel {

        #region Private Member

        

        #endregion

        #region Public Properties

        public string ImageSrc { get; set; }

        public string Description { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public InfoViewModel() {

        }

        #endregion
    }
}
