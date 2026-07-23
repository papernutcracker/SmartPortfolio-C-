//using System.Collections.Generic;

//namespace SmartDividendTracker.Services
//{
//    public static class LocalizationManager
//    {
//        private static string _currentLanguage = "en";

//        public static void SetLanguage(string lang)
//        {
//            _currentLanguage = lang.ToLower() == "uk" ? "uk" : "en";
//        }

//        public static string GetCurrentLanguage() => _currentLanguage;

//        private static readonly Dictionary<string, (string En, string Uk)> _phrases = new()
//        {
//            { "SelectLang", ("Select your language", "Оберіть вашу мову") },
//            { "Welcome", ("Welcome to Smart Dividend Tracker AI!", "Ласкаво просимо до Smart Dividend Tracker AI!") },
//            { "ExpLevel", ("What is your investment experience?", "Який ваш досвід інвестування?") },
//            { "ExpBeginner", ("Beginner (Just starting)", "Новачок (Тільки починаю)") },
//            { "ExpPro", ("Experienced", "Досвідчений") },
//            { "GoalInfo", ("Type your main investment goal (e.g., Passive Income): ", "Введіть вашу головну мету (напр., Пасивний дохід): ") },
//            { "HorizonPrompt", ("Select your investment horizon", "Оберіть ваш термін інвестування") },
//            { "Horiz5", ("Up to 5 years", "До 5 років") },
//            { "ViewSectorChart", ("View Sector Chart", "Переглянути діаграму часток") },

//            { "Horiz10", ("Up to 10 years", "До 10 років") },
//            { "HorizMore", ("More than 10 years", "Більше 10 років") },
//            { "HasPort", ("Do you already have an investment portfolio?", "Чи є у вас вже інвестиційний портфель?") },
//            { "Yes", ("Yes", "Так") },
//            { "No", ("No", "Ні") },
//            { "ProfileSaved", ("Profile saved successfully! Let's start.", "Профіль успішно збережено! Починаємо.") },
//            { "MainMenu", ("MAIN MENU", "ГОЛОВНЕ МЕНЮ") },
//            { "MenuOpt1", ("View Portfolio", "Переглянути портфель") },
//            { "MenuOpt2", ("Beginner's Cheat Sheet", "Шпаргалка новачка") },
//            { "MenuOpt3", ("Exit", "Вихід") },
//            { "GoalPrompt", ("Select your main investment goal", "Оберіть вашу головну мету інвестування") },
//            { "GoalPassive", ("Passive Income", "Пасивний дохід (Дивіденди)") },
//            { "GoalGrowth", ("Capital Growth", "Збільшення капіталу") },
//            { "GoalPurchase", ("Major Purchase (Real estate, etc.)", "Велика покупка (Нерухомість тощо)") },
//            { "GoalRemovedWarning", (
//                "Warning: 'Passive Income' goal was removed because it's too risky for a <5 years horizon.",
//                "Увага: Мету 'Пасивний дохід' вилучено, оскільки це ризиковано для горизонту до 5 років."
//            ) },
//            { "MenuOpt4", ("Profile Settings", "Налаштування профілю") },

//            { "SettingsTitle", ("SETTINGS", "НАЛАШТУВАННЯ") },
//            { "ChangeLang", ("Change Language", "Змінити мову") },
//            { "ChangeExp", ("Change Experience Level", "Змінити рівень досвіду") },
//            { "ChangeHorizon", ("Change Investment Horizon", "Змінити горизонт інвестування") },
//            { "ResetTutorial", ("Reset Tutorial", "Скинути навчання (Туторіал)") },
//            { "TutResetSuccess", ("Tutorial status has been reset! It will launch on next restart.", "Статус навчання скинуто! Воно запуститься при наступному старті.") },

