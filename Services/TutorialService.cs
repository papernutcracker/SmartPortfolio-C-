using System;
using System.Collections.Generic;
using SmartDividendTracker.Models;

namespace SmartDividendTracker.Services
{
    public static class TutorialService
    {
        public static void RunTutorial(UserProfile profile)
        {
            bool isUa = profile.Language.ToString() == "UA" || profile.Language.ToString() == "Ukrainian";
            Console.Clear();
            Console.WriteLine(isUa ? "Туторіал завершено!" : "Tutorial completed!");
            Console.ReadKey(true);
        }

        public static void ShowMenu(UserProfile profile)
        {
            bool isUa = profile.Language.ToString() == "UA" || profile.Language.ToString() == "Ukrainian";
            int lastChoice = 0;

            while (true)
            {
                Console.Clear();
                string header = "=========================================================\n" +
                                (isUa ? "                  НАВЧАЛЬНИЙ ЦЕНТР                       \n" : "                  EDUCATIONAL HUB                        \n") +
                                "=========================================================\n\n";

                var options = isUa ? new List<string>
                {
                    "1. Фінансовий план та психологія",
                    "2. Інфраструктура та брокери",
                    "3. Фінансові інструменти",
                    "4. Аналіз ринку",
                    "5. Стратегія портфеля",
                    "6. ПОВНИЙ КАТАЛОГ АКТИВІВ",
                    "Повернутися назад"
                } : new List<string>
                {
                    "1. Financial Plan & Psychology",
                    "2. Infrastructure & Brokers",
                    "3. Financial Instruments",
                    "4. Market Analysis",
                    "5. Portfolio Strategy",
                    "6. FULL ASSET CATALOG",
                    "Back to Main Menu"
                };

                int choice = ConsoleHelper.SelectOption(header, options, lastChoice);
                lastChoice = choice;

                if (choice == 6) break;

                ShowTopicContent(choice, isUa);
            }
        }

