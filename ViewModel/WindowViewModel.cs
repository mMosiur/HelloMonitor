using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HelloMonitor
{
    /// <summary>
    /// The View Model for the custom flat window
    /// </summary>
    public class WindowViewModel : BaseViewModel
    {

        #region Private Member

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        private int mOuterMarginSize = 10;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int mWindowRadius = 10;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;

        /// <summary>
        /// The variable storing the current page view model
        /// </summary>
        private IPageViewModel mCurrentPageViewModel;

        /// <summary>
        /// The list storing all page view models
        /// </summary>
        private List<IPageViewModel> mPageViewModels;

        #endregion

        #region Public Properties

        /// <summary>
        /// The smallest width the window can go to
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 400;

        /// <summary>
        /// The smallest height the window can go to
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;

        /// <summary>
        /// True if the window should be borderless because it is docked or maximized
        /// </summary>
        public bool Borderless { get { return (mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked); } }

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder { get { return Borderless ? 0 : 6; } }

        /// <summary>
        /// The size of the resize border around the window, taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }

        /// <summary>
        /// The padding of the inner content of the main window
        /// </summary>
        public Thickness InnerContentPadding { get { return new Thickness(ResizeBorder); } }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public int OuterMarginSize {
            get {
                // If it is maximized or docked, no border
                return Borderless ? 0 : mOuterMarginSize;
            }
            set {
                mOuterMarginSize = value;
            }
        }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius {
            get {
                // If it is maximized or docked, no border
                return Borderless ? 0 : mWindowRadius;
            }
            set {
                mWindowRadius = value;
            }
        }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public int TitleHeight { get; set; } = 42;
        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorder); } }

        /// <summary>
        /// The list containing all view models
        /// </summary>
        public List<IPageViewModel> PageViewModels {
            get {
                if (mPageViewModels == null) mPageViewModels = new List<IPageViewModel>();
                return mPageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel {
            get { return mCurrentPageViewModel; }
            set {
                mCurrentPageViewModel = value;
                OnPropertyChanged("CurrentPageViewModel");
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// The command to show the system menu of the window
        /// </summary>
        public ICommand MenuCommand { get; set; }

        #endregion

        #region Private Functions

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);
        }

        private void OnGo1Screen(object obj)
        {
            ChangeViewModel(PageViewModels[0]);
        }

        private void OnGo2Screen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
        }

        private void OnGo3Screen(object obj)
        {
            ChangeViewModel(PageViewModels[2]);
        }

        private void OnGo4Screen(object obj)
        {
            ChangeViewModel(PageViewModels[3]);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowViewModel(Window window)
        {
            mWindow = window;

            // Listen out for the window resizing
            mWindow.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResized();
            };

            // Create commands
            //MenuCommand = new RelayCommand(x => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

            // Fix window resize issue
            var resizer = new WindowResizer(mWindow);

            // Listen out for dock changes
            resizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                mDockPosition = dock;
                // Fire off resize events
                WindowResized();
            };

            PageViewModels.Add(new InfoPageViewModel());
            PageViewModels.Add(new InstaPageViewModel());
            PageViewModels.Add(new BusesPageViewModel());
            PageViewModels.Add(new DabPageViewModel());

            CurrentPageViewModel = PageViewModels[0];

            Mediator.Subscribe("GoTo1Screen", OnGo1Screen);
            Mediator.Subscribe("GoTo2Screen", OnGo2Screen);
            Mediator.Subscribe("GoTo3Screen", OnGo3Screen);
            Mediator.Subscribe("GoTo4Screen", OnGo4Screen);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(mWindow);

            // Add the window position so its a "ToScreen"
            return new Point(position.X + (Borderless ? 0 : mWindow.Left), position.Y + (Borderless ? 0 : mWindow.Top));
        }

        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            // Fire off events for all properties that are affected by a resize
            OnPropertyChanged(nameof(Borderless));
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(OuterMarginSize));
            OnPropertyChanged(nameof(OuterMarginSizeThickness));
            OnPropertyChanged(nameof(WindowRadius));
            OnPropertyChanged(nameof(WindowCornerRadius));
        }


        #endregion
    }
}
