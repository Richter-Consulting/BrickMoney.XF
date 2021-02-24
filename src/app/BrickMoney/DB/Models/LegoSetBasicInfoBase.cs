namespace BrickMoney
{
    public abstract class LegoSetBasicInfoBase
    {
        public string NameEN { get; set; }

        public string NameDE { get; set; }

        public int Year { get; set; }

        // Recommended retail price = Unverbindliche Preisempfehlung (UVP)
        public double RrPrice { get; set; } 
    }
}
