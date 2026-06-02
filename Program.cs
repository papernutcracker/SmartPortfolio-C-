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
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            var onboarding = new OnboardingService();
            UserProfile currentUser = onboarding.RunOrLoadProfile();

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
                bool isUa = profile.Language.ToString() == "UA" || profile.Language.ToString() == "Ukrainian";

                var localizedGoalsList = new List<string>();
                foreach (var goal in profile.Goals)
                {
                    localizedGoalsList.Add(goal switch
                    {
                        InvestmentGoal.PassiveIncome => isUa ? "Пасивний дохід" : "Passive Income",
                        InvestmentGoal.CapitalGrowth => isUa ? "Зростання капіталу" : "Capital Growth",
                        InvestmentGoal.MajorPurchase => isUa ? "Велика покупка" : "Major Purchase",
                        _ => goal.ToString()
                    });
                }
                string goalsText = string.Join(", ", localizedGoalsList);

                string localizedHorizon = profile.Horizon switch
                {
                    InvestmentHorizon.UpTo5Years => isUa ? "До 5 років" : "Up to 5 years",
                    InvestmentHorizon.UpTo10Years => isUa ? "До 10 років" : "Up to 10 years",
                    InvestmentHorizon.LongTerm => isUa ? "Більше 10 років" : "10+ years",
                    _ => profile.Horizon.ToString()
                };

                string expText = isUa ? (profile.Experience == ExperienceLevel.Beginner ? "Новачок" : "Досвідчений") : profile.Experience.ToString();

                string header = (isUa ? "--- ГОЛОВНЕ МЕНЮ ---\n" : "--- MAIN MENU ---\n") +
                                $"[{(isUa ? "Цілі" : "Goals")}: {goalsText} | {(isUa ? "Горизонт" : "Horizon")}: {localizedHorizon} | {(isUa ? "Досвід" : "Level")}: {expText}]";

                var menuOptions = isUa ? new List<string>
                {
                    "Перегляд портфеля",
                    "Шпаргалка для новачка",
                    "Навчальний центр",
                    "Складний відсоток",
                    "Налаштування профілю",
                    "Вихід"
                } : new List<string>
                {
                    "View Portfolio",
                    "Beginner's Cheat Sheet",
                    "Educational Hub",
                    "Compound Calculator",
                    "Profile Settings",
                    "Exit"
                };

                int choice = ConsoleHelper.SelectOption(header, menuOptions, lastMainChoice);
                lastMainChoice = choice;

                // Використовуємо індекси замість текстових порівнянь
                if (choice == 0)
                {
                    ShowPortfolioMenu(portfolioManager, isUa);
                }
                else if (choice == 1)
                {
                    CheatSheetService.Show(isUa);
                }
                else if (choice == 2)
                {
                    TutorialService.ShowMenu(profile);
                }
                else if (choice == 3)
                {
                    CompoundCalculatorService.RunCalculator(isUa);
                }
                else if (choice == 4)
                {
                    onboarding.OpenSettings(profile);
                }
                else if (choice == 5)
                {
                    ConsoleHelper.ShowExitAnimation(isUa ? "Дякуємо за використання Smart Dividend Portfolio Tracker!" : "Thank you for using Smart Dividend Portfolio Tracker!");
                    break;
                }
            }
        }

        static void ShowPortfolioMenu(PortfolioManager portfolioManager, bool isUa)
        {
            int lastChoice = 0;

            while (true)
            {
                decimal totalValue = portfolioManager.GetTotalPortfolioValue();

                string header = (isUa ? "--- МЕНЮ ПОРТФЕЛЯ ---\n" : "--- PORTFOLIO MENU ---\n") +
                                $"[{(isUa ? "Загальна вартість" : "Total Value")}: ${totalValue:F2}]";

                var options = isUa ? new List<string>
                {
                    "Переглянути активи",
                    "Додати акцію",
                    "Видалити акцію",
                    "Очистити портфель",
                    "Повернутися назад"
                } : new List<string>
                {
                    "View Assets",
                    "Add Stock",
                    "Remove Stock",
                    "Clear Portfolio",
                    "Back"
                };

                int choice = ConsoleHelper.SelectOption(header, options, lastChoice);
                lastChoice = choice;

                if (choice == 0) // View Assets
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("=========================================================================================");
                    Console.WriteLine(isUa ? "  ПЕРЕГЛЯД АКТИВІВ" : "  VIEW ASSETS");
                    Console.WriteLine("=========================================================================================\n");
                    Console.ResetColor();

                    var stocks = portfolioManager.GetAllStocks();

                    if (stocks.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(isUa ? "Ваш портфель наразі порожній." : "Your portfolio is currently empty.");
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
                        Console.WriteLine(isUa ? $"\nЗагальна вартість портфеля: ${totalValue:F2}" : $"\nTotal Portfolio Value: ${totalValue:F2}");
                        Console.WriteLine(isUa ? $"Очікуваний річний дохід: ${totalIncome:F2} (Сер. дохідність: {avgYield:F2}%)" : $"Estimated Annual Income: ${totalIncome:F2} (Avg Yield: {avgYield:F2}%)");
                        Console.ResetColor();
                    }

                    Console.WriteLine(isUa ? "\nНатисніть будь-яку клавішу для повернення..." : "\nPress any key to return...");
                    Console.ReadKey(true);
                }
                else if (choice == 1) // Add Stock
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("=======================================");
                    Console.WriteLine(isUa ? "  ДОДАВАННЯ АКЦІЇ" : "  ADD NEW STOCK");
                    Console.WriteLine("=======================================\n");
                    Console.ResetColor();

                    Console.Write(isUa ? "Введіть тікер (наприклад, AAPL): " : "Enter Ticker symbol (e.g., AAPL): ");
                    string ticker = Console.ReadLine()?.Trim().ToUpper() ?? "";
                    if (string.IsNullOrEmpty(ticker)) ticker = "UNKNOWN";

                    var sectors = isUa ? new List<string> {
                        "Технології", "Фінанси", "Охорона здоров'я", "Товари першої необхідності",
                        "Споживчі товари", "Енергетика", "Комунальні послуги", "Нерухомість", "Промисловість", "Матеріали"
                    } : new List<string> {
                        "Technology", "Financials", "Healthcare", "Consumer Staples",
                        "Consumer Discretionary", "Energy", "Utilities", "Real Estate", "Industrials", "Materials"
                    };

                    int sectorChoice = ConsoleHelper.SelectOption(isUa ? "Оберіть сектор компанії:" : "Select the company's sector:", sectors);
                    string sector = sectors[sectorChoice];

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(isUa ? $"--- Додавання: {ticker} ({sector}) ---\n" : $"--- Adding: {ticker} ({sector}) ---\n");
                    Console.ResetColor();

                    decimal price = 0;
                    while (true)
                    {
                        Console.Write(isUa ? "Введіть середню ціну акції: " : "Enter Average Price per share: ");
                        string input = Console.ReadLine()?.Replace(".", ",") ?? "";
                        if (decimal.TryParse(input, out price) && price >= 0) break;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(isUa ? "Невірний ввід. Введіть додатнє число." : "Invalid input. Please enter a positive number.");
                        Console.ResetColor();
                    }

                    int shares = 0;
                    while (true)
                    {
                        Console.Write(isUa ? "Введіть кількість акцій: " : "Enter number of shares: ");
                        string input = Console.ReadLine() ?? "";
                        if (int.TryParse(input, out shares) && shares > 0) break;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(isUa ? "Невірний ввід. Введіть ціле додатнє число." : "Invalid input. Please enter a positive whole number.");
                        Console.ResetColor();
                    }

                    decimal divYield = 0;
                    while (true)
                    {
                        Console.Write(isUa ? "Введіть дивідендну дохідність % (наприклад, 4,5): " : "Enter Dividend Yield percentage (e.g., 4.5): ");
                        string input = Console.ReadLine()?.Replace(".", ",") ?? "";
                        if (decimal.TryParse(input, out divYield) && divYield >= 0) break;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(isUa ? "Невірний ввід." : "Invalid input.");
                        Console.ResetColor();
                    }

                    decimal peRatio = 0;
                    while (true)
                    {
                        Console.Write(isUa ? "Введіть P/E Ratio: " : "Enter P/E Ratio: ");
                        string input = Console.ReadLine()?.Replace(".", ",") ?? "";
                        if (decimal.TryParse(input, out peRatio) && peRatio >= 0) break;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(isUa ? "Невірний ввід." : "Invalid input.");
                        Console.ResetColor();
                    }

                    var newStock = new DividendStock(ticker, sector, price, shares, divYield, peRatio);
                    portfolioManager.AddStock(newStock);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(isUa ? $"\nУспішно додано {shares} акцій {ticker} до портфеля." : $"\nSuccessfully added {shares} shares of {ticker} to your portfolio.");
                    Console.ResetColor();

                    Console.WriteLine(isUa ? "\nНатисніть будь-яку клавішу для повернення..." : "\nPress any key to return...");
                    Console.ReadKey(true);
                }
                else if (choice == 2) // Remove Stock
                {
                    var stocks = portfolioManager.GetAllStocks();

                    if (stocks.Count == 0)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(isUa ? "\nВаш портфель порожній. Нічого видаляти." : "\nYour portfolio is empty. Nothing to remove.");
                        Console.ResetColor();
                        Console.WriteLine(isUa ? "\nНатисніть будь-яку клавішу для повернення..." : "\nPress any key to return...");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        var removeOptions = new List<string>();
                        foreach (var stock in stocks)
                        {
                            removeOptions.Add($"{stock.Ticker,-6} | {stock.Sector} | {stock.Shares} - ${stock.TotalValue:F2}");
                        }

                        removeOptions.Add(isUa ? "[ Скасувати ]" : "[ Cancel ]");

                        int removeChoice = ConsoleHelper.SelectOption(isUa ? "Оберіть акцію для видалення:" : "Select a stock to remove:", removeOptions);

                        if (removeChoice < stocks.Count)
                        {
                            string tickerToRemove = stocks[removeChoice].Ticker;
                            portfolioManager.RemoveStock(tickerToRemove);

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(isUa ? $"\nУспішно видалено {tickerToRemove} з портфеля." : $"\nSuccessfully removed {tickerToRemove} from your portfolio.");
                            Console.ResetColor();
                            Console.WriteLine(isUa ? "\nНатисніть будь-яку клавішу для повернення..." : "\nPress any key to return...");
                            Console.ReadKey(true);
                        }
                    }
                }
                else if (choice == 3) // Clear Portfolio
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("=======================================");
                    Console.WriteLine(isUa ? "  ОЧИСТИТИ ПОРТФЕЛЬ" : "  CLEAR PORTFOLIO");
                    Console.WriteLine("=======================================\n");
                    Console.ResetColor();

                    Console.Write(isUa ? "Ви впевнені, що хочете видалити всі активи? Введіть 'ТАК' для підтвердження: " : "Are you sure you want to delete all assets? Type 'YES' to confirm: ");
                    string confirmation = Console.ReadLine()?.Trim().ToUpper() ?? "";

                    if (confirmation == "YES" || confirmation == "ТАК")
                    {
                        portfolioManager.ClearAll();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(isUa ? "\nПортфель успішно очищено." : "\nPortfolio has been successfully cleared.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(isUa ? "\n[Дію скасовано]" : "\n[Action canceled]");
                    }

                    Console.WriteLine(isUa ? "\nНатисніть будь-яку клавішу для повернення..." : "\nPress any key to return...");
                    Console.ReadKey(true);
                }
                else if (choice == 4) // Back
                {
                    break;
                }
            }
        }
    }
}