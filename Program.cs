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
            // Set console encoding to UTF-8
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            var onboarding = new OnboardingService();
            UserProfile currentUser = onboarding.RunOrLoadProfile();

            LocalizationManager.SetLanguage(currentUser.Language);

            // --- ЗАПУСК ІГРОВОГО ТУТОРІАЛУ ДЛЯ НОВАЧКІВ ---
            if (currentUser.Experience == ExperienceLevel.Beginner && !currentUser.HasCompletedTutorial)
            {
                TutorialService.RunTutorial();

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
                    LocalizationManager.Get("MenuOpt2"), // Beginner's Cheat Sheet
                    LocalizationManager.Get("MenuOpt5"), // Compound Calculator
                    LocalizationManager.Get("MenuOpt4"), // Profile Settings
                    LocalizationManager.Get("MenuOpt3")  // Exit
                };

                int choice = ConsoleHelper.SelectOption(header, menuOptions, lastMainChoice);
                lastMainChoice = choice;

                string selectedText = menuOptions[choice];

                if (selectedText == LocalizationManager.Get("MenuOpt1"))
                {
                    ShowPortfolioMenu(portfolioManager);
                }
                else if (selectedText == LocalizationManager.Get("MenuOpt2"))
                {
                    CheatSheetService.Show();
                }
                else if (selectedText == LocalizationManager.Get("MenuOpt5"))
                {
                    CompoundCalculatorService.RunCalculator();
                }
                else if (selectedText == LocalizationManager.Get("MenuOpt4"))
                {
                    onboarding.OpenSettings(profile);
                }
                else if (selectedText == LocalizationManager.Get("MenuOpt3"))
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

                string header = $"{LocalizationManager.Get("PortfolioMenu")}\n" +
                                $"[Total Value: ${totalValue:F2}]";

                var options = new List<string>
                {
                    LocalizationManager.Get("ViewAssets"),
                    LocalizationManager.Get("AddStock"),
                    LocalizationManager.Get("RemoveStock"),
                    LocalizationManager.Get("ClearPortfolio"), // НОВА КНОПКА ОЧИЩЕННЯ
                    LocalizationManager.Get("Back")
                };

                int choice = ConsoleHelper.SelectOption(header, options, lastChoice);
                lastChoice = choice;

                string selectedText = options[choice];

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
                        Console.WriteLine($"| {"Ticker",-6} | {"Sector",-22} | {"Price",8} | {"Shs",5} | {"Value",12} | {"Yield",7} | {"AnnualDiv",10} |");
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
                        Console.WriteLine($"\n{LocalizationManager.Get("TblValue")}: ${totalValue:F2}");
                        Console.WriteLine($"{LocalizationManager.Get("TotalIncome")} ${totalIncome:F2} (Yield: {avgYield:F2}%)");
                        Console.ResetColor();
                    }

                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey(true);
                }

                else if (selectedText == LocalizationManager.Get("AddStock"))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("=======================================");
                    Console.WriteLine(LocalizationManager.Get("AddStock").ToUpper());
                    Console.WriteLine("=======================================\n");
                    Console.ResetColor();

                    Console.Write(LocalizationManager.Get("EnterTicker"));
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
                        var removeOptions = new List<string>();
                        foreach (var stock in stocks)
                        {
                            removeOptions.Add($"{stock.Ticker,-6} | {stock.Sector} | {stock.Shares} shares | ${stock.TotalValue:F2}");
                        }

                        removeOptions.Add($"[ {LocalizationManager.Get("Cancel")} ]");

                        int removeChoice = ConsoleHelper.SelectOption(LocalizationManager.Get("SelectToRemove"), removeOptions);

                        if (removeChoice < stocks.Count)
                        {
                            string tickerToRemove = stocks[removeChoice].Ticker;
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

                // --- НОВИЙ БЛОК ОЧИЩЕННЯ ПОРТФЕЛЯ ---
                else if (selectedText == LocalizationManager.Get("ClearPortfolio"))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("=======================================");
                    Console.WriteLine($"  {LocalizationManager.Get("ClearPortfolio").ToUpper()}");
                    Console.WriteLine("=======================================\n");
                    Console.ResetColor();

                    Console.Write(LocalizationManager.Get("ClearConfirm"));
                    string confirmation = Console.ReadLine()?.Trim().ToUpper() ?? "";

                    if (confirmation == "YES")
                    {
                        portfolioManager.ClearAll();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n{LocalizationManager.Get("PortfolioCleared")}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("\n[Action canceled]");
                    }

                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey(true);
                }

                else if (selectedText == LocalizationManager.Get("Back"))
                {
                    break;
                }
            }
        }
    }
}