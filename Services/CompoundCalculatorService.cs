using System;
using System.Globalization;

namespace SmartDividendTracker.Services
{
    public static class CompoundCalculatorService
    {
        public static void RunCalculator(bool isUa)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=========================================================");
            Console.WriteLine(isUa ? "             КАЛЬКУЛЯТОР СКЛАДНОГО ВІДСОТКА              " : "             COMPOUND INTEREST CALCULATOR                ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            decimal principal = GetInput(isUa ? "Початковий капітал ($): " : "Initial Investment ($): ");
            decimal monthlyContribution = GetInput(isUa ? "Щомісячне поповнення ($): " : "Monthly Contribution ($): ");
            decimal annualRate = GetInput(isUa ? "Очікувана річна дохідність (%): " : "Expected Annual Return (%): ");
            int years = (int)GetInput(isUa ? "Термін інвестування (років): " : "Investment Horizon (years): ");

            double r = (double)annualRate / 100;
            int n = 12;
            int t = years;

            double futureValuePrincipal = (double)principal * Math.Pow(1 + r / n, n * t);
            double futureValueContributions = r > 0
                ? (double)monthlyContribution * ((Math.Pow(1 + r / n, n * t) - 1) / (r / n))
                : (double)monthlyContribution * n * t;

            double totalFutureValue = futureValuePrincipal + futureValueContributions;
            double totalInvested = (double)principal + ((double)monthlyContribution * n * t);
            double totalInterest = totalFutureValue - totalInvested;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n=========================================================");
            Console.WriteLine(isUa ? "                      РЕЗУЛЬТАТИ                         " : "                        RESULTS                          ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            Console.WriteLine(isUa ? $"Всього інвестовано власного капіталу: ${totalInvested:F2}" : $"Total Own Capital Invested: ${totalInvested:F2}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(isUa ? $"Зароблено складним відсотком:         ${totalInterest:F2}" : $"Interest Earned (Compound):           ${totalInterest:F2}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(isUa ? $"МАЙБУТНЯ ВАРТІСТЬ ПОРТФЕЛЯ:           ${totalFutureValue:F2}" : $"TOTAL FUTURE VALUE:                   ${totalFutureValue:F2}");
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
                // Замінюємо кому на крапку для уніфікації введення користувачем
                string input = Console.ReadLine()?.Replace(",", ".") ?? "";

                // Парсимо інваріантно (завжди очікує крапку як роздільник)
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