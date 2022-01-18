namespace DO
{
    public enum WeightCategories { Easy, Intermediate, Liver };
    public enum Priorities { Normal, Fast, Emergency };
    public enum Permission { Managment, Client }
    public class Config
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    //public int OrdinalParcelNumber { get; set; } = 10000;

    //public double vacant { get; set; } = 0.01;
    //public double CarriesLightWeight { get; set; } = 0.015;
    //public double CarriesMediumWeight { get; set; } = 0.018;
    //public double CarriesHeavyWeight { get; set; } = 0.02;
    //public double DroneChargingRate { get; set; } = 0.003;
}
