namespace SmartDividendTracker.Models
{
    public class DividendStock : Asset
    {
        public int Id { get; set; }
        public decimal DividendYield { get; set; }
        public decimal PeRatio { get; set; }

        public DividendStock(string ticker, string sector, decimal averagePrice, int shares, decimal dividendYield, decimal peRatio)
            : base(ticker, sector, averagePrice, shares)
        {
            DividendYield = dividendYield;
            PeRatio = peRatio;
        }

        public decimal CalculateAnnualDividend()
        {
            return TotalValue * (DividendYield / 100m);
        }
    }
}