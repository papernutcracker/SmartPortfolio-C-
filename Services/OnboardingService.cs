using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartDividendTracker.Models;

namespace SmartDividendTracker.Services
{
    public class OnboardingService
    {
        private const string ProfileFilePath = "user_profile.json";

        // Налаштування для читання та запису JSON (щоб Enum зберігалися як текст, а не цифри)
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public UserProfile RunOrLoadProfile()
        {
            if (File.Exists(ProfileFilePath))
            {
                try
                {
                    string json = File.ReadAllText(ProfileFilePath);
                    return JsonSerializer.Deserialize<UserProfile>(json, _jsonOptions);
                }
                catch (JsonException)
                {
                    // Якщо структура JSON змінилася (старий файл), ігноруємо його і створюємо новий
                    Console.Clear();
                    Console.WriteLine("Warning: Old or corrupted profile detected. Creating a new one...");
                    System.Threading.Thread.Sleep(2000);
                    return CreateNewProfile();
                }
            }

            return CreateNewProfile();
        }

        public void OpenSettings(UserProfile profile)
        {
            int lastSettingsChoice = 0; // Пам'ять для налаштувань

            while (true)
            {
                var options = new List<string>
                {
                    LocalizationManager.Get("SetLang"),
                    LocalizationManager.Get("SetExp"),
                    LocalizationManager.Get("SetGoals"),
                    LocalizationManager.Get("SetHorizon"),
                    LocalizationManager.Get("Back")
                };

                int choice = ConsoleHelper.SelectOption(LocalizationManager.Get("SettingsMenu"), options, lastSettingsChoice);
                lastSettingsChoice = choice;

                // БЕРЕМО ТЕКСТ ОБРАНОЇ КНОПКИ
                string selectedText = options[choice];

                // ПЕРЕВІРЯЄМО ПО ТЕКСТУ, А НЕ ПО ЦИФРАХ
                if (selectedText == LocalizationManager.Get("SetLang"))
                {
                    var langOptions = new List<string> { "English", "Українська" };
                    int langChoice = ConsoleHelper.SelectOption("Select Language / Оберіть мову", langOptions);
                    if (langChoice == 0) { profile.Language = "en"; LocalizationManager.SetLanguage("en"); }
                    else { profile.Language = "uk"; LocalizationManager.SetLanguage("uk"); }
                }
                else if (selectedText == LocalizationManager.Get("SetExp"))
                {
                    var expOptions = new List<string> { LocalizationManager.Get("ExpBeginner"), LocalizationManager.Get("ExpPro") };
                    int expChoice = ConsoleHelper.SelectOption(LocalizationManager.Get("ExpLevel"), expOptions);
                    profile.Experience = (expChoice == 0) ? ExperienceLevel.Beginner : ExperienceLevel.Experienced;
                }
                else if (selectedText == LocalizationManager.Get("SetGoals"))
                {
                    var goalOptions = new List<string>();
                    var availableGoals = new List<InvestmentGoal>();

                    if (profile.Horizon != InvestmentHorizon.UpTo5Years)
                    {
                        goalOptions.Add(LocalizationManager.Get("GoalPassive"));
                        availableGoals.Add(InvestmentGoal.PassiveIncome);
                    }
                    goalOptions.Add(LocalizationManager.Get("GoalGrowth"));
                    availableGoals.Add(InvestmentGoal.CapitalGrowth);
                    goalOptions.Add(LocalizationManager.Get("GoalPurchase"));
                    availableGoals.Add(InvestmentGoal.MajorPurchase);

                    List<int> goalChoices = ConsoleHelper.SelectMultipleOptions(LocalizationManager.Get("GoalPrompt"), goalOptions);

                    profile.Goals.Clear();
                    foreach (int c in goalChoices)
                    {
                        profile.Goals.Add(availableGoals[c]);
                    }
                }
                else if (selectedText == LocalizationManager.Get("SetHorizon"))
                {
                    var horizonOptions = new List<string> { LocalizationManager.Get("Horiz5"), LocalizationManager.Get("Horiz10"), LocalizationManager.Get("HorizMore") };
                    int horChoice = ConsoleHelper.SelectOption(LocalizationManager.Get("HorizonPrompt"), horizonOptions);
                    profile.Horizon = horChoice switch
                    {
                        0 => InvestmentHorizon.UpTo5Years,
                        1 => InvestmentHorizon.UpTo10Years,
                        _ => InvestmentHorizon.LongTerm
                    };

                    if (profile.Horizon == InvestmentHorizon.UpTo5Years && profile.Goals.Contains(InvestmentGoal.PassiveIncome))
                    {
                        profile.Goals.Remove(InvestmentGoal.PassiveIncome);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n=======================================");
                        Console.WriteLine(LocalizationManager.Get("GoalRemovedWarning"));
                        Console.WriteLine("=======================================\n");
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(3500);
                    }
                }
                else if (selectedText == LocalizationManager.Get("Back"))
                {
                    break; // Тепер кнопка "Назад" 100% працює коректно
                }

                SaveProfile(profile);
            }
        }

