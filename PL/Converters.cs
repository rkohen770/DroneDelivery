using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.IO;

namespace PL
{

    public class WeightCategoriesToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((BO.WeightCategories)value)
            {
                case BO.WeightCategories.Easy:
                    return "/Images/weigh_24px.png";

                case BO.WeightCategories.Intermediate:
                    return "/Images/intermediate_24px.png";

                case BO.WeightCategories.Liver:
                    return "/Images/liver_24px.png";

            }
            return Enum.GetName(typeof(BO.WeightCategories), value) + ".png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class WeightCategoriesConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((BO.WeightCategories)value)
            {
                case BO.WeightCategories.Easy:
                    return "Easy";

                case BO.WeightCategories.Intermediate:
                    return "Intermediate";

                case BO.WeightCategories.Liver:
                    return "Liver";

            }
            return Enum.GetName(typeof(BO.WeightCategories), value) + ".png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PhonePrint : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string PhoneNumber = (string)value;
            string help = PhoneNumber.Substring(0, 3) + "-" + PhoneNumber.Substring(3, 7) ;
            return help;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string PhoneNumber = (string)value;
            string help = PhoneNumber.Substring(0, 3) + "-" + PhoneNumber.Substring(3, 7);
            return help;
        }
    }

    public class DroneStatusToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((BO.DroneStatus)value)
            {
                case BO.DroneStatus.Available:
                    return "/Images/Check_Circle_24px.png";

                case BO.DroneStatus.Maintenance:
                    return "/Images/charging_station_24px.png";

                case BO.DroneStatus.Delivery:
                    return "/Images/drone_24px.png";

            }
            return Enum.GetName(typeof(BO.DroneStatus), value) + ".png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DroneStatusConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((BO.DroneStatus)value)
            {
                case BO.DroneStatus.Available:
                    return "Available";

                case BO.DroneStatus.Maintenance:
                    return "Maintenance";

                case BO.DroneStatus.Delivery:
                    return "Delivery";

            }
            return Enum.GetName(typeof(BO.DroneStatus), value) + ".png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DroneBattery : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)Math.Round((double)value) + "%";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
    }

    public class DroneCurrentLocation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (BO.Location)value;
            return  $"{val.Lattitude:n3}°N, {val.Longitude:n3}°E"; ;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
    }

    public class PrioritiesToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((BO.Priorities)value)
            {
                case BO.Priorities.Normal:
                    return "/Images/Normal_24px.png";

                case BO.Priorities.Fast:
                    return "/Images/Fast_24px.png";

                case BO.Priorities.Emergency:
                    return "/Images/Emergency_24px.png";

            }
            return Enum.GetName(typeof(BO.Priorities), value) + ".png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PrioritiesConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((BO.Priorities)value)
            {
                case BO.Priorities.Normal:
                    return "Normal";

                case BO.Priorities.Fast:
                    return "Fast";

                case BO.Priorities.Emergency:
                    return "Emergency";

            }
            return Enum.GetName(typeof(BO.Priorities), value) + ".png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ParcelStatusToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((BO.ParcelStatus)value)
            {
                case BO.ParcelStatus.Defined:
                    return "/Images/Defined_24px.png";

                case BO.ParcelStatus.Associated:
                    return "/Images/Associated_24px.png";

                case BO.ParcelStatus.WasCollected:
                    return "/Images/WasCollected_24px.png";
                
                case BO.ParcelStatus.Provided:
                    return "/Images/deliverd_box_24px.png";
            }
            return Enum.GetName(typeof(BO.ParcelStatus), value) + ".png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ParcelStatusConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((BO.ParcelStatus)value)
            {
                case BO.ParcelStatus.Defined:
                    return "Defined";

                case BO.ParcelStatus.Associated:
                    return "Associated";

                case BO.ParcelStatus.WasCollected:
                    return "Was Collected";

                case BO.ParcelStatus.Provided:
                    return "Provided";
            }
            return Enum.GetName(typeof(BO.ParcelStatus), value) + ".png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NotBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue)
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}