//            { "SettingsMenu", ("PROFILE SETTINGS", "НАЛАШТУВАННЯ ПРОФІЛЮ") },
//            { "SetGoals", ("Change Investment Goals", "Змінити цілі інвестування") },
//            { "Back", ("Back to Main Menu", "Назад до головного меню") },
//            { "ExitMessage", ("Saving progress... See you later, Investor!", "Зберігаємо прогрес... До зустрічі, Інвесторе!") },
//            { "PortfolioMenu", ("PORTFOLIO MANAGEMENT", "УПРАВЛІННЯ ПОРТФЕЛЕМ") },
//            { "AddStock", ("Add New Asset", "Додати новий актив") },
//            { "ViewAssets", ("View All Assets", "Переглянути всі активи") },
//            { "RemoveStock", ("Remove Asset", "Видалити актив") },
//            { "SelectSector", ("Select Sector", "Оберіть сектор економіки") },
//            { "EnterPrice", ("Enter Average Price ($): ", "Введіть середню ціну ($): ") },
//            { "EnterShares", ("Enter Quantity of Shares: ", "Введіть кількість акцій: ") },
//            { "EnterYield", ("Enter Dividend Yield (%): ", "Введіть дивідендну дохідність (%): ") },
//            { "EnterPE", ("Enter P/E Ratio: ", "Введіть показник P/E: ") },
//            { "InvalidInput", ("Invalid input. Please enter a valid number.", "Некоректний ввід. Будь ласка, введіть число.") },
//            { "StockAdded", ("Asset successfully added to portfolio!", "Актив успішно додано до портфеля!") },
//            { "TblTicker", ("Ticker", "Тикер") },
//            { "TblSector", ("Sector", "Сектор") },
//            { "TblPrice", ("Price", "Ціна") },
//            { "TblShares", ("Qty", "К-ть") },
//            { "TblValue", ("Total Value", "Вартість") },
//            { "TblYield", ("Yield", "Дохідн.") },
//            { "TblAnnualDiv", ("Div/Yr", "Див/Рік") },
//            { "TotalIncome", ("Total Annual Dividend Income:", "Загальний річний дивідендний дохід:") },
//            { "EmptyPort", ("Your portfolio is empty. Add some assets first!", "Ваш портфель порожній. Додайте активи!") },
//            { "SelectToRemove", ("Select an asset to remove", "Оберіть актив для видалення") },
//            { "StockRemoved", ("Asset successfully removed!", "Актив успішно видалено!") },
//            { "CalcMenuTitle", ("COMPOUND INTEREST MAGIC", "МАГІЯ СКЛАДНОГО ВІДСОТКА") },
//            { "EnterMonthly", ("Enter your monthly contribution ($): ", "Скільки ви можете відкладати щомісяця? ($): ") },
//            { "EnterRate", ("Enter expected annual return (%, default 8%): ", "Очікувана річна дохідність (%, зазвичай 8-10%): ") },
//            { "Year", ("Year", "Рік") },
//            { "Invested", ("You Invested", "Ви вклали") },
//            { "TotalValue", ("Portfolio Value", "Капітал") },
//            { "EnterInflation", ("Enter expected inflation (%, default 3%): ", "Очікувана інфляція (%, за замовчуванням 3%): ") },
//            { "NominalValue", ("Nominal Value", "Номінал ($)") },
//            { "RealValue", ("Real Value", "Реальна вартість") },
//            { "InflationNote", ("*Real Value shows your purchasing power in TODAY'S money.", "*Реальна вартість показує купівельну спроможність у СЬОГОДНІШНІХ цінах.") },
//            { "EnterEndDate", ("Enter target date (dd.MM.yyyy, default +10 years): ", "Введіть кінцеву дату (дд.ММ.рррр, за замовчуванням +10 років): ") },
//            { "InvalidDate", ("Invalid format or date in the past. Use dd.MM.yyyy", "Некоректна дата або дата в минулому. Використовуйте дд.ММ.рррр") },
//            { "MenuOpt5", ("Compound Calculator", "Калькулятор капіталу") },
//            { "EduMenuTitle", ("Education hub", "Навчальний центр") },
//            { "CheatSheetOpt", ("Beginner's Cheat Sheet", "Шпаргалка термінів") },
//            { "CalcOpt", ("Compound Interest Calculator", "Калькулятор складного відсотка") },
//            { "TutWelcome", ("Welcome, future investor! Let's start your financial journey.", "Вітаємо, майбутній інвесторе! Почнемо вашу фінансову подорож.") },
//            { "TutStep1", ("Investing is like planting a tree. The earlier you start, the bigger it grows.", "Інвестування — як посадка дерева. Чим раніше почнеш, тим більшим воно виросте.") },
//            { "TutReady", ("Awesome! You are ready. Let's go to the Main Menu.", "Чудово! Ви готові. Переходимо до Головного Меню.") },
//            { "PressEnter", ("Press ENTER to continue...", "Натисніть ENTER для продовження...") },
//            { "ClearPortfolio", ("Clear Entire Portfolio", "Очистити весь портфель") },
//            { "PortfolioCleared", ("Portfolio has been completely cleared!", "Портфель повністю очищено!") },
//            { "MenuOptGoalCalc", ("Goal Milestone Calculator", "Калькулятор фінансової цілі") },
//            { "ClearConfirm", ("Are you sure? Type YES to confirm: ", "Ви впевнені? Введіть ТАК для підтвердження: ") },
//            { "ClearCanceled", ("Action canceled. Your portfolio remains completely safe.", "Дію скасовано. Твій портфель залишається в повній безпеці.") },
//            { "SecTech", ("Technology", "Технології") },
//            { "SecFinance", ("Financials", "Фінанси") },
//            { "SecHealth", ("Healthcare", "Охорона здоров'я") },
//            { "SecStaples", ("Consumer Staples", "Товари першої необхідності") },
//            { "SecDiscretionary", ("Consumer Discretionary", "Споживчі товари") },
//            { "SecEnergy", ("Energy", "Енергетика") },
//            { "SecUtilities", ("Utilities", "Комунальні послуги") },
//            { "SecRealEstate", ("Real Estate", "Нерухомість") },
//            { "SecIndustrials", ("Industrials", "Промисловість") },
//            { "SecMaterials", ("Materials", "Матеріали") },
//            { "CancelHint", ("Press Enter to cancel", "Натисніть Enter для відміни") },
//            { "Cancel", ("Cancel", "Відміна") },

