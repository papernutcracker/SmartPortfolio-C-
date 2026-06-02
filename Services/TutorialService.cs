using System;
using System.Collections.Generic;
using SmartDividendTracker.Models;

namespace SmartDividendTracker.Services
{
    public static class TutorialService
    {
        // 1. Швидкий вступ для новачків (запускається 1 раз при старті)
        public static void RunTutorial(UserProfile profile)
        {
            bool isUa = profile.Language.ToString() == "UA" || profile.Language.ToString() == "Ukrainian";

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=========================================================");
            Console.WriteLine(isUa ? "        ВСТУПНИЙ ТУТОРІАЛ ДЛЯ НОВАЧКІВ           " : "        WELCOME TO THE DIVIDEND INVESTING TUTORIAL       ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            if (isUa)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Схоже, ти новачок у дивідендному інвестуванні. Давай пройдемося по базі!\n");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("1. ТІКЕР (TICKER)");
                Console.ResetColor();
                Console.WriteLine(" — Коротка абревіатура для ідентифікації акцій компанії на біржі. (Наприклад: AAPL для Apple).");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n2. ДИВІДЕНДНА ДОХІДНІСТЬ (DIVIDEND YIELD)");
                Console.ResetColor();
                Console.WriteLine(" — Показує, скільки компанія виплачує дивідендів на рік відносно ціни її акції.");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n3. P/E RATIO (Price-to-Earnings)");
                Console.ResetColor();
                Console.WriteLine(" — Співвідношення ціни акції до прибутку компанії. Допомагає зрозуміти, чи недооцінена акція.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Before you start building your portfolio, remember 3 basic terms:\n");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("1. TICKER");
                Console.ResetColor();
                Console.WriteLine(" — A short abbreviation used to identify shares (e.g., 'AAPL' for Apple).");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n2. DIVIDEND YIELD");
                Console.ResetColor();
                Console.WriteLine(" — Shows how much a company pays out in dividends relative to its stock price.");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n3. P/E RATIO (Price-to-Earnings)");
                Console.ResetColor();
                Console.WriteLine(" — Measures share price relative to earnings to see if a stock is undervalued.");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n=========================================================");
            Console.WriteLine(isUa ? "  Туторіал завершено! Готово до створення портфеля." : "  Tutorial completed! You are ready to build your portfolio.");
            Console.WriteLine("=========================================================\n");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(isUa ? "Натисни будь-яку клавішу для входу в Головне Меню..." : "Press any key to enter the Main Menu...");
            Console.ReadKey(true);
            Console.ResetColor();
        }

        // 2. Детальний Навчальний Центр (доступний з Головного Меню)
        public static void ShowMenu(UserProfile profile)
        {
            bool isUa = profile.Language.ToString() == "UA" || profile.Language.ToString() == "Ukrainian";
            int lastChoice = 0;

            while (true)
            {
                Console.Clear();
                string header = "=========================================================\n" +
                                (isUa ? "                  НАВЧАЛЬНИЙ ЦЕНТР                       \n" : "                  EDUCATIONAL HUB                        \n") +
                                "=========================================================\n\n" +
                                (isUa ? "Обери тему для читання:" : "Select a topic to study:");

                var options = isUa ? new List<string>
                {
                    "1. Особистий фінансовий план та психологія",
                    "2. Інфраструктура фондового ринку та брокери",
                    "3. Фінансові інструменти: Глибокий зріз",
                    "4. Аналіз ринку та компаній",
                    "5. Стратегія формування інвестиційного портфеля",
                    "Повернутися назад"
                } : new List<string>
                {
                    "1. Personal Financial Plan & Psychology",
                    "2. Stock Market Infrastructure & Brokers",
                    "3. Financial Instruments: Deep Dive",
                    "4. Market & Company Analysis",
                    "5. Portfolio Strategy & Risk Profiles",
                    "Back to Main Menu"
                };

                int choice = ConsoleHelper.SelectOption(header, options, lastChoice);
                lastChoice = choice;

                if (choice == 5) break; // Вихід

                ShowTopicContent(choice, isUa);
            }
        }

