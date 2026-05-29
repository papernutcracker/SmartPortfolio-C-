using System;
using System.Collections.Generic;

namespace SmartDividendTracker.Services
{
    public static class CheatSheetService
    {
        public static void Show()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', 110));

            // Динамічний заголовок
            bool isUk = LocalizationManager.GetCurrentLanguage() == "uk";
            string title = isUk ? "ШПАРГАЛКА НОВАЧКА" : "BEGINNER'S CHEAT SHEET";

            // Центруємо заголовок
            int spaces = (110 - title.Length) / 2;
            Console.WriteLine(new string(' ', spaces) + title);
            Console.WriteLine(new string('=', 110) + "\n");
            Console.ResetColor();

            // Словник (Англ. Термін, Англ. Опис, Укр. Термін, Укр. Опис)
            // Усі описи тепер суворо до 80 символів, щоб таблиця не ламалася!
            var terms = new List<(string EnTerm, string EnDef, string UkTerm, string UkDef)>
            {
                ("Stock", "A share of ownership (company grows - your share costs more).",
                 "Акція", "Частка в бізнесі (компанія розвивається — ваша частка дорожчає)."),

                ("Bond", "You lend money to a government or business at a fixed interest rate.",
                 "Облігація", "Ви позичаєте гроші державі чи бізнесу під заздалегідь відомий відсоток."),

                ("ETF (Fund)", "A 'basket' of many different companies you can buy as one single share.",
                 "ETF (фонд)", "\"Кошик\" із різних компаній, який можна купити як одну акцію."),

                ("Index", "The average temperature of the market (shows where a group of companies moves).",
                 "Індекс", "Середня температура ринку (показує куди рухається група компаній)."),

                ("Dividends", "A portion of the company's profit that it regularly pays into your account.",
                 "Дивіденди", "Частина прибутку компанії, яку вона регулярно виплачує вам на рахунок."),

                ("Compound Interest", "When interest begins to accrue on already earned interest (snowball effect).",
                 "Складний відсоток", "Коли відсотки починають нараховуватися на зароблені відсотки (снігова куля)."),

                ("P/E Ratio", "Years needed for a company to fully pay off its price with current profit.",
                 "P/E", "Показник того, за скільки років компанія окупить свою ціну прибутком."),

                ("Diversification", "The rule of 'not putting all eggs in one basket' - distributing money.",
                 "Диверсифікація", "Правило \"не класти всі яйця в один кошик\" — розумний розподіл грошей."),

                ("Volatility", "'Roller coaster' - how strongly and often the asset price jumps.",
                 "Волатильність", "\"Американські гірки\" — наскільки сильно і часто стрибає ціна активу."),

                ("Liquidity", "How quickly an asset can be turned into cash without losing its value.",
                 "Ліквідність", "Як швидко актив можна перетворити на готівку без втрати вартості."),

                ("Broker", "Your licensed intermediary through whose app you buy assets.",
                 "Брокер", "Ваш ліцензований посередник, через додаток якого ви купуєте активи.")
            };

            // Малюємо шапку таблиці
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            string termHeader = isUk ? "Термін" : "Term";
            string defHeader = isUk ? "Простими словами" : "In Simple Terms";

            Console.WriteLine($"| {termHeader,-22} | {defHeader,-81} |");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('-', 110));
            Console.ResetColor();

            // Виводимо всі терміни
            foreach (var item in terms)
            {
                string term = isUk ? item.UkTerm : item.EnTerm;
                string def = isUk ? item.UkDef : item.EnDef;

                Console.Write("| ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{term,-22}");
                Console.ResetColor();
                Console.WriteLine($" | {def,-81} |");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string('-', 110));
                Console.ResetColor();
            }

            Console.WriteLine(isUk ? "\nНатисніть будь-яку клавішу для повернення..." : "\nPress any key to return...");
            Console.ReadKey(true);
        }
    }
}