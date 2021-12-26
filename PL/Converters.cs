using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace PL
{
    public class DroneID : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value;

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value;

        }
    }
    public class DroneStatusToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((BO.DroneStatus)value)
            {
                case BO.DroneStatus.Available:
                    return "/images/Check Circle_24px.png";
                case BO.DroneStatus.Maintenance:
                    return "/images/maintenance_24px.png";

                case BO.DroneStatus.Delivery:
                    return "/images/home_24px.png";

            }
            return Enum.GetName(typeof(BO.DroneStatus), value) + ".png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
}
