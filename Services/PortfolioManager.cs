using System;
using System.Collections.Generic;
using System.Linq;
using SmartDividendTracker.Data;
using SmartDividendTracker.Models;

namespace Smart_Dividend_Portfolio_Tracker.Services
{
    public class PortfolioManager
    {
        // 1. Метод отримання всіх акцій (читає прямо з SQL Server)
        public List<DividendStock> GetAllStocks(int userId)
        {
            using (var db = new AppDbContext())
            {
                return db.Stocks.Where(s => s.UserProfileId == userId).ToList();
            }
        }

        public void AddStock(DividendStock stock, int userId)
        {
            using (var db = new AppDbContext())
            {
                stock.UserProfileId = userId; // Прив'язуємо акцію до користувача
                var existing = db.Stocks.FirstOrDefault(s => s.Ticker.ToLower() == stock.Ticker.ToLower() && s.UserProfileId == userId);

                if (existing != null)
                {
                    // Якщо є — оновлюємо її середню ціну та кількість
                    int totalShares = existing.Shares + stock.Shares;
                    existing.AveragePrice = ((existing.AveragePrice * existing.Shares) + (stock.AveragePrice * stock.Shares)) / totalShares;
                    existing.Shares = totalShares;
                    existing.DividendYield = stock.DividendYield;
                    existing.PeRatio = stock.PeRatio;
                }
                else
                {
                    // Якщо немає — додаємо як нову
                    db.Stocks.Add(stock);
                }

                // Зберігаємо зміни в базу даних
                db.SaveChanges();
            }
        }

        // 3. Метод видалення акції
        public void RemoveStock(string ticker)
        {
            using (var db = new AppDbContext())
            {
                var stockToRemove = db.Stocks.FirstOrDefault(s => s.Ticker.ToLower() == ticker.ToLower());
                if (stockToRemove != null)
                {
                    db.Stocks.Remove(stockToRemove);
                    db.SaveChanges();
                }
            }
        }

        // 4. Метод повного очищення портфеля (видаляє всі рядки з таблиці Stocks)
        public void ClearAll(int userId)
        {
            using (var db = new AppDbContext())
            {
                // Видаляємо акції тільки конкретного профілю, а не всю таблицю!
                var userStocks = db.Stocks.Where(s => s.UserProfileId == userId);
                db.Stocks.RemoveRange(userStocks);
                db.SaveChanges();
            }
        }

        // 5. Отримання загальної вартості (рахуємо з бази)
        public decimal GetTotalPortfolioValue(int userId)
        {
            using (var db = new AppDbContext())
            {
                return db.Stocks
                         .Where(s => s.UserProfileId == userId)
                         .AsEnumerable()
                         .Sum(s => s.TotalValue);
            }
        }

        // 6. Отримання річного доходу (рахуємо з бази)
        public decimal GetTotalAnnualIncome(int userId)
        {
            using (var db = new AppDbContext())
            {
                return db.Stocks
                         .Where(s => s.UserProfileId == userId)
                         .AsEnumerable()
                         .Sum(s => s.CalculateAnnualDividend());
            }
        }

        // 7. Виведення діаграми
        public void PrintSectorDiversification(bool isUa)
        {
            List<DividendStock> stocks;
            using (var db = new AppDbContext())
            {
                stocks = db.Stocks.ToList(); // Беремо всі акції для аналізу
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================================");
            Console.WriteLine(isUa ? "              ДИВЕРСИФІКАЦІЯ ЗА СЕКТОРАМИ                " : "                SECTOR DIVERSIFICATION                   ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            decimal totalValue = stocks.Sum(s => s.TotalValue);

            if (totalValue == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(isUa ? "Ваш портфель порожній. Немає даних для діаграми." : "Portfolio is empty. No data for chart.");
                Console.ResetColor();
                return;
            }

            var sectorGroups = new Dictionary<string, decimal>();
            foreach (var stock in stocks)
            {
                string sector = stock.Sector ?? "Unknown";
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
                int barLength = (int)(pct / 4);

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
    }
}