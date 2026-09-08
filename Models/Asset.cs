namespace SmartDividendTracker.Models
{
    public abstract class Asset
    {
        public string Ticker { get; set; }
        public string Sector { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal Shares { get; set; }

        public decimal TotalValue => AveragePrice * Shares;

        protected Asset(string ticker, string sector, decimal averagePrice, decimal shares)
        {
            Ticker = ticker.ToUpper();
            Sector = sector;
            AveragePrice = averagePrice;
            Shares = shares;
        }
    }
}