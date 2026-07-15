using System;
using System.Globalization;

namespace SmartDividendTracker.Services
{
    public static class CompoundCalculatorService
    {
        public static void RunCalculator(bool isUa)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=======================================");
            Console.WriteLine($"  {LocalizationManager.Get("CompoundCalc").ToUpper()}"); 
            Console.WriteLine("=======================================\n");
            Console.ResetColor();

            decimal? inputPrincipal = GetInput(LocalizationManager.Get("CalcInitial"), true);
            if (inputPrincipal == null) return;
            decimal principal = inputPrincipal.Value;

            decimal? inputMonthly = GetInput(LocalizationManager.Get("CalcMonthly"));
            if (inputMonthly == null) return;
            decimal monthlyContribution = inputMonthly.Value;

            decimal? inputRate = GetInput(LocalizationManager.Get("CalcRate"));
            if (inputRate == null) return;
            decimal annualRate = inputRate.Value;

            decimal? inputYears = GetInput(LocalizationManager.Get("CalcYears"));
            if (inputYears == null) return;
            int years = (int)inputYears.Value;

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

            Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
            Console.ReadKey(true);
        }

        private static decimal? GetInput(string prompt, bool allowCancel = false)
        {
            while (true)
            {
                string hint = allowCancel ? $" ({LocalizationManager.Get("CancelHint")})" : "";
                Console.Write($"{prompt}{hint}: ");

                string input = Console.ReadLine()?.Trim().Replace(",", ".") ?? "";

                if (string.IsNullOrEmpty(input) && allowCancel)
                    return null;

                if (!string.IsNullOrEmpty(input) && decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value) && value >= 0)
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(LocalizationManager.Get("InvalidInput"));
                Console.ResetColor();
            }
        }
    }
}