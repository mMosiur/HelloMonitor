using System;
using System.Diagnostics;
using System.Globalization;

namespace HelloMonitor {
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> into an actual view/page
    /// </summary>
    class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter> {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            // Find the appropriate page
            switch ((ApplicationPage)value) {
                case ApplicationPage.Info:
                    return new InfoPage();
                case ApplicationPage.Insta:
                    return new InstaPage();
                case ApplicationPage.Buses:
                    return new BusesPage();
                case ApplicationPage.Dab:
                    return new DabPage();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
