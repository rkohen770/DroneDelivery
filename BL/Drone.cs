using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Drone: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int DroneId { get; set; }
        public string DroneModel { get; set; }
        public WeightCategories Weight { get; set; }
        public double DroneBattery {
            get => DroneBattery;
            set
            {
                DroneBattery = value;
                OnPropertyChanged(nameof(DroneBattery));
            }
        }
        public DroneStatus? DroneStatus { get; set; }
        public ParcelInTransfer ParcelInTransfer { get; set; }
        public Location CurrentLocation { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty() + "\n";
        }

    }
}