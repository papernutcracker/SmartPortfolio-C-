using System;
using System.Collections.Generic;
using System.Linq;
using SmartDividendTracker.Models;
using SmartDividendTracker.Data; // Підключаємо нашу базу даних

namespace SmartDividendTracker.Services
{
    public class OnboardingService
    {
        public UserProfile RunOrLoadProfile()
        {
            // Звертаємося до бази даних замість JSON-файлу
            using (var db = new AppDbContext())
            {
                // Беремо першого користувача з таблиці (адже це локальний додаток)
                var profile = db.Users.FirstOrDefault();

                if (profile != null)
                {
                    return profile;
                }
            }

            // Якщо таблиця порожня (користувача немає), запускаємо онбординг
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
            using (var db = new AppDbContext())
            {
                // Якщо Id == 0, значить цього профілю ще немає в базі, його треба додати
                if (profile.Id == 0)
                {
                    db.Users.Add(profile);
                }
                else
                {
                    // Якщо Id вже є, значить ми просто оновлюємо існуючий рядок у таблиці
                    db.Users.Update(profile);
                }

                // Зберігаємо зміни
                db.SaveChanges();
            }
        }

        public void OpenSettings(UserProfile profile)
        {
            int lastSettingsChoice = 0;

            while (true)
            {
                bool isUa = profile.Language == "UA";
                string header = isUa ? "  ⚙️ НАЛАШТУВАННЯ ПРОФІЛЮ" : "  ⚙️ PROFILE SETTINGS";

                var options = isUa ? new List<string>
                {
                    "1. Змінити мову програми (UA/EN)",
                    "2. Повністю скинути профіль інвестора",
                    "3. Повернутися до Головного Меню"
                } : new List<string>
                {
                    "1. Change Language (UA/EN)",
                    "2. Reset Investor Profile",
                    "3. Back to Main Menu"
                };

                int choice = ConsoleHelper.SelectOption(header, options, lastSettingsChoice);
                lastSettingsChoice = choice;

                if (choice == 0)
                {
                    profile.Language = profile.Language == "UA" ? "EN" : "UA";
                    SaveProfile(profile);

                    LocalizationManager.SetLanguage(profile.Language == "UA" ? "uk" : "en");
                }
                else if (choice == 1)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(isUa
                        ? "⚠️ Ви впевнені, що хочете видалити профіль? (ТАК/НІ): "
                        : "⚠️ Are you sure you want to delete your profile? (YES/NO): ");
                    Console.ResetColor();

                    string confirm = Console.ReadLine()?.Trim().ToUpper() ?? "";
                    if (confirm == "ТАК" || confirm == "YES")
                    {
                        ResetProfile(isUa);
                        break;
                    }
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
                // ОЧИЩЕННЯ БАЗИ ДАНИХ
                using (var db = new AppDbContext())
                {
                    db.Users.RemoveRange(db.Users);   // Видаляємо всі профілі
                    db.Stocks.RemoveRange(db.Stocks); // Видаляємо всі прив'язані акції
                    db.Goals.RemoveRange(db.Goals);   // Видаляємо цілі (якщо вони вже в БД)

                    db.SaveChanges(); // Підтверджуємо масове видалення
                }

                // Також на всяк випадок видаляємо старий JSON файл, щоб він не плутав нас
                string oldFilePath = "user_profile.json";
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
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