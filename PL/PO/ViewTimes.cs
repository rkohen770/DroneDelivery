using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BO;
using BLApi;
using PL;


/// <summary>
///A Class that represents the current station where the bus is in real time 
/// </summary>
namespace PO
{
    public class ViewTimes
    {
        IBL bl = BLFactory.GetBL();
        public static PO.ViewTimes times = new ViewTimes();
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private int upLine;
        public int UpLine
        {
            get => upLine;
            set
            {
                upLine = value;
                OnPropertyChanged(nameof(upLine));
            }
        }
        private int upLineID;
        public int UpLineID
        {
            get => upLineID;
            set
            {
                upLine = value;
                OnPropertyChanged(nameof(upLineID));
            }
        }
        private string upDest;
        public string UpDest
        {
            get => upDest;
            set
            {
                upDest = value;
                OnPropertyChanged(nameof(upDest));
            }
        }
        private TimeSpan lastTime;
        public TimeSpan LastTime
        {
            get => lastTime;
            set
            {
                lastTime = value;
                OnPropertyChanged(nameof(lastTime));
            }
        }

    }
}
