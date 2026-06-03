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
            Console.WriteLine(isUa ? "                КАЛЬКУЛЯТОР ФІНАНСОВОЇ ЦІЛІ              " : "                     GOAL CALCULATOR                     ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            // Введення трьох базових цифр
            decimal currentPrice = GetInput(isUa ? "1. Ціна цілі сьогодні ($): " : "1. Current cost of the goal ($): ");
            int years = (int)GetInput(isUa ? "2. Через скільки років плануєш купівлю: " : "2. Target horizon (years): ");
            decimal annualReturn = GetInput(isUa ? "3. Очікувана річна дохідність інвестицій (%): " : "3. Expected annual return (%): ");

            // Крок 1. Рахуємо реальну ціну з урахуванням інфляції долара (2.14%)
            double inflationRate = 0.0214;
            double futurePrice = (double)currentPrice * Math.Pow(1 + inflationRate, years);

            // Крок 2. Рахуємо річний та щомісячний внесок за формулою ануїтету
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

            // Вивід результатів розрахунку
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n=========================================================");
            Console.WriteLine(isUa ? "                   РОЗРАХУНОК ПЛАНУ                      " : "                    PLAN CALCULATIONS                    ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            Console.WriteLine(isUa
                ? $" 1. Очікувана вартість цілі через {years} років (інфляція 2.14%): ${futurePrice:F2}"
                : $" 1. Expected goal cost in {years} years (2.14% inflation): ${futurePrice:F2}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(isUa
                ? $" 2. Необхідно інвестувати за 1 рік:                      ${annualContribution:F2}"
                : $" 2. Required investment per year:                        ${annualContribution:F2}");

            // Рядок із сумою, яку треба вкладати в місяць (робимо яскравий акцент)
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(isUa
                ? $"➜ Скільки тобі потрібно вкладати в місяць:                           ${monthlyContribution:F2}"
                : $"➜ How much you need to invest per month:                              ${monthlyContribution:F2}");
            Console.ResetColor();

            // Чек-лист із завданнями (без згадки про зошит)
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n---------------------------------------------------------");
            Console.WriteLine(isUa ? "📋 ЩО ТОБІ ПОТРІБНО ЗРОБИТИ ЗАРАЗ (ЧЕК-ЛИСТ):" : "📋 YOUR NEXT STEPS (CHECKLIST):");
            Console.WriteLine("---------------------------------------------------------");
            Console.ResetColor();

            if (isUa)
            {
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

                if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value) && value >= 0)
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