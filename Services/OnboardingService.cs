using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SmartDividendTracker.Models;

namespace SmartDividendTracker.Services
{
    public class OnboardingService
    {
        private readonly string _filePath = "user_profile.json";

        public UserProfile RunOrLoadProfile()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    string json = File.ReadAllText(_filePath);
                    var profile = JsonSerializer.Deserialize<UserProfile>(json);
                    if (profile != null)
                    {
                        return profile;
                    }
                }
                catch (Exception)
                {
                }
            }

            return RunNewOnboarding();
        }

        private UserProfile RunNewOnboarding()
        {
            Console.Clear();

            string langHeader = "    WELCOME TO SMART DIVIDEND TRACKER\n" +
                                "---------------------------------------\n" +
                                "Select your preferred language /\nОберіть мову інтерфейсу:";

            var langOptions = new List<string>
            {
                "English (EN)",
                "Українська (UA)"
            };

            int langChoice = ConsoleHelper.SelectOption(langHeader, langOptions);
            string selectedLanguage = langChoice == 1 ? "UA" : "EN";

            string expHeader = selectedLanguage == "UA" ?
                "Оберіть ваш рівень досвіду:" :
                "Select your investment experience level:";

            var expOptions = selectedLanguage == "UA" ? new List<string>
            {
                "Новачок (Beginner)",
                "Досвідчений (Experienced)"
            } : new List<string>
            {
                "Beginner (I am new to this)",
                "Experienced (I have an existing portfolio)"
            };

            int expChoice = ConsoleHelper.SelectOption(expHeader, expOptions);
            ExperienceLevel selectedExp = expChoice == 1 ? ExperienceLevel.Experienced : ExperienceLevel.Beginner;

            var newProfile = new UserProfile
            {
                Language = selectedLanguage,
                Experience = selectedExp,
                HasCompletedTutorial = false
            };

            SaveProfile(newProfile);
            return newProfile;
        }

        public void SaveProfile(UserProfile profile)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(profile, options);
            File.WriteAllText(_filePath, json);
        }

        public void OpenSettings(UserProfile profile)
        {
            int lastChoice = 0;

            while (true)
            {
                bool isUa = profile.Language.ToString() == "UA" || profile.Language.ToString() == "Ukrainian";

                string expText = profile.Experience.ToString();
                if (isUa)
                {
                    expText = profile.Experience == ExperienceLevel.Beginner ? "Новачок" : "Досвідчений";
                }

                string title = isUa ? "      НАЛАШТУВАННЯ ПРОФІЛЮ" : "        PROFILE SETTINGS";
                string header = $"{title}\n\n" +
                                (isUa ? $"Поточна мова: {profile.Language}\nРівень досвіду: {expText}" : $"Current Language: {profile.Language}\nCurrent Experience Level: {profile.Experience}");

                var options = isUa ? new List<string>
                {
                    "Змінити мову",
                    "Скинути налаштування (Видалити профіль)",
                    "Повернутися до Головного Меню"
                } : new List<string>
                {
                    "Change Language",
                    "Factory Reset (Delete Profile & Progress)",
                    "Back to Main Menu"
                };

                int choice = ConsoleHelper.SelectOption(header, options, lastChoice);
                lastChoice = choice;

                if (choice == 0)
                {
                    string langHeader = isUa ? "Оберіть нову мову:" : "Select new language:";
                    var langOptions = new List<string> { "English (EN)", "Українська (UA)" };

                    int newLangChoice = ConsoleHelper.SelectOption(langHeader, langOptions);
                    profile.Language = newLangChoice == 1 ? "UA" : "EN";

                    SaveProfile(profile);

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(profile.Language == "UA" ? "\nМову успішно оновлено!" : "\nLanguage updated successfully!");
                    Console.ResetColor();
                    Console.WriteLine(profile.Language == "UA" ? "Натисніть будь-яку клавішу для продовження..." : "Press any key to continue...");
                    Console.ReadKey(true);
                }
                else if (choice == 1)
                {
                    ResetProfile(isUa);
                }
                else if (choice == 2)
                {
                    break;
                }
            }
        }

        public void ResetProfile(bool isUa)
        {
            string header = isUa ?
                            "!!! УВАГА !!!\nВи збираєтеся видалити свій профіль, налаштування та прогрес.\nВи впевнені, що хочете повністю видалити профіль?" :
                            "!!! WARNING !!!\nYou are about to delete your profile, settings, and tutorial progress.\nAre you sure you want to completely delete your profile?";

            var options = isUa ? new List<string>
            {
                "НІ, Скасувати дію",
                "ТАК, Видалити все"
            } : new List<string>
            {
                "NO, Cancel Action",
                "YES, Delete Everything"
            };

            int confirmChoice = ConsoleHelper.SelectOption(header, options);

            if (confirmChoice == 1)
            {
                if (File.Exists(_filePath))
                {
                    File.Delete(_filePath);
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(isUa ? "\nПрофіль успішно видалено. Програму буде закрито." : "\nProfile successfully deleted. The application will now close.");
                Console.ResetColor();
                Console.WriteLine(isUa ? "Будь ласка, запустіть програму знову, щоб створити новий профіль." : "Please restart the app to create a new profile from scratch.");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }
    }
}