        private UserProfile CreateNewProfile()
        {
            var profile = new UserProfile();

            // 1. ВИБІР МОВИ
            var langOptions = new List<string> { "English", "Українська" };
            int langChoice = ConsoleHelper.SelectOption("Select Language / Оберіть мову", langOptions);

            if (langChoice == 1)
            {
                profile.Language = "uk";
                LocalizationManager.SetLanguage("uk");
            }

            // 2. РІВЕНЬ ДОСВІДУ
            var expOptions = new List<string> {
                LocalizationManager.Get("ExpBeginner"),
                LocalizationManager.Get("ExpPro")
            };
            int expChoice = ConsoleHelper.SelectOption(LocalizationManager.Get("ExpLevel"), expOptions);
            profile.Experience = (expChoice == 0) ? ExperienceLevel.Beginner : ExperienceLevel.Experienced;

            // 3. ГОРИЗОНТ ІНВЕСТУВАННЯ (Тепер він ПЕРЕД цілями!)
            var horizonOptions = new List<string> {
                LocalizationManager.Get("Horiz5"),
                LocalizationManager.Get("Horiz10"),
                LocalizationManager.Get("HorizMore")
            };
            int horChoice = ConsoleHelper.SelectOption(LocalizationManager.Get("HorizonPrompt"), horizonOptions);

            profile.Horizon = horChoice switch
            {
                0 => InvestmentHorizon.UpTo5Years,
                1 => InvestmentHorizon.UpTo10Years,
                _ => InvestmentHorizon.LongTerm
            };

            // 4. МЕТА ІНВЕСТУВАННЯ (Динамічний список)
            var goalOptions = new List<string>();
            var availableGoals = new List<InvestmentGoal>(); // Зберігає реальні Enum для обраних пунктів

            // Додаємо Дивіденди ТІЛЬКИ якщо горизонт більше 5 років
            if (profile.Horizon != InvestmentHorizon.UpTo5Years)
            {
                goalOptions.Add(LocalizationManager.Get("GoalPassive"));
                availableGoals.Add(InvestmentGoal.PassiveIncome);
            }

            // Ці цілі доступні завжди
            goalOptions.Add(LocalizationManager.Get("GoalGrowth"));
            availableGoals.Add(InvestmentGoal.CapitalGrowth);
            goalOptions.Add(LocalizationManager.Get("GoalPurchase"));
            availableGoals.Add(InvestmentGoal.MajorPurchase);

            List<int> goalChoices = ConsoleHelper.SelectMultipleOptions(LocalizationManager.Get("GoalPrompt"), goalOptions);

            profile.Goals.Clear();
            foreach (int c in goalChoices)
            {
                profile.Goals.Add(availableGoals[c]); // Беремо мету з масиву доступних
            }
            // 5. НАЯВНІСТЬ ПОРТФЕЛЯ
            var yesNoOptions = new List<string> {
                LocalizationManager.Get("Yes"),
                LocalizationManager.Get("No")
            };
            int portChoice = ConsoleHelper.SelectOption(LocalizationManager.Get("HasPort"), yesNoOptions);
            profile.HasExistingPortfolio = (portChoice == 0); // Індекс 0 відповідає варіанту "Yes"

            // ЗБЕРЕЖЕННЯ
            SaveProfile(profile);

            Console.Clear();
            Console.WriteLine($"\n{LocalizationManager.Get("ProfileSaved")}\n");
            System.Threading.Thread.Sleep(1500);

            return profile;
        }

        private void SaveProfile(UserProfile profile)
        {
            string json = JsonSerializer.Serialize(profile, _jsonOptions);
            File.WriteAllText(ProfileFilePath, json);
        }
    }
}