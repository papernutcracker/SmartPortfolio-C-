using System;
using System.Collections.Generic;
using System.Linq;
using SmartDividendTracker.Models;

namespace SmartDividendTracker.Services
{
    public class PortfolioManager
    {
        private List<DividendStock> _stocks;

        public PortfolioManager()
        {
            _stocks = new List<DividendStock>();
        }

        public void AddStock(DividendStock stock)
        {
            var existingStock = _stocks.FirstOrDefault(s => s.Ticker == stock.Ticker);
            if (existingStock != null)
            {
                _stocks.Remove(existingStock);
            }
            _stocks.Add(stock);
        }

        public bool RemoveStock(string ticker)
        {
            var stock = _stocks.FirstOrDefault(s => s.Ticker == ticker.ToUpper());
            if (stock != null)
            {
                _stocks.Remove(stock);
                return true;
            }
            return false;
        }

        public decimal GetTotalPortfolioValue() => _stocks.Sum(s => s.TotalValue);

        public decimal GetTotalAnnualIncome() => _stocks.Sum(s => s.CalculateAnnualDividend());

        public Dictionary<string, decimal> GetSectorAllocation()
        {
            decimal totalValue = GetTotalPortfolioValue();
            if (totalValue == 0) return new Dictionary<string, decimal>();

            return _stocks
                .GroupBy(s => s.Sector)
                .ToDictionary(
                    group => group.Key,
                    group => Math.Round((group.Sum(s => s.TotalValue) / totalValue) * 100m, 2)
                );
        }

        public IReadOnlyList<DividendStock> GetAllStocks() => _stocks.AsReadOnly();
    }
}