//            { "CompoundCalc", ("Compound Interest Calculator", "Калькулятор складного відсотка") },
//            { "CalcInitial", ("Initial Investment", "Початковий капітал") },
//            { "CalcMonthly", ("Monthly Contribution", "Щомісячне поповнення") },
//            { "CalcRate", ("Estimated Annual Yield (%)", "Очікувана річна дохідність (%)") },
//            { "CalcYears", ("Investment Period (Years)", "Термін інвестування (років)") },

//            // Головне меню калькулятора цілей
//            { "GoalMenuHeader", ("  🎯 FINANCIAL GOAL MANAGER", "  🎯 МЕНЕДЖЕР ФІНАНСОВИХ ЦІЛЕЙ") },
//            { "GoalMenuOpt1", ("1. Calculate and save a new goal", "1. Розрахувати та зберегти нову ціль") },
//            { "GoalMenuOpt2", ("2. View my saved goals", "2. Переглянути мої цілі") },
//            { "GoalMenuOpt3", ("3. Delete a financial goal", "3. Видалити фінансову ціль") },
//            { "GoalMenuOpt4", ("4. Back to Main Menu", "4. Повернутися в головне меню") },

//            // Екран "Як це працює"
//            { "GoalHowItWorks", ("              HOW THE GOAL PLANNING WORKS                  ", "         ЯК ПРАЦЮЄ АЛГОРИТМ ПЛАНУВАННЯ ЦІЛІ             ") },
//            { "GoalStep1Title", ("📌 Step 1. Find the 'real' cost through the years (with inflation)", "📌 Крок 1. Дізнайся 'реальну' ціну через роки (з інфляцією)") },
//            { "GoalStep1Desc", ("We use a standard 2.14% annual inflation rate for the USD.", "Гроші знецінюються, тому твоя ціль через роки коштуватиме дорожче.\nЯк орієнтир для долара береться інфляція 2.14% річних.") },
//            { "GoalStep2Title", ("📌 Step 2. Calculate your monthly investment target", "📌 Крок 2. Рахуємо, скільки треба відкладати щомісяця") },
//            { "GoalStep2Desc", ("We divide the future cost by the compound interest factor.", "Ми беремо майбутню ціну і ділимо її на коефіцієнт складного відсотка.") },
//            { "GoalPressAnyCalc", ("Press any key to start calculating your own goal...", "Натисни будь-яку клавішу, щоб перейти до розрахунку...") },

//            // Запит даних
//            { "GoalNewPlanHeader", ("                     NEW GOAL PLAN                       ", "                НОВИЙ ФІНАНСОВИЙ ПЛАН ЦІЛІ               ") },
//            { "GoalNamePrompt", (" Name your goal (e.g. Apartment)", " Назви свою ціль (напр. Квартира)") },
//            { "GoalPricePrompt", ("1. Current cost of your goal ($)", "1. Ціна твоєї цілі сьогодні ($)") },
//            { "GoalYearsPrompt", ("2. Target horizon (years)", "2. Через скільки років плануєш купівлю") },
//            { "GoalRatePrompt", ("3. Expected annual return (%)", "3. Очікувана річна дохідність інвестицій (%)") },

