using System;
using System.Globalization;

namespace SmartDividendTracker.Services
{
    public static class GoalCalculatorService
    {
        public static void Run(bool isUa)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=========================================================");
            Console.WriteLine(isUa ? "         ЯК ПРАЦЮЄ АЛГОРИТМ ПЛАНУВАННЯ ЦІЛІ             " : "             HOW THE GOAL PLANNING WORKS                 ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            if (isUa)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("📌 Крок 1. Дізнайся 'реальну' ціну через роки (з інфляцією)");
                Console.ResetColor();
                Console.WriteLine("Гроші знецінюються, тому твоя ціль через роки коштуватиме дорожче.\n" +
                                  "Як орієнтир для долара береться інфляція 2.14% річних.\n" +
                                  "Формула: Сума сьогодні * (1.0214 ^ кількість років).\n");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("📌 Крок 2. Рахуємо, скільки треба відкладати щомісяця");
                Console.ResetColor();
                Console.WriteLine("Ми беремо майбутню ціну (з Кроку 1) і ділимо її на коефіцієнт\n" +
                                  "складного відсотка твоєї очікуваної дохідності інвестицій.\n" +
                                  "Це покаже необхідну суму на рік, яку ми ділимо на 12.\n");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("💡 ПРИКЛАД ДЛЯ НАОЧНОСТІ:");
                Console.ResetColor();
                Console.WriteLine("Тобі потрібно купити нерухомість через 10 років.\n" +
                                  "• Ціль зараз: $50,000\n" +
                                  "• Ціль через 10 років (з інфляцією): $60,950\n" +
                                  "• Якщо інвестувати під 10% річних: треба ~$3,824 на рік.\n" +
                                  "• ОТЖЕ, ЩОМІСЯЦЯ ТРЕБА ІНВЕСТУВАТИ: ~$319\n");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Натисни будь-яку клавішу, щоб перейти до розрахунку власної цілі...");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("📌 Step 1. Find the 'real' cost through the years (with inflation)");
                Console.ResetColor();
                Console.WriteLine("Money loses value, so your goal will cost more in the future.\n" +
                                  "We use a standard 2.14% annual inflation rate for the USD.\n" +
                                  "Formula: Current Cost * (1.0214 ^ number of years).\n");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("📌 Step 2. Calculate your monthly investment target");
                Console.ResetColor();
                Console.WriteLine("We take the future adjusted cost (from Step 1) and divide it by\n" +
                                  "the compound interest factor based on your expected return.\n" +
                                  "This gives the annual target, which we then divide by 12.\n");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("💡 PRACTICAL EXAMPLE:");
                Console.ResetColor();
                Console.WriteLine("Imagine you want to buy property in 10 years.\n" +
                                  "• Goal cost today: $50,000\n" +
                                  "• Cost in 10 years (adjusted for inflation): $60,950\n" +
                                  "• If you invest at 10% ROI: you need ~$3,824 per year.\n" +
                                  "• REQUIRED MONTHLY INVESTMENT: ~$319\n");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Press any key to start calculating your own goal...");
            }

            Console.ReadKey(true);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=========================================================");
            Console.WriteLine(isUa ? "                КАЛЬКУЛЯТОР ФІНАНСОВОЇ ЦІЛІ              " : "                     GOAL CALCULATOR                     ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            decimal currentPrice = GetInput(isUa ? "1. Ціна твоєї цілі сьогодні ($): " : "1. Current cost of your goal ($): ");
            int years = (int)GetInput(isUa ? "2. Через скільки років плануєш купівлю: " : "2. Target horizon (years): ");
            decimal annualReturn = GetInput(isUa ? "3. Очікувана річна дохідність інвестицій (%): " : "3. Expected annual return (%): ");

            double inflationRate = 0.0214;
            double futurePrice = (double)currentPrice * Math.Pow(1 + inflationRate, years);

            double r = (double)annualReturn / 100;
            double annualContribution = 0;

            if (r > 0)
            {
                double temp = Math.Pow(1 + r, years) - 1;
                annualContribution = futurePrice / (temp / r);
            }
            else
            {
                annualContribution = futurePrice / years;
            }

            double monthlyContribution = annualContribution / 12;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n=========================================================");
            Console.WriteLine(isUa ? "                   РОЗРАХУНОК ПЛАНУ                      " : "                    PLAN CALCULATIONS                    ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            Console.WriteLine(isUa
                ? $"Крок 1. Реальна вартість цілі через {years} років (інфляція 2.14%): ${futurePrice:F2}"
                : $"Step 1. Real goal cost in {years} years (2.14% inflation): ${futurePrice:F2}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(isUa
                ? $"Крок 2. Необхідно інвестувати за 1 рік:                      ${annualContribution:F2}"
                : $"Step 2. Required investment per year:                        ${annualContribution:F2}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(isUa
                ? $"➜ Тобі потрібно вкладати в місяць:                           ${monthlyContribution:F2}"
                : $"➜ You need to invest per month:                              ${monthlyContribution:F2}");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n---------------------------------------------------------");
            Console.WriteLine(isUa ? "📋 ЩО ТОБІ ПОТРІБНО ЗРОБИТИ ЗАРАЗ (ЧЕК-ЛИСТ):" : "📋 YOUR NEXT STEPS (CHECKLIST):");
            Console.WriteLine("---------------------------------------------------------");
            Console.ResetColor();

            if (isUa)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"✓ [ ] Тобі потрібно вкладати в місяць: ${monthlyContribution:F2}");
                Console.ResetColor();
                Console.WriteLine("✓ [ ] Порахуй свій щомісячний дохід: скільки маєш зараз і скільки потрібно, щоб відкладати цю суму.");
                Console.WriteLine("✓ [ ] Не будь перфекціоністом: якщо цифри лякають, збільш термін або розбий ціль на етапи.");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n🌟 Головне — це регулярність внесків! Твої цілі абсолютно реальні.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"✓ [ ] You need to invest per month: ${monthlyContribution:F2}");
                Console.ResetColor();
                Console.WriteLine("✓ [ ] Calculate your monthly budget: look at what you have and what changes are needed.");
                Console.WriteLine("✓ [ ] Don't be a perfectionist: if numbers scare you, extend the deadline or divide the goal.");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n🌟 Consistency is key! Your goals are absolutely within reach.");
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}"); 
            Console.ReadKey(true);
            Console.ResetColor();
        }

        private static decimal GetInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Replace(",", ".") ?? "";

                if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value) && value >= 0) //
                {
                    return value;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(LocalizationManager.Get("InvalidInput"));
                Console.ResetColor();
            }
        }
    }
}