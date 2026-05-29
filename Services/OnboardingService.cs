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
        private readonly string _filePath = "user_profile.json";

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public UserProfile RunOrLoadProfile()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    string json = File.ReadAllText(_filePath);
                    UserProfile loadedProfile = JsonSerializer.Deserialize<UserProfile>(json, _jsonOptions);

                    if (loadedProfile != null)
                    {
                        return loadedProfile;
                    }
                }
                catch (JsonException)
                {
                    // Ignore corrupted JSON and create a new profile
                }
            }

            return CreateNewProfile();
        }

        public void SaveProfile(UserProfile profile)
        {
            string jsonString = JsonSerializer.Serialize(profile, _jsonOptions);
            File.WriteAllText(_filePath, jsonString);
        }

        private UserProfile CreateNewProfile()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine("   WELCOME TO SMART DIVIDEND PORTFOLIO TRACKER");
            Console.WriteLine("==================================================\n");
            Console.ResetColor();

            var profile = new UserProfile();

            var languages = new List<string> { "English", "Українська" };
            int langChoice = ConsoleHelper.SelectOption("Select your preferred language / Оберіть мову:", languages);
            profile.Language = langChoice == 0 ? "en" : "uk";

            LocalizationManager.SetLanguage(profile.Language);

            var expOptions = new List<string> { "Beginner", "Intermediate", "Advanced" };
            int expChoice = ConsoleHelper.SelectOption("Select your experience level / Оберіть рівень досвіду:", expOptions);
            profile.Experience = (ExperienceLevel)expChoice;

            var horizonOptions = new List<string> { "Up to 5 years", "Up to 10 years", "Long Term (10+ years)" };
            int horizChoice = ConsoleHelper.SelectOption("Select your investment horizon / Оберіть горизонт:", horizonOptions);
            profile.Horizon = (InvestmentHorizon)horizChoice;

            profile.Goals = new List<InvestmentGoal> { InvestmentGoal.PassiveIncome };

            profile.HasCompletedTutorial = false;

            SaveProfile(profile);
            return profile;
        }

        public void OpenSettings(UserProfile profile)
        {
            int lastChoice = 0;

            while (true)
            {
                Console.Clear();
                var options = new List<string>
                {
                    LocalizationManager.Get("ChangeLang"),
                    LocalizationManager.Get("ChangeExp"),
                    LocalizationManager.Get("ChangeHorizon"),
                    LocalizationManager.Get("ResetTutorial"),
                    LocalizationManager.Get("Back")
                };

                int choice = ConsoleHelper.SelectOption(LocalizationManager.Get("SettingsTitle"), options, lastChoice);
                lastChoice = choice;
                string selectedText = options[choice];

                if (selectedText == LocalizationManager.Get("ChangeLang"))
                {
                    var languages = new List<string> { "English", "Українська" };
                    int langChoice = ConsoleHelper.SelectOption("Select language / Оберіть мову:", languages);
                    profile.Language = langChoice == 0 ? "en" : "uk";

                    LocalizationManager.SetLanguage(profile.Language);
                    SaveProfile(profile);
                }
                else if (selectedText == LocalizationManager.Get("ChangeExp"))
                {
                    var expOptions = new List<string> { "Beginner", "Intermediate", "Advanced" };
                    int expChoice = ConsoleHelper.SelectOption("Select your experience level / Оберіть рівень досвіду:", expOptions);
                    profile.Experience = (ExperienceLevel)expChoice;
                    SaveProfile(profile);
                }
                else if (selectedText == LocalizationManager.Get("ChangeHorizon"))
                {
                    var horizonOptions = new List<string> { "Up to 5 years", "Up to 10 years", "Long Term (10+ years)" };
                    int horizChoice = ConsoleHelper.SelectOption("Select your investment horizon / Оберіть горизонт:", horizonOptions);
                    profile.Horizon = (InvestmentHorizon)horizChoice;
                    SaveProfile(profile);
                }
                else if (selectedText == LocalizationManager.Get("ResetTutorial"))
                {
                    profile.HasCompletedTutorial = false;
                    SaveProfile(profile);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{LocalizationManager.Get("TutResetSuccess")}");
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey(true);
                }
                else
                {
                    break;
                }
            }
        }
    }
}