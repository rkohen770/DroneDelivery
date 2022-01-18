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
    public class Config_1
    {
        public static int OrdinalParcelNumber  = 10000;

        public static double vacant = 0.01;
        public static double CarriesLightWeight  = 0.015;
        public static double CarriesMediumWeight = 0.018;
        public static double CarriesHeavyWeight = 0.02;
        public static double DroneChargingRate = 0.003;
    }
}
