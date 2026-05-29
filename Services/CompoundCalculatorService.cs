using System;
using System.Globalization;

namespace SmartDividendTracker.Services
{
    public static class CompoundCalculatorService
    {
        public static void RunCalculator()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', 78));

            string title = LocalizationManager.Get("CalcMenuTitle");
            int spaces = (78 - title.Length) / 2;
            Console.WriteLine(new string(' ', spaces > 0 ? spaces : 0) + title);

            Console.WriteLine(new string('=', 78) + "\n");
            Console.ResetColor();

            // 1. Ввід щомісячного поповнення
            decimal monthlyContribution = 0;
            while (true)
            {
                Console.Write(LocalizationManager.Get("EnterMonthly"));
                string input = Console.ReadLine()?.Replace(".", ",") ?? "";
                if (decimal.TryParse(input, out monthlyContribution) && monthlyContribution >= 0) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(LocalizationManager.Get("InvalidInput"));
                Console.ResetColor();
            }

            // 2. Ввід кінцевої дати (За замовчуванням: +10 років від сьогодні)
            DateTime startDate = DateTime.Today;
            DateTime targetDate = startDate.AddYears(10);

            while (true)
            {
                Console.Write(LocalizationManager.Get("EnterEndDate"));
                string input = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(input)) break; // Якщо Enter - залишаємо +10 років

                // Сувора перевірка формату дд.ММ.рррр, щоб уникнути неіснуючих дат
                if (DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    if (parsedDate > startDate)
                    {
                        targetDate = parsedDate;
                        break;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(LocalizationManager.Get("InvalidDate"));
                Console.ResetColor();
            }

            // 3. Ввід відсотка дохідності (Default: 8%)
            decimal annualRate = 8m;
            while (true)
            {
                Console.Write(LocalizationManager.Get("EnterRate"));
                string input = Console.ReadLine()?.Replace(".", ",") ?? "";
                if (string.IsNullOrWhiteSpace(input)) break;

                if (decimal.TryParse(input, out annualRate) && annualRate >= 0) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(LocalizationManager.Get("InvalidInput"));
                Console.ResetColor();
            }

            // 4. Ввід інфляції (Default: 3%)
            decimal inflationRate = 3m;
            while (true)
            {
                Console.Write(LocalizationManager.Get("EnterInflation"));
                string input = Console.ReadLine()?.Replace(".", ",") ?? "";
                if (string.IsNullOrWhiteSpace(input)) break;

                if (decimal.TryParse(input, out inflationRate) && inflationRate >= 0) break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(LocalizationManager.Get("InvalidInput"));
                Console.ResetColor();
            }

            // Малюємо результати
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"--- Goal: {targetDate:dd.MM.yyyy} | ${monthlyContribution}/mo | Return: {annualRate}% | Inflation: {inflationRate}% ---\n");
            Console.ResetColor();

            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"| {LocalizationManager.Get("Year"),-4} | {LocalizationManager.Get("Invested"),-16} | {LocalizationManager.Get("NominalValue"),-20} | {LocalizationManager.Get("RealValue"),-22} |");
            Console.ResetColor();
            Console.WriteLine(new string('-', 75));

            decimal totalInvested = 0;
            decimal totalNominalValue = 0;
            decimal monthlyRate = annualRate / 100m / 12m;

            // Вираховуємо загальну кількість місяців між сьогодні і метою
            int totalMonths = ((targetDate.Year - startDate.Year) * 12) + targetDate.Month - startDate.Month;
            if (totalMonths <= 0) totalMonths = 1; // Захист від введення дат у поточному місяці

            int totalYearsSpan = (int)Math.Ceiling(totalMonths / 12.0);

            for (int m = 1; m <= totalMonths; m++)
            {
                totalInvested += monthlyContribution;
                totalNominalValue = (totalNominalValue + monthlyContribution) * (1m + monthlyRate);

                // Виводимо підсумок в кінці кожного року (кожні 12 місяців) АБО в самому кінці
                if (m % 12 == 0 || m == totalMonths)
                {
                    int currentYearPassed = m / 12;
                    DateTime currentDate = startDate.AddMonths(m);

                    // Реальна вартість
                    double timeInYears = m / 12.0;
                    double inflationFactor = Math.Pow((double)(1m + (inflationRate / 100m)), timeInYears);
                    decimal realValue = totalNominalValue / (decimal)inflationFactor;

                    // Якщо термін короткий (до 15 років) - показуємо кожен рік. Якщо довгий - кожні 5 років
                    bool shouldShowRow = (totalYearsSpan <= 15) || (currentYearPassed % 5 == 0) || (m == totalMonths);

                    if (shouldShowRow)
                    {
                        Console.WriteLine($"| {currentDate.Year,-4} | ${totalInvested,-15:F0} | ${totalNominalValue,-19:F0} | ${realValue,-21:F0} |");
                    }
                }
            }

            Console.WriteLine(new string('-', 75));
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n{LocalizationManager.Get("InflationNote")}");
            Console.ResetColor();

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey(true);
        }
    }
}