namespace SmartDividendTracker.Models
{
    // Клас дивідендної акції, що успадковує Asset
    public class DividendStock : Asset
    {
        public decimal DividendYield { get; set; } // У відсотках
        public decimal PeRatio { get; set; }

        public DividendStock(string ticker, string sector, decimal averagePrice, int shares, decimal dividendYield, decimal peRatio)
            : base(ticker, sector, averagePrice, shares)
        {
            DividendYield = dividendYield;
            PeRatio = peRatio;
        }

        // Прогнозований річний дохід з цієї конкретної позиції
        public decimal CalculateAnnualDividend()
        {
            return TotalValue * (DividendYield / 100m);
        }
    }
}