        private static void ShowTopicContent(int topicIndex, bool isUa)
        {
            Console.Clear();

            if (isUa)
            {
                switch (topicIndex)
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("=========================================================");
                        Console.WriteLine("   1. ОСОБИСТИЙ ФІНАНСОВИЙ ПЛАН ТА ПСИХОЛОГІЯ");
                        Console.WriteLine("=========================================================\n");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("- Психологічна підготовка:");
                        Console.ResetColor();
                        Console.WriteLine("  Перед створенням плану важливо опрацювати «вторинні вигоди» — підсвідомі причини, чому людині вигідно залишатися у поточному фінансовому стані (наприклад, уникати відповідальності).");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Техніки цілепокладання:");
                        Console.ResetColor();
                        Console.WriteLine("  Окрім розрахунків, цілі слід формулювати за допомогою техніки WHOOP, де детально прописується план подолання перешкод за формулою: «Якщо виникне (перешкода), то я зроблю (дію)».");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Інфляційні очікування:");
                        Console.ResetColor();
                        Console.WriteLine("  При розрахунку майбутньої вартості капіталу варто спиратися на історичні дані: середня інфляція в доларах за останні 10 років становить близько 2,14%.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Аналіз доходів:");
                        Console.ResetColor();
                        Console.WriteLine("  Фінансовий план обов'язково повинен містити порівняння поточного щомісячного доходу з тим доходом, який необхідний для реалізації поставлених цілей у визначені терміни.");
                        break;

                    case 1:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("=========================================================");
                        Console.WriteLine("   2. ІНФРАСТРУКТУРА ФОНДОВОГО РИНКУ ТА БРОКЕРИ");
                        Console.WriteLine("=========================================================\n");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("- Види інвестицій:");
                        Console.ResetColor();
                        Console.WriteLine("  Поділяються на реальні (нерухомість, бізнес), фінансові (цінні папери) та спекулятивні (короткострокове збагачення на різниці цін).");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Реєстрація у брокера:");
                        Console.ResetColor();
                        Console.WriteLine("  Процес відкриття рахунку включає не лише перевірку ліцензій, але й підписання податкової форми W8-BEN та обов'язкове здійснення першого пробного переказу коштів.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Червоні прапорці шахраїв:");
                        Console.ResetColor();
                        Console.WriteLine("  Слід категорично уникати компаній, які пропонують договори позики, наполягають на використанні «торгових роботів», обіцяють гарантований заробіток або пропонують зв'язок через Skype.");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n- Рекомендовані брокери:");
                        Console.ResetColor();
                        Console.WriteLine("  До надійних варіантів відносяться Interactive Brokers (США, страховка рахунку до $250 000), Degiro (Нідерланди) та XTB (Польща).");
                        break;

                    case 2:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("=========================================================");
                        Console.WriteLine("   3. ФІНАНСОВІ ІНСТРУМЕНТИ: ГЛИБОКИЙ ЗРІЗ");
                        Console.WriteLine("=========================================================\n");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("- Акції та дивіденди:");
                        Console.ResetColor();
                        Console.WriteLine("  Шукайте «Дивідендних аристократів» (збільшують виплати 25 років поспіль) або «Дивідендних чемпіонів» (понад 5 років). ADR дозволяють купувати акції іноземних компаній у доларах на біржах США.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Облігації та рейтинги:");
                        Console.ResetColor();
                        Console.WriteLine("  Оцінюються агентствами (Moody's, S&P). Рейтинг від AAA до D. Діє непорушний закон: чим вищий відсоток прибутковості пропонує облігація, тим вищий ризик непогашення.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Деталі IPO:");
                        Console.ResetColor();
                        Console.WriteLine("  Процес супроводжується процедурою Due Diligence та Road Show. Важливим показником є «алокація» — відсоток від заявленої інвестором суми, на який реально дозволили купити акції.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Дорогоцінні метали:");
                        Console.ResetColor();
                        Console.WriteLine("  Можна інвестувати у фізичні злитки, акції видобувних компаній (Barrick Gold) або через ETF (GLD, SLV), що дозволяє купити метал безпосередньо на біржі.");
                        break;

                    case 3:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("=========================================================");
                        Console.WriteLine("   4. АНАЛІЗ РИНКУ ТА КОМПАНІЙ");
                        Console.WriteLine("=========================================================\n");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("- 11 секторів економіки:");
                        Console.ResetColor();
                        Console.WriteLine("  Сектор товарів першої необхідності та комунальні послуги є найбільш захисними під час рецесії.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Фундаментальний аналіз:");
                        Console.ResetColor();
                        Console.WriteLine("  Оцінка складається з 5 кроків: розуміння суті бізнесу, аналіз клієнтів/регіонів, оцінка попиту, вивчення конкурентів та аналіз планів.");

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n- Пастки у звітностях:");
                        Console.ResetColor();
                        Console.WriteLine("  Обережно ставтеся до показника EBITDA (не враховує податки/відсотки). Важливо, щоб готівка перевищувала дебіторську заборгованість, а капітал був більшим за борг.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Технічний аналіз свічок:");
                        Console.ResetColor();
                        Console.WriteLine("  Японська свічка демонструє переможця: зелена — покупці («бики»), червона — продавці («ведмеді»). Рух ціни буває трьох станів: висхідний, спадний або боковик.");
                        break;

                    case 4:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("=========================================================");
                        Console.WriteLine("   5. СТРАТЕГІЯ ФОРМУВАННЯ ПОРТФЕЛЯ");
                        Console.WriteLine("=========================================================\n");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("- Консервативний профіль:");
                        Console.ResetColor();
                        Console.WriteLine("  Прибутковість ~7%. Рекомендується тримати 85-90% у розвинених країнах (США, Німеччина) і 10-15% у країнах, що розвиваються (Китай, Індія).");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Помірний профіль:");
                        Console.ResetColor();
                        Console.WriteLine("  Прибутковість ~9%. Для довгострокових інвесторів, які хочуть зростання, але не потребують миттєвого доходу.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Агресивний профіль:");
                        Console.ResetColor();
                        Console.WriteLine("  Прибутковість понад 9.6%. Співвідношення може складати 60% на 40% (розвинені / ті, що розвиваються). Підходить готовим до значної волатильності.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Управління портфелем:");
                        Console.ResetColor();
                        Console.WriteLine("  Регулярна робота включає огляд активів, ребалансування (відновлення початкового співвідношення активів), реінвестування купонів та дивідендів.");
                        break;
                }
            }
            else
            {
                // Для англійської мови структура кольорів зберігається ідентичною
                switch (topicIndex)
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("=========================================================");
                        Console.WriteLine("   1. PERSONAL FINANCIAL PLAN & PSYCHOLOGY");
                        Console.WriteLine("=========================================================\n");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("- Psychological Preparation:");
                        Console.ResetColor();
                        Console.WriteLine("  Work through 'secondary gains' — subconscious reasons why it is convenient to stay in your current financial state (e.g., avoiding responsibility).");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Goal Setting:");
                        Console.ResetColor();
                        Console.WriteLine("  Use the WHOOP technique. Outline your plan to overcome obstacles: 'If [obstacle] occurs, then I will do [action].'");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Inflation Expectations:");
                        Console.ResetColor();
                        Console.WriteLine("  Base future capital calculations on historical data. The average USD inflation over the last 10 years is about 2.14%.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Income Analysis:");
                        Console.ResetColor();
                        Console.WriteLine("  Compare your current monthly income with the income required to achieve your goals within the set timeframe.");
                        break;

                    case 1:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("=========================================================");
                        Console.WriteLine("   2. STOCK MARKET INFRASTRUCTURE & BROKERS");
                        Console.WriteLine("=========================================================\n");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("- Investment Types:");
                        Console.ResetColor();
                        Console.WriteLine("  Divided into Real (real estate, business), Financial (securities), and Speculative (short-term profit on price differences).");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Broker Registration:");
                        Console.ResetColor();
                        Console.WriteLine("  Opening an account involves checking licenses, signing the W8-BEN tax form, and making a mandatory trial transfer.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Scam Red Flags:");
                        Console.ResetColor();
                        Console.WriteLine("  Avoid companies offering loan agreements, insisting on 'trading robots', promising guaranteed earnings, or requesting communication via Skype.");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n- Recommended Brokers:");
                        Console.ResetColor();
                        Console.WriteLine("  Reliable options include Interactive Brokers (US, insured up to $250k), Degiro (Netherlands), and XTB (Poland).");
                        break;

                    case 2:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("=========================================================");
                        Console.WriteLine("   3. FINANCIAL INSTRUMENTS: DEEP DIVE");
                        Console.WriteLine("=========================================================\n");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("- Stocks & Dividends:");
                        Console.ResetColor();
                        Console.WriteLine("  Look for 'Dividend Aristocrats' (25+ years of growth) or 'Dividend Champions' (5+ years). ADRs allow buying foreign company shares in USD on US exchanges.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Bonds & Ratings:");
                        Console.ResetColor();
                        Console.WriteLine("  Issuer creditworthiness is evaluated by agencies (Moody's, S&P) from AAA to D. Rule: Higher yield strictly correlates with higher risk of default.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- IPO Details:");
                        Console.ResetColor();
                        Console.WriteLine("  Accompanied by Due Diligence and Road Shows. Pay attention to 'allocation' — the percentage of your requested amount the broker actually allows you to buy.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Precious Metals:");
                        Console.ResetColor();
                        Console.WriteLine("  You can invest in physical bars, mining company stocks (e.g., Barrick Gold), or Gold/Silver ETFs (GLD, SLV) directly on the exchange.");
                        break;

                    case 3:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("=========================================================");
                        Console.WriteLine("   4. MARKET & COMPANY ANALYSIS");
                        Console.WriteLine("=========================================================\n");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("- 11 Economic Sectors:");
                        Console.ResetColor();
                        Console.WriteLine("  Consumer Staples and Utilities are the most defensive sectors during an economic recession.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Fundamental Analysis:");
                        Console.ResetColor();
                        Console.WriteLine("  Evaluate in 5 steps: understand the business, analyze clients/regions, evaluate demand, study competitors, and analyze future plans.");

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n- Reporting Traps:");
                        Console.ResetColor();
                        Console.WriteLine("  Be cautious with EBITDA (ignores taxes/interest). Cash should exceed receivables, and total equity should exceed total debt.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Technical Analysis:");
                        Console.ResetColor();
                        Console.WriteLine("  Japanese candlesticks show who won the timeframe: green (buyers/'bulls'), red (sellers/'bears'). Trends can be upward, downward, or sideways.");
                        break;

                    case 4:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("=========================================================");
                        Console.WriteLine("   5. PORTFOLIO STRATEGY & RISK PROFILES");
                        Console.WriteLine("=========================================================\n");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("- Conservative Profile:");
                        Console.ResetColor();
                        Console.WriteLine("  ~7% return. Hold 85-90% in Developed countries (US, Germany) and 10-15% in Emerging markets (China, India).");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Moderate Profile:");
                        Console.ResetColor();
                        Console.WriteLine("  ~9% return. For long-term investors who want growth but don't need immediate income.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Aggressive Profile:");
                        Console.ResetColor();
                        Console.WriteLine("  9.6%+ return. Allocation can be 60% Developed / 40% Emerging. Suited for those ready for significant volatility.");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n- Portfolio Management:");
                        Console.ResetColor();
                        Console.WriteLine("  Regular work includes asset review, rebalancing (restoring initial percentages), and reinvesting coupons and dividends.");
                        break;
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(isUa ? "\nНатисни будь-яку клавішу для повернення..." : "\nPress any key to return...");
            Console.ReadKey(true);
            Console.ResetColor();
        }
    }
}