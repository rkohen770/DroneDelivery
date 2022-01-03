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
                    return "/Images/easy_24px.png";

                case BO.WeightCategories.Intermediate:
                    return "/Images/medium_24px.png";

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


    public class DroneStatusToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((BO.DroneStatus)value)
            {
                case BO.DroneStatus.Available:
                    return "/Images/Check_Circle_24px.png";

                case BO.DroneStatus.Maintenance:
                    return "/Images/charging_battery_24px.png";

                case BO.DroneStatus.Delivery:
                    return "/Images/drone_in_delivery_24px.png";

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
                    return "/Images/Provided_box_24px.png";
            }
            return Enum.GetName(typeof(BO.ParcelStatus), value) + ".png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
