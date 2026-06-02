using System;

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

            // Збираємо дані
            decimal principal = GetInput(isUa ? "Початковий капітал ($): " : "Initial Investment ($): ", isUa);
            decimal monthlyContribution = GetInput(isUa ? "Щомісячне поповнення ($): " : "Monthly Contribution ($): ", isUa);
            decimal annualRate = GetInput(isUa ? "Очікувана річна дохідність (%): " : "Expected Annual Return (%): ", isUa);
            int years = (int)GetInput(isUa ? "Термін інвестування (років): " : "Investment Horizon (years): ", isUa);

            // Математика складного відсотка
            double r = (double)annualRate / 100;
            int n = 12; // Капіталізація щомісяця
            int t = years;

            double futureValuePrincipal = (double)principal * Math.Pow(1 + r / n, n * t);
            double futureValueContributions = 0;

            if (r > 0)
            {
                futureValueContributions = (double)monthlyContribution * ((Math.Pow(1 + r / n, n * t) - 1) / (r / n));
            }
            else
            {
                futureValueContributions = (double)monthlyContribution * n * t;
            }

            double totalFutureValue = futureValuePrincipal + futureValueContributions;
            double totalInvested = (double)principal + ((double)monthlyContribution * n * t);
            double totalInterest = totalFutureValue - totalInvested;

            // Вивід результатів
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
            Console.WriteLine(isUa ? "\nНатисніть будь-яку клавішу для повернення..." : "\nPress any key to return...");
            Console.ReadKey(true);
            Console.ResetColor();
        }

        // Універсальний метод для зчитування чисел із захистом від помилок
        private static decimal GetInput(string prompt, bool isUa)
        {
            decimal value;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Replace(".", ",");

                if (decimal.TryParse(input, out value) && value >= 0)
                {
                    return value;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(isUa ? "Невірний ввід. Будь ласка, введіть додатнє число." : "Invalid input. Please enter a positive number.");
                Console.ResetColor();
            }
        }
    }
}