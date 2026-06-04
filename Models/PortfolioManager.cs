using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SmartDividendTracker.Models;

namespace SmartDividendTracker.Services
{
    public class PortfolioManager
    {
        private readonly string _filePath = "portfolio.json";
        private List<DividendStock> _stocks = new();

        public PortfolioManager()
        {
            LoadPortfolio();
        }

        public List<DividendStock> GetAllStocks() => _stocks;

        public void AddStock(DividendStock stock)
        {
            var existing = _stocks.Find(s => s.Ticker.Equals(stock.Ticker, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                int totalShares = existing.Shares + stock.Shares;
                existing.AveragePrice = ((existing.AveragePrice * existing.Shares) + (stock.AveragePrice * stock.Shares)) / totalShares;
                existing.Shares = totalShares;
                existing.DividendYield = stock.DividendYield; 
                existing.PeRatio = stock.PeRatio;
            }
            else
            {
                _stocks.Add(stock);
            }

            SavePortfolio();
        }

        public void RemoveStock(string ticker)
        {
            _stocks.RemoveAll(s => s.Ticker.Equals(ticker, StringComparison.OrdinalIgnoreCase));
            SavePortfolio();
        }

        public void ClearAll()
        {
            _stocks.Clear();
            SavePortfolio();
        }

        public decimal GetTotalPortfolioValue()
        {
            decimal total = 0;
            foreach (var stock in _stocks) total += stock.TotalValue;
            return total;
        }

        public decimal GetTotalAnnualIncome()
        {
            decimal total = 0;
            foreach (var stock in _stocks) total += stock.CalculateAnnualDividend();
            return total;
        }

        public void PrintSectorDiversification(bool isUa)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================================");
            Console.WriteLine(isUa ? "              ДИВЕРСИФІКАЦІЯ ЗА СЕКТОРАМИ                " : "                SECTOR DIVERSIFICATION                   ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            decimal totalValue = GetTotalPortfolioValue();

            if (totalValue == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(isUa ? "Ваш портфель порожній. Немає даних для діаграми." : "Portfolio is empty. No data for chart.");
                Console.ResetColor();
                return;
            }

            var sectorGroups = new Dictionary<string, decimal>();
            foreach (var stock in _stocks)
            {
                string sector = stock.Sector;
                if (isUa)
                {
                    sector = sector switch
                    {
                        "Technology" => "Технології",
                        "Financials" => "Фінанси",
                        "Healthcare" => "Охорона здоров'я",
                        "Consumer Staples" => "Товари першої необх.",
                        "Consumer Discretionary" => "Споживчі товари",
                        "Energy" => "Енергетика",
                        "Utilities" => "Комун. послуги",
                        "Real Estate" => "Нерухомість",
                        "Industrials" => "Промисловість",
                        "Materials" => "Матеріали",
                        _ => sector
                    };
                }

                if (sectorGroups.ContainsKey(sector))
                    sectorGroups[sector] += stock.TotalValue;
                else
                    sectorGroups[sector] = stock.TotalValue;
            }

            foreach (var group in sectorGroups)
            {
                decimal pct = (group.Value / totalValue) * 100;
                int barLength = (int)(pct / 4); // 1 кубик "█" = 4% ширини екрана

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{group.Key,-22}: ");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(new string('█', barLength));

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(new string('░', 25 - barLength));

                Console.ResetColor();
                Console.WriteLine($" {pct:F1}% (${group.Value:F2})");
            }
        }

        private void SavePortfolio()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_stocks, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception) {}
        }

        private void LoadPortfolio()
        {
            if (File.Exists(_filePath))
            {
                ConsoleHelper.ShowSpinner(LocalizationManager.GetCurrentLanguage() == "uk"
                    ? "Зчитуємо дані портфеля з JSON..."
                    : "Reading portfolio data from JSON...");
                try
                {
                    string json = File.ReadAllText(_filePath);
                    var data = JsonSerializer.Deserialize<List<DividendStock>>(json);
                    if (data != null) _stocks = data;
                }
                catch (Exception)
                {
                    _stocks = new List<DividendStock>();
                }
            }
        }
    }
}
