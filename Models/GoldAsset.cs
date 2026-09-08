namespace SmartDividendTracker.Models
{
    public class GoldAsset : Asset
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public decimal WeightInGrams { get; set; }

        public GoldAsset(string ticker, string sector, decimal averagePrice, decimal weightInGrams)
            : base(ticker, sector, averagePrice, weightInGrams) 
        {
            WeightInGrams = weightInGrams;
        }

        public decimal CalculateGoldValue()
        {
            return AveragePrice * WeightInGrams;
        }
    }
}