namespace BrickMoney
{
    public class LegoSetBasicInfo
    {
        public int LegoSetId { get; set; }

        public string NameEN { get; set; }

        public string NameDE { get; set; }

        public int Year { get; set; }

        // Recommended retail price = Unverbindliche Preisempfehlung (UVP)
        public double RrPrice { get; set; }
    }
}
