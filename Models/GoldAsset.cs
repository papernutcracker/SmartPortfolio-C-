namespace SmartDividendTracker.Models
{
    public class GoldAsset : Asset
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public decimal WeightInGrams { get; set; } // Вага золота

        public GoldAsset(string ticker, string sector, decimal averagePrice, decimal weightInGrams)
            : base(ticker, sector, averagePrice, (int)weightInGrams)
        {
            WeightInGrams = weightInGrams;
        }

        // Можна додати специфічний метод розрахунку вартості золота
        public decimal CalculateGoldValue()
        {
            return AveragePrice * WeightInGrams;
        }
    }
}