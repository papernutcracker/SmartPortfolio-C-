using System;
using System.Collections.Generic;
using SmartDividendTracker.Models;
using SmartDividendTracker.Services;

namespace SmartDividendTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            // Налаштування кодування для української мови
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            // Ініціалізація сервісів
            var onboarding = new OnboardingService();
            UserProfile currentUser = onboarding.RunOrLoadProfile();

            // Встановлюємо мову
            LocalizationManager.SetLanguage(currentUser.Language);

            var portfolioManager = new PortfolioManager();

            // Запускаємо головне меню
            ShowMainMenu(currentUser, portfolioManager, onboarding);
        }

        static void ShowMainMenu(UserProfile profile, PortfolioManager portfolioManager, OnboardingService onboarding)
        {
            int lastMainChoice = 0;

            while (true)
            {
                // Формуємо список цілей для заголовка
                var localizedGoalsList = new List<string>();
                foreach (var goal in profile.Goals)
                {
                    localizedGoalsList.Add(goal switch
                    {
                        InvestmentGoal.PassiveIncome => LocalizationManager.Get("GoalPassive"),
                        InvestmentGoal.CapitalGrowth => LocalizationManager.Get("GoalGrowth"),
                        InvestmentGoal.MajorPurchase => LocalizationManager.Get("GoalPurchase"),
                        _ => goal.ToString()
                    });
                }
                string goalsText = string.Join(", ", localizedGoalsList);

                // Формуємо горизонт
                string localizedHorizon = profile.Horizon switch
                {
                    InvestmentHorizon.UpTo5Years => LocalizationManager.Get("Horiz5"),
                    InvestmentHorizon.UpTo10Years => LocalizationManager.Get("Horiz10"),
                    InvestmentHorizon.LongTerm => LocalizationManager.Get("HorizMore"),
                    _ => profile.Horizon.ToString()
                };

                string header = $"{LocalizationManager.Get("MainMenu")}\n" +
                                $"[Goals: {goalsText} | Horizon: {localizedHorizon} | Level: {profile.Experience}]";

                var menuOptions = new List<string>
                {
                    LocalizationManager.Get("MenuOpt1"), // View Portfolio
                    LocalizationManager.Get("MenuOpt2"), // AI Analysis
                    LocalizationManager.Get("MenuOpt4"), // Profile Settings
                    LocalizationManager.Get("MenuOpt3")  // Exit
                };

                int choice = ConsoleHelper.SelectOption(header, menuOptions, lastMainChoice);
                lastMainChoice = choice;

                string selectedText = menuOptions[choice];

                if (selectedText == LocalizationManager.Get("MenuOpt1"))
                {
                    // Вхід у підменю портфеля
                    ShowPortfolioMenu(portfolioManager);
                }
                else if (selectedText == LocalizationManager.Get("MenuOpt2"))
                {
                    Console.WriteLine("\n[AI Market Analysis is under construction.]");
                    Console.WriteLine("Press any key to return...");
                    Console.ReadKey(true);
                }
                else if (selectedText == LocalizationManager.Get("MenuOpt4"))
                {
                    // Вхід у налаштування
                    onboarding.OpenSettings(profile);
                }
                else if (selectedText == LocalizationManager.Get("MenuOpt3"))
                {
                    // Вихід з анімацією
                    ConsoleHelper.ShowExitAnimation(LocalizationManager.Get("ExitMessage"));
                    break;
                }
            }
        }

        static void ShowPortfolioMenu(PortfolioManager portfolioManager)
        {
            int lastChoice = 0;

            while (true)
            {
                // Ця змінна totalValue рахується тут і використовується у всьому меню
                decimal totalValue = portfolioManager.GetTotalPortfolioValue();

                string header = $"{LocalizationManager.Get("PortfolioMenu")}\n" +
                                $"[Total Value: ${totalValue:F2}]";

                var options = new List<string>
                {
                    LocalizationManager.Get("ViewAssets"),
                    LocalizationManager.Get("AddStock"),
                    LocalizationManager.Get("RemoveStock"),
                    LocalizationManager.Get("Back")
                };

                int choice = ConsoleHelper.SelectOption(header, options, lastChoice);
                lastChoice = choice;

                string selectedText = options[choice];

                // --- 1. ПЕРЕГЛЯД АКТИВІВ (ТАБЛИЦЯ) ---
                if (selectedText == LocalizationManager.Get("ViewAssets"))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("=========================================================================================");
                    Console.WriteLine($"  {LocalizationManager.Get("ViewAssets").ToUpper()}");
                    Console.WriteLine("=========================================================================================\n");
                    Console.ResetColor();

                    var stocks = portfolioManager.GetAllStocks();

                    if (stocks.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(LocalizationManager.Get("EmptyPort"));
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"| {LocalizationManager.Get("TblTicker"),-6} | {LocalizationManager.Get("TblSector"),-22} | {LocalizationManager.Get("TblPrice"),8} | {LocalizationManager.Get("TblShares"),5} | {LocalizationManager.Get("TblValue"),12} | {LocalizationManager.Get("TblYield"),7} | {LocalizationManager.Get("TblAnnualDiv"),10} |");
                        Console.ResetColor();
                        Console.WriteLine(new string('-', 89));

                        foreach (var stock in stocks)
                        {
                            decimal annualDiv = stock.CalculateAnnualDividend();
                            Console.WriteLine($"| {stock.Ticker,-6} | {stock.Sector,-22} | {stock.AveragePrice,8:F2} | {stock.Shares,5} | {stock.TotalValue,12:F2} | {stock.DividendYield,6:F2}% | {annualDiv,10:F2} |");
                        }
                        Console.WriteLine(new string('-', 89));

                        // Використовуємо вже існуючу змінну totalValue!
                        decimal totalIncome = portfolioManager.GetTotalAnnualIncome();
                        decimal avgYield = totalValue > 0 ? (totalIncome / totalValue) * 100m : 0m;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n{LocalizationManager.Get("TblValue")}: ${totalValue:F2}");
                        Console.WriteLine($"{LocalizationManager.Get("TotalIncome")} ${totalIncome:F2} (Yield: {avgYield:F2}%)");
                        Console.ResetColor();
                    }

                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey(true);
                }

                // --- 2. ДОДАВАННЯ НОВОГО АКТИВУ ---
                else if (selectedText == LocalizationManager.Get("AddStock"))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("=======================================");
                    Console.WriteLine(LocalizationManager.Get("AddStock").ToUpper());
                    Console.WriteLine("=======================================\n");
                    Console.ResetColor();

                    Console.Write(LocalizationManager.Get("EnterTicker"));
                    // Додали ?? "" щоб уникнути помилок CS8600
                    string ticker = Console.ReadLine()?.Trim().ToUpper() ?? "";
                    if (string.IsNullOrEmpty(ticker)) ticker = "UNKNOWN";

                    var sectors = new List<string> {
                        "Technology", "Financials", "Healthcare", "Consumer Staples",
                        "Consumer Discretionary", "Energy", "Utilities", "Real Estate", "Industrials", "Materials"
                    };
                    int sectorChoice = ConsoleHelper.SelectOption(LocalizationManager.Get("SelectSector"), sectors);
                    string sector = sectors[sectorChoice];

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"--- Adding: {ticker} ({sector}) ---\n");
                    Console.ResetColor();

                    decimal price = 0;
                    while (true)
                    {
                        Console.Write(LocalizationManager.Get("EnterPrice"));
                        string input = Console.ReadLine()?.Replace(".", ",") ?? "";
                        if (decimal.TryParse(input, out price) && price >= 0) break;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(LocalizationManager.Get("InvalidInput"));
                        Console.ResetColor();
                    }

                    int shares = 0;
                    while (true)
                    {
                        Console.Write(LocalizationManager.Get("EnterShares"));
                        string input = Console.ReadLine() ?? "";
                        if (int.TryParse(input, out shares) && shares > 0) break;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(LocalizationManager.Get("InvalidInput"));
                        Console.ResetColor();
                    }

                    decimal divYield = 0;
                    while (true)
                    {
                        Console.Write(LocalizationManager.Get("EnterYield"));
                        string input = Console.ReadLine()?.Replace(".", ",") ?? "";
                        if (decimal.TryParse(input, out divYield) && divYield >= 0) break;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(LocalizationManager.Get("InvalidInput"));
                        Console.ResetColor();
                    }

                    decimal peRatio = 0;
                    while (true)
                    {
                        Console.Write(LocalizationManager.Get("EnterPE"));
                        string input = Console.ReadLine()?.Replace(".", ",") ?? "";
                        if (decimal.TryParse(input, out peRatio) && peRatio >= 0) break;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(LocalizationManager.Get("InvalidInput"));
                        Console.ResetColor();
                    }

                    var newStock = new DividendStock(ticker, sector, price, shares, divYield, peRatio);
                    portfolioManager.AddStock(newStock);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{LocalizationManager.Get("StockAdded")}");
                    Console.ResetColor();

                    Console.WriteLine("Press any key to return...");
                    Console.ReadKey(true);
                }

                // --- 3. ВИДАЛЕННЯ АКТИВУ ---
                else if (selectedText == LocalizationManager.Get("RemoveStock"))
                {
                    var stocks = portfolioManager.GetAllStocks();

                    if (stocks.Count == 0)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n{LocalizationManager.Get("EmptyPort")}");
                        Console.ResetColor();
                        Console.WriteLine("\nPress any key to return...");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        // Формуємо красивий список наявних акцій для меню
                        var removeOptions = new List<string>();
                        foreach (var stock in stocks)
                        {
                            removeOptions.Add($"{stock.Ticker,-6} | {stock.Sector} | {stock.Shares} shares | ${stock.TotalValue:F2}");
                        }

                        // Додаємо кнопку скасування в кінець списку
                        removeOptions.Add($"[ {LocalizationManager.Get("Cancel")} ]");
                        
                        // Викликаємо наше меню зі стрілочками
                        int removeChoice = ConsoleHelper.SelectOption(LocalizationManager.Get("SelectToRemove"), removeOptions);

                        // Перевіряємо, чи користувач не натиснув "Скасувати" (це останній індекс)
                        if (removeChoice < stocks.Count)
                        {
                            string tickerToRemove = stocks[removeChoice].Ticker;

                            // Викликаємо метод видалення з нашого PortfolioManager
                            portfolioManager.RemoveStock(tickerToRemove);

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\n{LocalizationManager.Get("StockRemoved")} ({tickerToRemove})");
                            Console.ResetColor();
                            Console.WriteLine("\nPress any key to return...");
                            Console.ReadKey(true);
                        }
                    }
                }

                // --- 4. НАЗАД ---
                else if (selectedText == LocalizationManager.Get("Back"))
                {
                    break;
                }
            }
        }
    }
}