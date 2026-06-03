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
                string header = $"  {LocalizationManager.Get("EduMenuTitle").ToUpper()}";

                var options = profile.Language == "UA" ? new List<string>
                {
                    "1. Фінансовий план та психологія",
                    "2. Інфраструктура та брокери",
                    "3. Фінансові інструменти",
                    "4. Аналіз ринку",
                    "5. Стратегія портфеля",
                    "6. Перелік популярних тікерів",
                    LocalizationManager.Get("Back")
                } : new List<string>
                {
                    "1. Financial Plan & Psychology",
                    "2. Infrastructure & Brokers",
                    "3. Financial Instruments",
                    "4. Market Analysis",
                    "5. Portfolio Strategy",
                    "6. List of popular tickers",
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
                return;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=========================================================================================");
            Console.WriteLine(isUa ? "   ДЕТАЛЬНИЙ НАВЧАЛЬНИЙ ОГЛЯД МАТЕРІАЛУ" : "   DETAILED EDUCATIONAL OVERVIEW");
            Console.WriteLine("=========================================================================================\n");
            Console.ResetColor();

            if (isUa)
            {
                switch (topicIndex)
                {
                    case 0: // Особистий фінансовий план
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("## 1. ОСОБИСТИЙ ФІНАНСОВИЙ ПЛАН\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=== Постановка фінансових цілей ===");
                        Console.ResetColor();
                        Console.WriteLine("Етап постановки фінансових цілей є критично важливим для інвестора. Без чітких цілей\n" +
                                          "виникає ризик витрачання заощаджень на неважливі речі, а також ризик руху в хибному\n" +
                                          "напрямку або повної зупинки розвитку. Техніки роботи з цілями:\n");
                        Console.WriteLine("• Техніка «Колесо»: Допомагає поділити весь шлях до глобальної мети на окремі сегменти.");
                        Console.WriteLine("• Техніка WOOP: Визначення мети -> опис результату -> виявлення перешкод -> план дій.");
                        Console.WriteLine("• Система SMART: Перевірка цілі на конкретність, вимірність, досяжність, релевантність та час.\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=== Врахування інфляції ===");
                        Console.ResetColor();
                        Console.WriteLine("Інфляція — головний ворог заощаджень. Історичні середні значення:\n" +
                                          "Гривня: ~11% | Долар США: ~2.14% | Євро: ~1.77%\n\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=== Складові таблиці фінансового плану ===");
                        Console.ResetColor();
                        Console.WriteLine("Для плану розраховуються: необхідний дохід, поточний дохід, термін досягнення,\n" +
                                          "вартість цілі зараз, вартість у майбутньому (з інфляцією) та щомісячний внесок.\n" +
                                          "Більш детальні розрахунки можна зробити у калькуляторах");
                        break;

                    case 1: // Інфраструктура фондового ринку та вибір брокера
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("## 2. ІНФРАСТРУКТУРА ФОНДОВОГО РИНКУ ТА ВИБІР БРОКЕРА\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=== Поняття фондового ринку та біржі ===");
                        Console.ResetColor();
                        Console.WriteLine("Фондовий ринок — частина фінансової системи, де купують/продають електронні цінні папери.\n" +
                                          "Фондова біржа — ліцензований майданчик, який забезпечує ліквідність та прозорість.\n" +
                                          "Торгові сесії (за Києвом):\n" +
                                          "• Азійсько-Тихоокеанська: 03:00 - 11:00\n" +
                                          "• Європейська: 09:00 - 18:30\n" +
                                          "• Американська (NYSE, NASDAQ): 16:30 - 23:00\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=== Критерії аналізу та вибору брокера ===");
                        Console.ResetColor();
                        Console.WriteLine("Інвестувати можна виключно через сертифікованого брокера. Критерії оцінки:\n" +
                                          "1. Наявність ліцензії авторитетного регулятора (не офшор).\n" +
                                          "2. Тарифи та комісії за угоди й виведення коштів.\n" +
                                          "3. Сума страхового покриття рахунку (захист на випадок банкрутства).\n" +
                                          "4. Доступ до світових ринків та історія/масштаби компанії.\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=== Рекомендовані брокери та безпека ===");
                        Console.ResetColor();
                        Console.WriteLine("• Interactive Brokers (США): Без мін. депозиту, страховка до $500k, комісії від $0.35\n" +
                                          "• Degiro (Нідерланди): Мін. депозит $10, страховка до €100k, комісії $1-2\n" +
                                          "• XTB (Польща): Без мін. депозиту, комісія за фізичні акції 0%\n\n" +
                                          "🛑 Червоні прапорці шахрайства: відсутність ліцензії, пропозиції позик, заклики в IPO\n" +
                                          "через месенджери, робочі роботи. У США ліцензії перевіряють через FINRA (BrokerCheck).");
                        break;

                    case 2: // Фінансові інструменти
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("## 3. ФІНАНСОВІ ІНСТРУМЕНТИ\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=== 1. Акції (Пайові цінні папери) ===");
                        Console.ResetColor();
                        Console.WriteLine("• Звичайні: дають право голосу та дивіденди.\n" +
                                          "• Привілейовані: мають фіксований дохід, але обмежене право голосу.\n" +
                                          "• Стратегії: акції зростання (без дивідендів: Tesla, Amazon) та акції вартості (Berkshire Hathaway).\n" +
                                          "• Найнадійніші компанії — «блакитні фішки». Дивідендні аристократи збільшують виплати 25+ років.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n=== 2. IPO (Initial Public Offering) ===");
                        Console.ResetColor();
                        Console.WriteLine("Перший публічний продаж акцій компанії. Важливий Lock-up період (90–180 днів),\n" +
                                          "протягом якого ранні інвестори та засновники не мають права продавати папери.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n=== 3. Облігації (Боргові цінні папери) ===");
                        Console.ResetColor();
                        Console.WriteLine("Позика емітенту під фіксований купон. Рівні: Інвестиційний (AAA до BBB) та Сміттєвий (BB і нижче).\n" +
                                          "Головний закон: чим вища купонна прибутковість, тим вищий ризик дефолту.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n=== 4. Індекси, ETF, REIT та Метали ===");
                        Console.ResetColor();
                        Console.WriteLine("• ETF (Exchange Traded Fund): готовий диверсифікований кошик акцій за певним індексом (напр. S&P 500).\n" +
                                          "• REIT (Фонди нерухомості): зобов'язані виплачувати не менше 90% чистого доходу як дивіденди.\n" +
                                          "• Дорогоцінні метали: інвестиції через зливки, монети, або товарні ETF (GLD, IAU, SLV).");
                        break;

                    case 3: // Аналіз ринку та компаній
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("## 4. АНАЛІЗ РИНКУ ТА КОМПАНІЙ\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=== Технічний аналіз ===");
                        Console.ResetColor();
                        Console.WriteLine("Базується на свічкових графіках (японські свічки: Open, Close, High, Low).\n" +
                                          "Математичний індикатор RSI (Індекс відносної сили):\n" +
                                          "• RSI > 70: Стан перекупленості (ціна завищена, купувати не рекомендується).\n" +
                                          "• RSI 30-70: Справедлива поточна ринкова ціна.\n" +
                                          "• RSI < 30: Стан перепроданості (актив активно скидають, чудовий момент для покупки).\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=== Фундаментальний аналіз компанії ===");
                        Console.ResetColor();
                        Console.WriteLine("Глибоке вивчення бізнесу за 5 етапами: аналіз діяльності -> клієнти та географія\n" +
                                          "-> фактори попиту -> конкурентне середовище -> плани масштабування.\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=== Оцінка фінансової звітності та макроекономіка ===");
                        Console.ResetColor();
                        Console.WriteLine("• Income Statement: Виручка, чистий прибуток, EPS, EBITDA.\n" +
                                          "• Balance Sheet: Активи (Assets) проти зобов'язань (Liabilities). Борг має бути меншим за капітал.\n" +
                                          "• Cash Flow Statement: Реальний рух грошей від операційної, інвестиційної та фін. діяльності.\n" +
                                          "• Макропоказники: ВВП (зменшення = криза), Безробіття, Інфляція та Кредитна ставка ЦБ.");
                        break;

                    case 4: // Створення інвестиційного портфеля
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("## 5. СТВОРЕННЯ ІНВЕСТИЦІЙНОГО ПОРТФЕЛЯ\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=== Визначення ризик-профілю інвестора ===");
                        Console.ResetColor();
                        Console.WriteLine("Залежить від часового горизонту та стійкості до волатильності ринку:\n" +
                                          "• Консервативна стратегія: збереження капіталу (середня прибутковість ~7% річних).\n" +
                                          "• Помірна стратегія: збалансований довгостроковий приріст (~9% річних).\n" +
                                          "• Агресивна стратегія: максимізація прибутків через волатильні активи та крипту.\n" +
                                          "Активи повинні мати негативну або низьку кореляцію (синхронність руху цін) для захисту.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n=== Формування портфеля за принципом TOP DOWN (Зверху вниз) ===");
                        Console.ResetColor();
                        Console.WriteLine("1. Розподіл за регіонами: Розвинені ринки (Developed) та ринки, що ростуть (Emerging).\n" +
                                          "2. Аналіз конкретної країни (макропоказники, політика).\n" +
                                          "3. Аналіз секторів відповідно до фази бізнес-циклу:\n" +
                                          "   - Фаза зростання: Циклічні сектори (Фінанси, Технології, Промисловість, Нерухомість).\n" +
                                          "   - Фаза рецесії: Оборонні сектори (Товари першої необхідності, Охорона здоров'я, Комунальні послуги).\n" +
                                          "4. Фундаментальний відбір компаній у межах обраного сектору (мультиплікатори).\n" +
                                          "5. Технічний аналіз (пошук точок входу на ринок через індикатор RSI).\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=== Обслуговування стратегії ===");
                        Console.ResetColor();
                        Console.WriteLine("Включає плановий огляд портфеля, щорічне ребалансування (відновлення часток часток),\n" +
                                          "реінвестування дивідендів та коригування плану під життєві обставини.");
                        break;
                }
            }
            else
            {
                // Англійська локалізація (короткі тези для збереження білінгвальності структури)
                switch (topicIndex)
                {
                    case 0:
                        Console.WriteLine("Financial Plan: Goals settings (SMART, WOOP), Inflation formulas (FV = PV*(1+i)^n) and allocation tables.");
                        break;
                    case 1:
                        Console.WriteLine("Infrastructure & Brokers: Market sessions, broker selection criteria, regulated agents (IBKR, Degiro, XTB), and FINRA validation.");
                        break;
                    case 2:
                        Console.WriteLine("Financial Instruments: Stocks (Growth vs Value, Blue Chips), IPOs (Lock-up rules), Bonds (Credit ratings), ETFs, REITs, and Commodities.");
                        break;
                    case 3:
                        Console.WriteLine("Market Analysis: Technical Analysis (Candlesticks, RSI thresholds), Fundamental Analysis steps, Financial Statements analysis, and Macro indicators.");
                        break;
                    case 4:
                        Console.WriteLine("Portfolio Strategy: Risk-profiling, TOP-DOWN asset allocation framework, correlation matrices, and strategy rebalancing techniques.");
                        break;
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
            Console.ReadKey(true);
            Console.ResetColor();
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

            Console.WriteLine(isUa ?
                $"| {"Тікер",-8} | {"Сектор",-22} | {"Дохідн.",-7} | {"P/E",-6} |" :
                $"| {"Ticker",-8} | {"Sector",-12} | {"Yield",-7} | {"P/E",-6} |");
            Console.ResetColor();

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
                        "Miners" => "Видобуток",
                        _ => sector
                    };
                }

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