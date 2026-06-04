using System;
using System.Collections.Generic;
using System.Globalization;
using SmartDividendTracker.Models;

namespace SmartDividendTracker.Services
{
    public static class GoalCalculatorService
    {
        public static void Run(UserProfile profile, OnboardingService onboarding)
        {
            bool isUa = profile.Language == "UA";
            int lastChoice = 0;

            while (true)
            {
                Console.Clear();

                string header = isUa ? "  🎯 МЕНЕДЖЕР ФІНАНСОВИХ ЦІЛЕЙ" : "  🎯 FINANCIAL GOAL MANAGER";

                var options = isUa ? new List<string>
                {
                    "1. Розрахувати та зберегти нову ціль",
                    $"2. Переглянути мої цілі ({profile.SavedCustomGoals.Count})",
                    "3. Видалити фінансову ціль",
                    "4. Повернутися в головне меню"
                } : new List<string>
                {
                    "1. Calculate and save a new goal",
                    $"2. View my saved goals ({profile.SavedCustomGoals.Count})",
                    "3. Delete a financial goal",
                    "4. Back to Main Menu"
                };

                int choice = ConsoleHelper.SelectOption(header, options, lastChoice);
                lastChoice = choice;

                if (choice == 0) CalculateAndSaveNewGoal(profile, onboarding, isUa);
                else if (choice == 1) ViewSavedGoals(profile, isUa);
                else if (choice == 2) DeleteGoal(profile, onboarding, isUa);
                else if (choice == 3) break;
            }
        }

