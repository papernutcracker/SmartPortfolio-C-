using System;
using System.Collections.Generic;
using System.Globalization;
using SmartDividendTracker.Models;
using SmartDividendTracker.Services;

namespace SmartDividendTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            var onboarding = new OnboardingService();
            UserProfile currentUser = onboarding.RunOrLoadProfile();

            // Синхронізуємо мову профілю з менеджером локалізації
            LocalizationManager.SetLanguage(currentUser.Language == "UA" ? "uk" : "en");

            if (currentUser.Experience == ExperienceLevel.Beginner && !currentUser.HasCompletedTutorial)
            {
                TutorialService.RunTutorial(currentUser);
                currentUser.HasCompletedTutorial = true;
                onboarding.SaveProfile(currentUser);
            }

            var portfolioManager = new PortfolioManager();
            ShowMainMenu(currentUser, portfolioManager, onboarding);
        }

        static void ShowMainMenu(UserProfile profile, PortfolioManager portfolioManager, OnboardingService onboarding)
        {
            int lastMainChoice = 0;

            while (true)
            {

                LocalizationManager.SetLanguage(profile.Language == "UA" ? "uk" : "en");

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

                // Локалізація горизонту
                string localizedHorizon = profile.Horizon switch
                {
                    InvestmentHorizon.UpTo5Years => LocalizationManager.Get("Horiz5"),
                    InvestmentHorizon.UpTo10Years => LocalizationManager.Get("Horiz10"),
                    InvestmentHorizon.LongTerm => LocalizationManager.Get("HorizMore"),
                    _ => profile.Horizon.ToString()
                };

                // Локалізація рівня досвіду
                string expText = profile.Experience == ExperienceLevel.Beginner
                    ? LocalizationManager.Get("ExpBeginner")
                    : LocalizationManager.Get("ExpPro");

                string header = $"--- {LocalizationManager.Get("MainMenu")} ---\n" +
                                $"[{LocalizationManager.Get("GoalPrompt")}: {goalsText} | {LocalizationManager.Get("HorizonPrompt")}: {localizedHorizon} | {LocalizationManager.Get("ExpLevel")}: {expText}]";

                var menuOptions = new List<string>
                {
                    LocalizationManager.Get("MenuOpt1"),    // View Portfolio
                    LocalizationManager.Get("CheatSheetOpt"),// Beginner's Cheat Sheet
                    LocalizationManager.Get("EduMenuTitle"),  // Educational Hub
                    LocalizationManager.Get("MenuOpt5"),    // Compound Calculator
                    LocalizationManager.Get("MenuOptGoalCalc"), // НАШ НОВИЙ КАЛЬКУЛЯТОР ЦІЛІ
                    LocalizationManager.Get("MenuOpt4"),    // Profile Settings
                    LocalizationManager.Get("MenuOpt3")     // Exit
                };

                int choice = ConsoleHelper.SelectOption(header, menuOptions, lastMainChoice);
                lastMainChoice = choice;

                // Перерозподіл індексів обробки кліків
                if (choice == 0) ShowPortfolioMenu(portfolioManager);
                else if (choice == 1) CheatSheetService.Show(profile.Language == "UA");
                else if (choice == 2) TutorialService.ShowMenu(profile);
                else if (choice == 3) CompoundCalculatorService.RunCalculator(profile.Language == "UA");
                else if (choice == 4) GoalCalculatorService.Run(profile.Language == "UA"); // Запуск нового калькулятора
                else if (choice == 5) onboarding.OpenSettings(profile);
                else if (choice == 6)
                {
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
                decimal totalValue = portfolioManager.GetTotalPortfolioValue();
                string header = $"--- {LocalizationManager.Get("PortfolioMenu")} ---\n" +
                                $"[{LocalizationManager.Get("TotalValue")}: ${totalValue:F2}]";

                var options = new List<string>
                {
                    LocalizationManager.Get("ViewAssets"),
                    LocalizationManager.Get("AddStock"),
                    LocalizationManager.Get("RemoveStock"),
                    LocalizationManager.Get("ClearPortfolio"),
                    LocalizationManager.Get("Back")
                };

                int choice = ConsoleHelper.SelectOption(header, options, lastChoice);
                lastChoice = choice;

                if (choice == 0) // View Assets
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

                        decimal totalIncome = portfolioManager.GetTotalAnnualIncome();
                        decimal avgYield = totalValue > 0 ? (totalIncome / totalValue) * 100m : 0m;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n{LocalizationManager.Get("TotalValue")}: ${totalValue:F2}");
                        Console.WriteLine($"{LocalizationManager.Get("TotalIncome")} ${totalIncome:F2} ({LocalizationManager.Get("TblYield")}: {avgYield:F2}%)");
                        Console.ResetColor();
                    }

                    Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
                    Console.ReadKey(true);
                }
                else if (choice == 1) // Add Stock
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("=======================================");
                    Console.WriteLine($"  {LocalizationManager.Get("AddStock").ToUpper()}");
                    Console.WriteLine("=======================================\n");
                    Console.ResetColor();

                    Console.Write(LocalizationManager.Get("EnterTicker"));
                    string ticker = Console.ReadLine()?.Trim().ToUpper() ?? "UNKNOWN";

                    var sectors = new List<string> {
                        "Technology", "Financials", "Healthcare", "Consumer Staples",
                        "Consumer Discretionary", "Energy", "Utilities", "Real Estate", "Industrials", "Materials"
                    };

                    int sectorChoice = ConsoleHelper.SelectOption(LocalizationManager.Get("SelectSector"), sectors);
                    string sector = sectors[sectorChoice];

                    decimal price = ReadDecimalInput(LocalizationManager.Get("EnterPrice"));
                    int shares = ReadIntInput(LocalizationManager.Get("EnterShares"));
                    decimal divYield = ReadDecimalInput(LocalizationManager.Get("EnterYield"));
                    decimal peRatio = ReadDecimalInput(LocalizationManager.Get("EnterPE"));

                    var newStock = new DividendStock(ticker, sector, price, shares, divYield, peRatio);
                    portfolioManager.AddStock(newStock);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{LocalizationManager.Get("StockAdded")}");
                    Console.ResetColor();

                    Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
                    Console.ReadKey(true);
                }
                else if (choice == 2) // Remove Stock
                {
                    var stocks = portfolioManager.GetAllStocks();

                    if (stocks.Count == 0)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n{LocalizationManager.Get("EmptyPort")}");
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                    else
                    {
                        var removeOptions = new List<string>();
                        foreach (var stock in stocks)
                        {
                            removeOptions.Add($"{stock.Ticker,-6} | {stock.Sector} | {stock.Shares} - ${stock.TotalValue:F2}");
                        }
                        removeOptions.Add($"[ {LocalizationManager.Get("Cancel")} ]");

                        int removeChoice = ConsoleHelper.SelectOption(LocalizationManager.Get("SelectToRemove"), removeOptions);

                        if (removeChoice < stocks.Count)
                        {
                            string tickerToRemove = stocks[removeChoice].Ticker;
                            portfolioManager.RemoveStock(tickerToRemove);

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\n{LocalizationManager.Get("StockRemoved")}");
                            Console.ResetColor();
                            Console.ReadKey(true);
                        }
                    }
                }
                else if (choice == 3) // Clear Portfolio
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("=======================================");
                    Console.WriteLine($"  {LocalizationManager.Get("ClearPortfolio").ToUpper()}");
                    Console.WriteLine("=======================================\n");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(LocalizationManager.Get("ClearConfirm"));
                    Console.ResetColor();

                    string confirmation = Console.ReadLine()?.Trim().ToUpper() ?? "";

                    if (confirmation == "YES" || confirmation == "ТАК")
                    {
                        portfolioManager.ClearAll();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n{LocalizationManager.Get("PortfolioCleared")}");
                        Console.ResetColor();
                    }
                    else
                    {
                        // Яскравий заспокійливий варнінг, якщо користувач передумав або помилився
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n⚠️ {LocalizationManager.Get("ClearCanceled")}");
                        Console.ResetColor();
                    }

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
                    Console.ResetColor();
                    Console.ReadKey(true);
                }
                else if (choice == 4) // Back
                {
                    break;
                }
            }
        }

        // Культуро-незалежні хелпери для введення даних
        private static decimal ReadDecimalInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Replace(",", ".") ?? "";
                if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value) && value >= 0)
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(LocalizationManager.Get("InvalidInput"));
                Console.ResetColor();
            }
        }

        private static int ReadIntInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";
                if (int.TryParse(input, out int value) && value > 0)
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(LocalizationManager.Get("InvalidInput"));
                Console.ResetColor();
            }
        }
    }
}