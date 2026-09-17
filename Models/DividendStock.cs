namespace SmartDividendTracker.Models
{
    public class DividendStock : Asset
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; } // Додаємо зв'язок з користувачем
        public decimal DividendYield { get; set; }
        public decimal PeRatio { get; set; }

        public string Currency { get; set; } = "USD";


        // Нове поле, яке буде зберігатися в базі даних
        public decimal CurrentPrice { get; set; }

        // Обчислювані поля (вони рахуються автоматично і не завантажують базу)
        public decimal TotalCost => Math.Round(Shares * AveragePrice, 2);
        public decimal CurrentValue => Math.Round(Shares * CurrentPrice, 2);
        public decimal ProfitLoss => Math.Round(CurrentValue - TotalCost, 2);
        public string ProfitLossStr => ProfitLoss >= 0 ? $"+{ProfitLoss}" : ProfitLoss.ToString();

        public DividendStock(string ticker, string sector, decimal averagePrice, decimal shares, decimal dividendYield, decimal peRatio)
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