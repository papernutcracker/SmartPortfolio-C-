using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartDividendTracker.Models
{
    public class PortfolioManager
    {
        // List where all added stocks are stored
        private List<DividendStock> _stocks = new List<DividendStock>();

        public void AddStock(DividendStock stock)
        {
            _stocks.Add(stock);
        }

        public void RemoveStock(string ticker)
        {
            // Find the stock by ticker, ignoring case (uppercase/lowercase)
            var stockToRemove = _stocks.FirstOrDefault(s => s.Ticker.Equals(ticker, StringComparison.OrdinalIgnoreCase));

            if (stockToRemove != null)
            {
                _stocks.Remove(stockToRemove);
            }
        }

        public IReadOnlyList<DividendStock> GetAllStocks()
        {
            return _stocks.AsReadOnly();
        }

        public decimal GetTotalPortfolioValue()
        {
            // Calculate the total value of the entire portfolio
            return _stocks.Sum(s => s.TotalValue);
        }

        public decimal GetTotalAnnualIncome()
        {
            // Calculate total annual dividend income
            return _stocks.Sum(s => s.CalculateAnnualDividend());
        }

        // The method to completely clear the portfolio
        public void ClearAll()
        {
            _stocks.Clear();
        }

        // Method for calculating diversification (useful for future analytics)
        public Dictionary<string, decimal> GetSectorAllocation()
        {
            var totalValue = GetTotalPortfolioValue();
            if (totalValue == 0) return new Dictionary<string, decimal>();

            return _stocks.GroupBy(s => s.Sector)
                          .ToDictionary(
                              g => g.Key,
                              g => (g.Sum(s => s.TotalValue) / totalValue) * 100m
                          );
        }
    }
}