namespace SmartDividendTracker.Models
{
    // Абстрактний базовий клас для будь-якого активу
    public abstract class Asset
    {
        public string Ticker { get; set; }
        public string Sector { get; set; }
        public decimal AveragePrice { get; set; }
        public int Shares { get; set; }

        public decimal TotalValue => AveragePrice * Shares;

        protected Asset(string ticker, string sector, decimal averagePrice, int shares)
        {
            Ticker = ticker.ToUpper();
            Sector = sector;
            AveragePrice = averagePrice;
            Shares = shares;
        }
    }
}