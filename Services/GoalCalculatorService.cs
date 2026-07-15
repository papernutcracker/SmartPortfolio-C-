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
            int lastChoice = 0;

            while (true)
            {
                Console.Clear();

                string header = LocalizationManager.Get("GoalMenuHeader");

                var options = new List<string>
                {
                    LocalizationManager.Get("GoalMenuOpt1"),
                    $"{LocalizationManager.Get("GoalMenuOpt2")} ({profile.SavedCustomGoals?.Count ?? 0})",
                    LocalizationManager.Get("GoalMenuOpt3"),
                    LocalizationManager.Get("GoalMenuOpt4")
                };

                int choice = ConsoleHelper.SelectOption(header, options, lastChoice);
                lastChoice = choice;

                if (choice == 0) CalculateAndSaveNewGoal(profile, onboarding);
                else if (choice == 1) ViewSavedGoals(profile);
                else if (choice == 2) DeleteGoal(profile, onboarding);
                else if (choice == 3) break;
            }
        }

        private static void CalculateAndSaveNewGoal(UserProfile profile, OnboardingService onboarding)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=========================================================");
            Console.WriteLine(LocalizationManager.Get("GoalHowItWorks"));
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(LocalizationManager.Get("GoalStep1Title"));
            Console.ResetColor();
            Console.WriteLine(LocalizationManager.Get("GoalStep1Desc") + "\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(LocalizationManager.Get("GoalStep2Title"));
            Console.ResetColor();
            Console.WriteLine(LocalizationManager.Get("GoalStep2Desc") + "\n");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(LocalizationManager.Get("GoalPressAnyCalc"));
            Console.ReadKey(true);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=========================================================");
            Console.WriteLine(LocalizationManager.Get("GoalNewPlanHeader"));
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            // 1. ЗАПИТ НАЗВИ 
            Console.Write($"{LocalizationManager.Get("GoalNamePrompt")} ({LocalizationManager.Get("CancelHint")}): ");
            string goalName = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(goalName))
            {
                return;
            }

            // 2. ЗАПИТ ЧИСЕЛ
            decimal? currentPriceInput = GetInput(LocalizationManager.Get("GoalPricePrompt"));
            if (currentPriceInput == null) return;
            decimal currentPrice = currentPriceInput.Value;

            decimal? yearsInput = GetInput(LocalizationManager.Get("GoalYearsPrompt"));
            if (yearsInput == null) return;
            int years = (int)yearsInput.Value;

            decimal? returnInput = GetInput(LocalizationManager.Get("GoalRatePrompt"));
            if (returnInput == null) return;
            decimal annualReturn = returnInput.Value;

            double inflationRate = 0.0214;
            double futurePrice = (double)currentPrice * Math.Pow(1 + inflationRate, years);
            double r = (double)annualReturn / 100;
            double annualContribution = r > 0 ? futurePrice / ((Math.Pow(1 + r, years) - 1) / r) : futurePrice / years;
            double monthlyContribution = annualContribution / 12;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n=========================================================");
            Console.WriteLine(LocalizationManager.Get("GoalCalcHeader"));
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            Console.WriteLine(
                $"{LocalizationManager.Get("GoalTarget")} {goalName}\n" +
                $"{LocalizationManager.Get("GoalCostToday")} ${currentPrice:F2}\n" +
                $"{string.Format(LocalizationManager.Get("GoalCostFuture"), years)} ${futurePrice:F2}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{LocalizationManager.Get("GoalReqInv")} ${monthlyContribution:F2}");
            Console.ResetColor();

            Console.Write(LocalizationManager.Get("GoalSavePrompt"));
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
                Console.WriteLine(LocalizationManager.Get("GoalSavedSuccess"));
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(LocalizationManager.Get("GoalSavedCancel"));
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
            Console.ReadKey(true);
        }

        private static void ViewSavedGoals(UserProfile profile)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================================================================");
            Console.WriteLine(LocalizationManager.Get("GoalListHeader"));
            Console.WriteLine("=========================================================================================\n");
            Console.ResetColor();

            if (profile.SavedCustomGoals == null || profile.SavedCustomGoals.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(LocalizationManager.Get("GoalListEmpty"));
                Console.ResetColor();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(
                    $"| {LocalizationManager.Get("TblGoalName"),-15} | {LocalizationManager.Get("TblPriceToday"),-12} | " +
                    $"{LocalizationManager.Get("TblYears"),-5} | {LocalizationManager.Get("TblReturn"),-7} | " +
                    $"{LocalizationManager.Get("TblFuturePrice"),-14} | {LocalizationManager.Get("TblMonthlyInv"),-12} |");
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

        private static void DeleteGoal(UserProfile profile, OnboardingService onboarding)
        {
            if (profile.SavedCustomGoals == null || profile.SavedCustomGoals.Count == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(LocalizationManager.Get("GoalDelEmpty"));
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
                Console.ReadKey(true);
                return;
            }

            var goalOptions = new List<string>();
            foreach (var goal in profile.SavedCustomGoals)
            {
                goalOptions.Add($"{goal.Name,-15} | {goal.Years} {LocalizationManager.Get("GoalYearsWord")} ➜ ${goal.MonthlyContribution:F2}/міс");
            }
            goalOptions.Add(LocalizationManager.Get("GoalDelCancel"));

            string prompt = LocalizationManager.Get("GoalDelPrompt");
            int choice = ConsoleHelper.SelectOption(prompt, goalOptions);

            if (choice < profile.SavedCustomGoals.Count)
            {
                string deletedName = profile.SavedCustomGoals[choice].Name;
                profile.SavedCustomGoals.RemoveAt(choice);

                onboarding.SaveProfile(profile);

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(string.Format(LocalizationManager.Get("GoalDelSuccess"), deletedName));
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\n{LocalizationManager.Get("PressEnter")}");
                Console.ReadKey(true);
            }
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