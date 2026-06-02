using System;

namespace SmartDividendTracker.Services
{
    public static class CheatSheetService
    {
        public static void Show(bool isUa)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=========================================================");
            Console.WriteLine(isUa ? "                 ШПАРГАЛКА ДЛЯ НОВАЧКА                   " : "                BEGINNER'S CHEAT SHEET                   ");
            Console.WriteLine("=========================================================\n");
            Console.ResetColor();

            if (isUa)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Дивіденди (Dividends)");
                Console.ResetColor();
                Console.WriteLine(" — Частина прибутку компанії, яка виплачується акціонерам.");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nЕкс-дивідендна дата (Ex-Dividend Date)");
                Console.ResetColor();
                Console.WriteLine(" — День, до якого потрібно купити акцію, щоб гарантовано отримати найближчу виплату.");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nДивідендна дохідність (Dividend Yield)");
                Console.ResetColor();
                Console.WriteLine(" — Відсоток річного доходу відносно поточної ціни акції.");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nКоефіцієнт виплат (Payout Ratio)");
                Console.ResetColor();
                Console.WriteLine(" — Відсоток чистого прибутку, який компанія віддає на дивіденди (безпечно: до 60-70%).");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nP/E (Price-to-Earnings)");
                Console.ResetColor();
                Console.WriteLine(" — Співвідношення ціни акції до прибутку на одну акцію. Допомагає зрозуміти, чи компанія переоцінена.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Dividends");
                Console.ResetColor();
                Console.WriteLine(" — A portion of a company's earnings paid out to its shareholders.");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nEx-Dividend Date");
                Console.ResetColor();
                Console.WriteLine(" — The cutoff day to buy a stock and still receive the upcoming dividend payout.");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nDividend Yield");
                Console.ResetColor();
                Console.WriteLine(" — The annual dividend income expressed as a percentage of the current stock price.");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nPayout Ratio");
                Console.ResetColor();
                Console.WriteLine(" — The percentage of net income paid out as dividends (safe range: under 60-70%).");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nP/E Ratio (Price-to-Earnings)");
                Console.ResetColor();
                Console.WriteLine(" — Measures a company's current share price relative to its per-share earnings.");
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(isUa ? "\nНатисніть будь-яку клавішу для повернення..." : "\nPress any key to return...");
            Console.ReadKey(true);
            Console.ResetColor();
        }
    }
}