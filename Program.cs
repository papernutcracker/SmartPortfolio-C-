using System;
using System.Collections.Generic;
using SmartDividendTracker.Models;
using SmartDividendTracker.Services;

namespace SmartDividendTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            var onboarding = new OnboardingService();
            UserProfile currentUser = onboarding.RunOrLoadProfile();

            LocalizationManager.SetLanguage(currentUser.Language);

            var portfolioManager = new PortfolioManager();

            // Передаємо onboarding сюди, щоб можна було викликати меню налаштувань
            ShowMainMenu(currentUser, portfolioManager, onboarding);
        }

        static void ShowMainMenu(UserProfile profile, PortfolioManager portfolioManager, OnboardingService onboarding)
        {
            int lastMainChoice = 0;

            while (true)
            {
                // 1. Формуємо красивий список цілей
                var localizedGoalsList = new List<string>();
                foreach (var goal in profile.Goals)
                {
                    localizedGoalsList.Add(goal switch
                    {
                        InvestmentGoal.PassiveIncome => LocalizationManager.Get("GoalPassive"),
                        InvestmentGoal.CapitalGrowth => LocalizationManager.Get("GoalGrowth"),
                        InvestmentGoal.MajorPurchase => LocalizationManager.Get("GoalPurchase"),
                        _ => goal.ToString()
                    });
                }
                string goalsText = string.Join(", ", localizedGoalsList);

                // 2. НОВЕ: Формуємо красивий текст для горизонту (з пробілами)
                string localizedHorizon = profile.Horizon switch
                {
                    InvestmentHorizon.UpTo5Years => LocalizationManager.Get("Horiz5"),
                    InvestmentHorizon.UpTo10Years => LocalizationManager.Get("Horiz10"),
                    InvestmentHorizon.LongTerm => LocalizationManager.Get("HorizMore"),
                    _ => profile.Horizon.ToString()
                };

                // 3. Формуємо заголовок
                string header = $"{LocalizationManager.Get("MainMenu")}\n" +
                                $"[Goals: {goalsText} | Horizon: {localizedHorizon} | Level: {profile.Experience}]";

                var menuOptions = new List<string>
                {
                    LocalizationManager.Get("MenuOpt1"),
                    LocalizationManager.Get("MenuOpt2"),
                    LocalizationManager.Get("MenuOpt4"),
                    LocalizationManager.Get("MenuOpt3")
                };


                int choice = ConsoleHelper.SelectOption(header, menuOptions, lastMainChoice);

                lastMainChoice = choice;
                // ... далі код if/else виклику меню залишається без змін ...

                if (choice == 0)
                {
                    Console.WriteLine("\n[Portfolio module will be here...]");
                    Console.WriteLine("Press any key to return...");
                    Console.ReadKey(true);
                }
                else if (choice == 1)
                {
                    Console.WriteLine("\n[AI Market Analysis is under construction.]");
                    Console.WriteLine("Press any key to return...");
                    Console.ReadKey(true);
                }
                else if (choice == 2)
                {
                    // ВИКЛИКАЄМО МЕНЮ НАЛАШТУВАНЬ
                    onboarding.OpenSettings(profile);
                }
                else if (choice == 3)
                {
                    // Викликаємо нашу нову анімацію перед виходом
                    ConsoleHelper.ShowExitAnimation(LocalizationManager.Get("ExitMessage"));
                    break;
                }
            }
        }
    }
}