        private static void ShowTopicContent(int topicIndex, bool isUa)
        {
            Console.Clear();
            if (topicIndex == 5)
            {
                PrintFullAssetTable(isUa);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("=========================================================");
                Console.WriteLine(isUa ? "   ДЕТАЛЬНА ІНФОРМАЦІЯ" : "   DETAILED INFORMATION");
                Console.WriteLine("=========================================================\n");
                Console.ResetColor();

                switch (topicIndex)
                {
                    case 0: Console.WriteLine(isUa ? "Психологічна підготовка: Опрацювання вторинних вигод." : "Psychological prep..."); break;
                    case 1: Console.WriteLine(isUa ? "Брокери: Вибір надійного партнера (IBKR, XTB)." : "Brokers..."); break;
                    case 2: Console.WriteLine(isUa ? "Інструменти: Акції, ETF, Облігації." : "Instruments..."); break;
                    case 3: Console.WriteLine(isUa ? "Аналіз: 11 секторів, фундамент (EBITDA)." : "Analysis..."); break;
                    case 4: Console.WriteLine(isUa ? "Стратегія: Консервативна, Агресивна." : "Strategy..."); break;
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(isUa ? "\nНатисни будь-яку клавішу..." : "\nPress any key...");
                Console.ReadKey(true);
            }
        }

        private static ConsoleColor GetSectorColor(string sector)
        {
            return sector switch
            {
                "Tech" => ConsoleColor.Cyan,
                "Finance" => ConsoleColor.Green,
                "Health" => ConsoleColor.Magenta,
                "Consumer" => ConsoleColor.Yellow,
                "Industr." => ConsoleColor.Blue,
                "Staples" => ConsoleColor.DarkYellow,
                "Energy" => ConsoleColor.DarkRed,
                "Materials" => ConsoleColor.Gray,
                "Utilities" => ConsoleColor.White,
                "REIT" => ConsoleColor.DarkMagenta,
                "ETF/Index" => ConsoleColor.DarkCyan,
                "Metals" => ConsoleColor.Gray,
                "Miners" => ConsoleColor.DarkGray,
                _ => ConsoleColor.White
            };
        }

        private static void PrintFullAssetTable(bool isUa)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(isUa ?
                $"| {"Тікер",-8} | {"Сектор",-12} | {"Тип",-12} |" :
                $"| {"Ticker",-8} | {"Sector",-12} | {"Type",-12} |");
            Console.ResetColor();
            Console.WriteLine(new string('-', 40));

            var assets = new List<string[]> {
                new string[] { "AAPL", "Tech", "BlueChip" }, new string[] { "MSFT", "Tech", "BlueChip" }, new string[] { "NVDA", "Tech", "BlueChip" }, new string[] { "AVGO", "Tech", "BlueChip" }, new string[] { "ORCL", "Tech", "BlueChip" },
                new string[] { "JPM", "Finance", "BlueChip" }, new string[] { "V", "Finance", "BlueChip" }, new string[] { "MA", "Finance", "BlueChip" }, new string[] { "BAC", "Finance", "BlueChip" }, new string[] { "BRK", "Finance", "BlueChip" },
                new string[] { "JNJ", "Health", "BlueChip" }, new string[] { "UNH", "Health", "BlueChip" }, new string[] { "PFE", "Health", "BlueChip" }, new string[] { "LLY", "Health", "BlueChip" }, new string[] { "MRK", "Health", "BlueChip" }, new string[] { "ABBV", "Health", "BlueChip" },
                new string[] { "AMZN", "Consumer", "Growth" }, new string[] { "TSLA", "Consumer", "Growth" }, new string[] { "HD", "Consumer", "Growth" }, new string[] { "NKE", "Consumer", "Growth" }, new string[] { "MCD", "Consumer", "Growth" },
                new string[] { "UPS", "Industr.", "BlueChip" }, new string[] { "HON", "Industr.", "BlueChip" }, new string[] { "UNP", "Industr.", "BlueChip" }, new string[] { "RTX", "Industr.", "BlueChip" }, new string[] { "BA", "Industr.", "BlueChip" },
                new string[] { "GOOG", "Comm.", "Growth" }, new string[] { "META", "Comm.", "Growth" }, new string[] { "DIS", "Comm.", "Growth" }, new string[] { "NFLX", "Comm.", "Growth" }, new string[] { "CMCSA", "Comm.", "Growth" },
                new string[] { "WMT", "Staples", "Defensive" }, new string[] { "PG", "Staples", "Defensive" }, new string[] { "KO", "Staples", "Defensive" }, new string[] { "PEP", "Staples", "Defensive" }, new string[] { "COST", "Staples", "Defensive" },
                new string[] { "XOM", "Energy", "Cyclical" }, new string[] { "CVX", "Energy", "Cyclical" }, new string[] { "COP", "Energy", "Cyclical" }, new string[] { "EOG", "Energy", "Cyclical" }, new string[] { "SLB", "Energy", "Cyclical" },
                new string[] { "LIN", "Materials", "Industrial" }, new string[] { "SHW", "Materials", "Industrial" }, new string[] { "APD", "Materials", "Industrial" }, new string[] { "ECL", "Materials", "Industrial" }, new string[] { "FCX", "Materials", "Mining" },
                new string[] { "NEE", "Utilities", "Defensive" }, new string[] { "DUK", "Utilities", "Defensive" }, new string[] { "SO", "Utilities", "Defensive" }, new string[] { "D", "Utilities", "Defensive" }, new string[] { "EXC", "Utilities", "Defensive" },
                new string[] { "AMT", "REIT", "REIT" }, new string[] { "PLD", "REIT", "REIT" }, new string[] { "CCI", "REIT", "REIT" }, new string[] { "EQIX", "REIT", "REIT" }, new string[] { "PSA", "REIT", "REIT" },
                new string[] { "VOO", "ETF/Index", "ETF" }, new string[] { "GLD", "Metals", "ETF" }, new string[] { "IAU", "Metals", "ETF" }, new string[] { "SLV", "Metals", "ETF" }, new string[] { "GDX", "Miners", "ETF" }, new string[] { "SIL", "Miners", "ETF" },
                new string[] { "GOLD", "Miners", "Mining" }, new string[] { "NEM", "Miners", "Mining" }, new string[] { "AU", "Miners", "Mining" }, new string[] { "FRES", "Miners", "Mining" }, new string[] { "GLEN", "Miners", "Mining" }, new string[] { "PAAS", "Miners", "Mining" }, new string[] { "AGL", "Miners", "Mining" }, new string[] { "IMP", "Miners", "Mining" }
            };

            foreach (var asset in assets)
            {
                Console.ForegroundColor = GetSectorColor(asset[1]);
                Console.WriteLine($"| {asset[0],-8} | {asset[1],-12} | {asset[2],-12} |");
            }

            Console.ResetColor();
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(isUa ? "\nНатисни будь-яку клавішу для повернення..." : "\nPress any key to return...");
            Console.ReadKey(true);
        }
    }
}