        private static void CalculateAndSaveNewGoal(UserProfile profile, OnboardingService onboarding, bool isUa)
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
                                  "Як орієнтир для долара береться інфляція 2.14% річних.\n");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("📌 Крок 2. Рахуємо, скільки треба відкладати щомісяця");
                Console.ResetColor();
                Console.WriteLine("Ми беремо майбутню ціну і ділимо її на коефіцієнт складного відсотка.\n");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Натисни будь-яку клавішу, щоб перейти до розрахунку...");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("📌 Step 1. Find the 'real' cost through the years (with inflation)");
                Console.ResetColor();
                Console.WriteLine("We use a standard 2.14% annual inflation rate for the USD.\n");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("📌 Step 2. Calculate your monthly investment target");
                Console.ResetColor();
                Console.WriteLine("We divide the future cost by the compound interest factor.\n");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Press any key to start calculating your own goal...");
            }
            Console.ReadKey(true);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=========================================================");
            Console.WriteLine(isUa ? "                НОВИЙ ФІНАНСОВИЙ ПЛАН ЦІЛІ               " : "                     NEW GOAL PLAN                       ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            Console.Write(isUa ? " Назви свою ціль (напр. Квартира): " : " Name your goal (e.g. Apartment): ");
            string goalName = Console.ReadLine()?.Trim() ?? (isUa ? "Ціль" : "Goal");
            if (string.IsNullOrEmpty(goalName)) goalName = isUa ? "Ціль" : "Goal";

            decimal currentPrice = GetInput(isUa ? "1. Ціна твоєї цілі сьогодні ($): " : "1. Current cost of your goal ($): ");
            int years = (int)GetInput(isUa ? "2. Через скільки років плануєш купівлю: " : "2. Target horizon (years): ");
            decimal annualReturn = GetInput(isUa ? "3. Очікувана річна дохідність інвестицій (%): " : "3. Expected annual return (%): ");

            double inflationRate = 0.0214;
            double futurePrice = (double)currentPrice * Math.Pow(1 + inflationRate, years);
            double r = (double)annualReturn / 100;
            double annualContribution = r > 0 ? futurePrice / ((Math.Pow(1 + r, years) - 1) / r) : futurePrice / years;
            double monthlyContribution = annualContribution / 12;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n=========================================================");
            Console.WriteLine(isUa ? "                   РОЗРАХУНОК ПЛАНУ                      " : "                    PLAN CALCULATIONS                    ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            Console.WriteLine(isUa
                ? $"🎯 Ціль: {goalName}\n" +
                  $"• Вартість сьогодні: ${currentPrice:F2}\n" +
                  $"• Вартість через {years} років (з інфляцією 2.14%): ${futurePrice:F2}"
                : $"🎯 Goal: {goalName}\n" +
                  $"• Cost today: ${currentPrice:F2}\n" +
                  $"• Cost in {years} years (2.14% inflation): ${futurePrice:F2}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(isUa
                ? $"➜ НЕОБХІДНО ІНВЕСТУВАТИ ЩОМІСЯЦЯ: ${monthlyContribution:F2}"
                : $"➜ REQUIRED MONTHLY INVESTMENT: ${monthlyContribution:F2}");
            Console.ResetColor();

            Console.Write(isUa ? "\n💾 Бажаєш зберегти цю ціль у свій профіль? (ТАК/НІ): " : "\n💾 Do you want to save this goal to your profile? (YES/NO): ");
            string answer = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (answer == "ТАК" || answer == "YES")
            {
                var newGoal = new CustomGoal
                {
                    Name = goalName,
                    CurrentPrice = currentPrice,
                    Years = years,
                    AnnualReturn = annualReturn,
                    FuturePrice = futurePrice,
                    MonthlyContribution = monthlyContribution
                };

                if (profile.SavedCustomGoals == null) profile.SavedCustomGoals = new List<CustomGoal>();
                profile.SavedCustomGoals.Add(newGoal);

                onboarding.SaveProfile(profile);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(isUa ? "\n✔ Ціль успішно збережено!" : "\n✔ Goal saved successfully!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(isUa ? "\nРозрахунок завершено без збереження." : "\nCalculation finished without saving.");
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
            Console.ReadKey(true);
        }

        private static void ViewSavedGoals(UserProfile profile, bool isUa)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================================================================");
            Console.WriteLine(isUa ? "                                 СПИСОК МОЇХ ФІНАНСОВИХ ЦІЛЕЙ                            " : "                                      MY SAVED FINANCIAL GOALS                           ");
            Console.WriteLine("=========================================================================================\n");
            Console.ResetColor();

            if (profile.SavedCustomGoals == null || profile.SavedCustomGoals.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(isUa ? "У тебе поки немає збережених цілей. Прорахуй щось у пункті 1!" : "You have no saved goals yet. Calculate one in option 1!");
                Console.ResetColor();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(isUa
                    ? $"| {"Назва цілі",-15} | {"Ціна зараз",-12} | {"Років",-5} | {"Дохідн.",-7} | {"Ціна майбут.",-14} | {"Внесок/міс",-12} |"
                    : $"| {"Goal Name",-15} | {"Price Today",-12} | {"Years",-5} | {"Return",-7} | {"Future Price",-14} | {"Monthly Inv",-12} |");
                Console.ResetColor();
                Console.WriteLine(new string('-', 83));

                foreach (var goal in profile.SavedCustomGoals)
                {
                    Console.WriteLine($"| {goal.Name,-15} | ${goal.CurrentPrice,-11:F0} | {goal.Years,-5} | {goal.AnnualReturn,-5:F0}% | ${goal.FuturePrice,-12:F2} | ${goal.MonthlyContribution,-11:F2} |");
                }
                Console.WriteLine(new string('-', 83));
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
            Console.ReadKey(true);
            Console.ResetColor();
        }

        private static void DeleteGoal(UserProfile profile, OnboardingService onboarding, bool isUa)
        {
            if (profile.SavedCustomGoals == null || profile.SavedCustomGoals.Count == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(isUa ? "\nУ тебе немає збережених цілей для видалення." : "\nYou have no saved goals to delete.");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
                Console.ReadKey(true);
                return;
            }

            var goalOptions = new List<string>();
            foreach (var goal in profile.SavedCustomGoals)
            {
                goalOptions.Add($"{goal.Name,-15} | {goal.Years} {EnvironmentText(goal.Years, isUa)} ➜ ${goal.MonthlyContribution:F2}/міс");
            }
            goalOptions.Add(isUa ? "[ Скасувати дію ]" : "[ Cancel Action ]");

            string prompt = isUa ? "Оберіть фінансову ціль, яку бажаєте видалити:" : "Select a financial goal to delete:";
            int choice = ConsoleHelper.SelectOption(prompt, goalOptions);

            if (choice < profile.SavedCustomGoals.Count)
            {
                string deletedName = profile.SavedCustomGoals[choice].Name;
                profile.SavedCustomGoals.RemoveAt(choice);

                onboarding.SaveProfile(profile);

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(isUa
                    ? $"\n✔ Ціль «{deletedName}» успешно видалено з твого профілю!"
                    : $"\n✔ Goal \"{deletedName}\" has been successfully removed!");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
                Console.ReadKey(true);
            }
        }

        private static string EnvironmentText(int years, bool isUa)
        {
            if (!isUa) return years == 1 ? "year" : "years";

            int lastDigit = years % 10;
            int lastTwoDigits = years % 100;

            if (lastTwoDigits >= 11 && lastTwoDigits <= 14) return "років";
            if (lastDigit == 1) return "рік";
            if (lastDigit >= 2 && lastDigit <= 4) return "роки";
            return "років";
        }

        private static decimal GetInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Replace(",", ".") ?? "";
                if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value) && value >= 0)
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(LocalizationManager.Get("InvalidInput"));
                Console.ResetColor();
            }
        }
    }
}