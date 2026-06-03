using System;
using System.Collections.Generic;
using SmartDividendTracker.Models;

namespace SmartDividendTracker.Services
{
    public static class TutorialService
    {
        public static void RunTutorial(UserProfile profile)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=========================================================");
            Console.WriteLine($"  {LocalizationManager.Get("EduMenuTitle")}");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            string[] steps = {
                LocalizationManager.Get("TutWelcome"),
                LocalizationManager.Get("TutStep1"),
                LocalizationManager.Get("TutStep2"),
                LocalizationManager.Get("TutReady")
            };

            foreach (var step in steps)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"» {step}\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(LocalizationManager.Get("PressEnter"));
                Console.ResetColor();
                Console.ReadKey(true);
                Console.WriteLine();
            }

            Console.Clear();
        }

        public static void ShowMenu(UserProfile profile)
        {
            int lastChoice = 0;

            while (true)
            {
                Console.Clear();
                string header = "=========================================================\n" +
                                $"                  {LocalizationManager.Get("EduMenuTitle")}                       \n" +
                                "=========================================================\n\n";

                var options = profile.Language == "UA" ? new List<string>
                {
                    "1. Фінансовий план та психологія",
                    "2. Інфраструктура та брокери",
                    "3. Фінансові інструменти",
                    "4. Аналіз ринку",
                    "5. Стратегія портфеля",
                    "6. ПОВНИЙ КАТАЛОГ АКТИВІВ",
                    LocalizationManager.Get("Back")
                } : new List<string>
                {
                    "1. Financial Plan & Psychology",
                    "2. Infrastructure & Brokers",
                    "3. Financial Instruments",
                    "4. Market Analysis",
                    "5. Portfolio Strategy",
                    "6. FULL ASSET CATALOG",
                    LocalizationManager.Get("Back")
                };

                int choice = ConsoleHelper.SelectOption(header, options, lastChoice);
                lastChoice = choice;

                if (choice == 6) break;

                ShowTopicContent(choice, profile.Language == "UA");
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
                    case 0: Console.WriteLine(isUa ? "Психологічна підготовка: Опрацювання вторинних вигод від незаробляння грошей, формування довгострокового мислення." : "Psychological prep: Shifting focus to long-term mindset and money habits."); break;
                    case 1: Console.WriteLine(isUa ? "Брокери: Вибір надійного партнера з регуляцією (Interactive Brokers, XTB). Податки та декларації." : "Brokers: Choosing reliable regulated platforms (IBKR, XTB). Tax compliance."); break;
                    case 2: Console.WriteLine(isUa ? "Інструменти: Акції (частка в бізнесі), ETF (готові фонди), Облігації (боргові розписки)." : "Instruments: Stocks (business shares), ETFs (diversified baskets), Bonds (fixed income)."); break;
                    case 3: Console.WriteLine(isUa ? "Аналіз: 11 секторів економіки. Фундаментальні мультиплікатори: P/E, P/S, Payout Ratio, дивидендна історія." : "Analysis: 11 market sectors. Key fundamental metrics: P/E, Dividend History, Payout Ratio."); break;
                    case 4: Console.WriteLine(isUa ? "Стратегія: Дивідендна (фокус на грошовий потік), Ростова (Growth), або збалансоване індексування (VOO/SPY)." : "Strategy: Dividend Income (cash flow focus) vs Capital Growth vs Index Investing."); break;
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
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
                "Comm." => ConsoleColor.DarkCyan,
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

            // Збільшено ширину колонки Сектор до -22 символів для UA
            Console.WriteLine(isUa ?
                $"| {"Тікер",-8} | {"Сектор",-22} | {"Дохідн.",-7} | {"P/E",-6} |" :
                $"| {"Ticker",-8} | {"Sector",-12} | {"Yield",-7} | {"P/E",-6} |");
            Console.ResetColor();

            // Загальна довжина рядка для укр версії тепер становить рівно 56 символів
            Console.WriteLine(new string('-', isUa ? 56 : 45));

            var assets = new List<string[]> {
                new string[] { "AAPL", "Tech", "0.5%", "32.1" }, new string[] { "MSFT", "Tech", "0.7%", "35.4" }, new string[] { "NVDA", "Tech", "0.02%", "68.2" }, new string[] { "AVGO", "Tech", "1.2%", "29.5" }, new string[] { "ORCL", "Tech", "1.4%", "31.0" },
                new string[] { "JPM", "Finance", "2.4%", "12.3" }, new string[] { "V", "Finance", "0.8%", "30.5" }, new string[] { "MA", "Finance", "0.6%", "35.1" }, new string[] { "BAC", "Finance", "2.6%", "11.8" }, new string[] { "BRK", "Finance", "0.0%", "21.4" },
                new string[] { "JNJ", "Health", "3.1%", "15.6" }, new string[] { "UNH", "Health", "1.5%", "22.8" }, new string[] { "PFE", "Health", "5.8%", "14.2" }, new string[] { "LLY", "Health", "0.6%", "81.5" }, new string[] { "MRK", "Health", "2.5%", "16.4" }, new string[] { "ABBV", "Health", "3.8%", "18.1" },
                new string[] { "AMZN", "Consumer", "0.0%", "42.6" }, new string[] { "TSLA", "Consumer", "0.0%", "54.1" }, new string[] { "HD", "Consumer", "2.3%", "23.4" }, new string[] { "NKE", "Consumer", "1.6%", "25.2" }, new string[] { "MCD", "Consumer", "2.2%", "24.7" },
                new string[] { "UPS", "Industr.", "4.1%", "19.3" }, new string[] { "HON", "Industr.", "2.1%", "22.0" }, new string[] { "UNP", "Industr.", "2.3%", "21.5" }, new string[] { "RTX", "Industr.", "2.4%", "18.7" }, new string[] { "BA", "Industr.", "0.0%", "N/A" },
                new string[] { "GOOG", "Comm.", "0.5%", "24.3" }, new string[] { "META", "Comm.", "0.4%", "26.8" }, new string[] { "DIS", "Comm.", "0.7%", "31.2" }, new string[] { "NFLX", "Comm.", "0.0%", "39.5" }, new string[] { "CMCSA", "Comm.", "2.8%", "11.4" },
                new string[] { "WMT", "Staples", "1.3%", "28.1" }, new string[] { "PG", "Staples", "2.4%", "26.3" }, new string[] { "KO", "Staples", "3.0%", "24.5" }, new string[] { "PEP", "Staples", "2.9%", "25.0" }, new string[] { "COST", "Staples", "0.6%", "46.2" },
                new string[] { "XOM", "Energy", "3.3%", "13.1" }, new string[] { "CVX", "Energy", "4.1%", "12.4" }, new string[] { "COP", "Energy", "2.9%", "11.2" }, new string[] { "EOG", "Energy", "2.5%", "10.6" }, new string[] { "SLB", "Energy", "2.1%", "15.9" },
                new string[] { "LIN", "Materials", "1.3%", "32.4" }, new string[] { "SHW", "Materials", "0.9%", "30.7" }, new string[] { "APD", "Materials", "2.6%", "25.1" }, new string[] { "ECL", "Materials", "1.1%", "35.8" }, new string[] { "FCX", "Materials", "1.4%", "27.9" },
                new string[] { "NEE", "Utilities", "3.2%", "22.3" }, new string[] { "DUK", "Utilities", "4.1%", "17.6" }, new string[] { "SO", "Utilities", "3.8%", "19.1" }, new string[] { "D", "Utilities", "4.8%", "16.5" }, new string[] { "EXC", "Utilities", "3.9%", "15.2" },
                new string[] { "AMT", "REIT", "3.3%", "22.4" }, new string[] { "PLD", "REIT", "2.8%", "24.6" }, new string[] { "CCI", "REIT", "4.8%", "20.1" }, new string[] { "EQIX", "REIT", "1.9%", "30.8" }, new string[] { "PSA", "REIT", "4.1%", "18.3" },
                new string[] { "VOO", "ETF/Index", "1.3%", "26.0" }, new string[] { "GLD", "Metals", "0.0%", "N/A" }, new string[] { "IAU", "Metals", "0.0%", "N/A" }, new string[] { "SLV", "Metals", "0.0%", "N/A" }, new string[] { "GDX", "Miners", "1.6%", "22.4" }, new string[] { "SIL", "Miners", "1.1%", "25.7" },
                new string[] { "GOLD", "Miners", "2.1%", "18.2" }, new string[] { "NEM", "Miners", "2.4%", "20.5" }, new string[] { "AU", "Miners", "1.4%", "17.9" }, new string[] { "FRES", "Miners", "1.8%", "19.3" }, new string[] { "GLEN", "Miners", "3.5%", "14.1" }, new string[] { "PAAS", "Miners", "1.2%", "23.6" }, new string[] { "AGL", "Miners", "2.0%", "16.8" }, new string[] { "IMP", "Miners", "4.0%", "12.2" }
            };

            foreach (var asset in assets)
            {
                string ticker = asset[0];
                string sector = asset[1];
                string yield = asset[2];
                string pe = asset[3];

                Console.ForegroundColor = GetSectorColor(sector);

                if (isUa)
                {
                    sector = sector switch
                    {
                        "Tech" => "Технології",
                        "Finance" => "Фінанси",
                        "Health" => "Охорона здоров'я",
                        "Consumer" => "Споживчі товари",
                        "Industr." => "Промисловість",
                        "Comm." => "Комунікації",
                        "Staples" => "Товари першої необх.",
                        "Energy" => "Енергетика",
                        "Materials" => "Матеріали",
                        "Utilities" => "Комун. послуги",
                        "REIT" => "Нерухомість (REIT)",
                        "ETF/Index" => "ETF / Індекс",
                        "Metals" => "Дорогоцінні метали",
                        "Miners" => "Шахти цінн. мет.",
                        _ => sector
                    };
                }

                // Використовуємо -22 для вирівнювання сектора в UA розкладці
                Console.WriteLine(isUa
                    ? $"| {ticker,-8} | {sector,-22} | {yield,-7} | {pe,-6} |"
                    : $"| {ticker,-8} | {sector,-12} | {yield,-7} | {pe,-6} |");
            }

            Console.ResetColor();
            Console.WriteLine(new string('-', isUa ? 56 : 45));
            Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
            Console.ReadKey(true);
        }
    }
}