//            // Екран результатів
//            { "GoalCalcHeader", ("                   PLAN CALCULATIONS                     ", "                   РОЗРАХУНОК ПЛАНУ                      ") },
//            { "GoalTarget", ("🎯 Goal:", "🎯 Ціль:") },
//            { "GoalCostToday", ("• Cost today:", "• Вартість сьогодні:") },
//            { "GoalCostFuture", ("• Cost in {0} years (2.14% inflation):", "• Вартість через {0} років (з інфляцією 2.14%):") },
//            { "GoalReqInv", ("➜ REQUIRED MONTHLY INVESTMENT:", "➜ НЕОБХІДНО ІНВЕСТУВАТИ ЩОМІСЯЦЯ:") },
//            { "GoalSavePrompt", ("\n💾 Do you want to save this goal to your profile? (YES/NO): ", "\n💾 Бажаєш зберегти цю ціль у свій профіль? (ТАК/НІ): ") },
//            { "GoalSavedSuccess", ("\n✔ Goal saved successfully!", "\n✔ Ціль успішно збережено!") },
//            { "GoalSavedCancel", ("\nCalculation finished without saving.", "\nРозрахунок завершено без збереження.") },

//            // Перегляд цілей (Таблиця)
//            { "GoalListHeader", ("                                     MY SAVED FINANCIAL GOALS                            ", "                                 СПИСОК МОЇХ ФІНАНСОВИХ ЦІЛЕЙ                            ") },
//            { "GoalListEmpty", ("You have no saved goals yet. Calculate one in option 1!", "У тебе поки немає збережених цілей. Прорахуй щось у пункті 1!") },
//            { "TblGoalName", ("Goal Name", "Назва цілі") },
//            { "TblPriceToday", ("Price Today", "Ціна зараз") },
//            { "TblYears", ("Years", "Років") },
//            { "TblReturn", ("Return", "Дохідн.") },
//            { "TblFuturePrice", ("Future Price", "Ціна майбут.") },
//            { "TblMonthlyInv", ("Monthly Inv", "Внесок/міс") },

//            // Видалення цілей
//            { "GoalDelEmpty", ("\nYou have no saved goals to delete.", "\nУ тебе немає збережених цілей для видалення.") },
//            { "GoalYearsWord", ("years", "років") },
//            { "GoalDelCancel", ("[ Cancel Action ]", "[ Скасувати дію ]") },
//            { "GoalDelPrompt", ("Select a financial goal to delete:", "Оберіть фінансову ціль, яку бажаєте видалити:") },
//            { "GoalDelSuccess", ("\n✔ Goal \"{0}\" has been successfully removed!", "\n✔ Ціль «{0}» успішно видалено з твого профілю!") },

//            { "EnterTicker", ("Enter stock ticker", "Введіть тікер акції") },


//        };

//        public static string Get(string key)
//        {
//            if (!_phrases.ContainsKey(key)) return key;
//            return _currentLanguage == "uk" ? _phrases[key].Uk : _phrases[key].En;
//        }
//    }
//}

using Microsoft.VisualBasic;
using System.Globalization;
using System.Threading;
using SmartDividendTracker.Resources;

namespace SmartDividendTracker.Services
{
    public static class LocalizationManager
    {
        private static string _currentLanguage = "en";

        public static void SetLanguage(string lang)
        {
            _currentLanguage = lang.ToLower() == "uk" ? "uk" : "en";

            var culture = new CultureInfo(_currentLanguage);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public static string GetCurrentLanguage() => _currentLanguage;

        // ПОВЕРТАЄМО МЕТОД GET, щоб старий код працював!
        // Тепер він бере дані з файлів Strings.resx
        public static string Get(string key)
        {
            // ResourceManager автоматично підтягує англійську або українську залежно від налаштувань
            string translation = Resources.Strings.ResourceManager.GetString(key);

            // Якщо раптом ключ не знайдено, повертаємо сам ключ, щоб програма не впала
            return translation ?? key;
        }
    }
}