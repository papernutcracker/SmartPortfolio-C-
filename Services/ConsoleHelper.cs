using System;
using System.Collections.Generic;

namespace SmartDividendTracker.Services
{
    public static class ConsoleHelper
    {

        // МЕНЮ ОДИНАРНОГО ВИБОРУ (З кольорами)
        // МЕНЮ ОДИНАРНОГО ВИБОРУ
        public static int SelectOption(string prompt, List<string> options, int defaultIndex = 0)
        {
            int selectedIndex = defaultIndex; // Тепер курсор починає не з нуля, а з пам'яті
            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=======================================");
                Console.WriteLine(prompt);
                Console.WriteLine("=======================================\n");
                Console.ResetColor();

                for (int i = 0; i < options.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($" > {options[i]} ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"   {options[i]} ");
                        Console.ResetColor();
                    }
                }

                // ВИПРАВЛЕННЯ: Очищуємо буфер від випадкових "подвійних" натискань
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                // Тепер безпечно читаємо клавішу
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0) selectedIndex = options.Count - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= options.Count) selectedIndex = 0;
                }
                else if (key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    Console.Clear();
                    System.Threading.Thread.Sleep(100); // Додаткова мікропауза для стабільності
                    return selectedIndex;
                }
            }
        }

        // МЕНЮ МУЛЬТИ-ВИБОРУ
        public static List<int> SelectMultipleOptions(string prompt, List<string> options)
        {
            int selectedIndex = 0;
            var checkedIndexes = new HashSet<int>();
            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=======================================");
                Console.WriteLine(prompt);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(LocalizationManager.GetCurrentLanguage() == "uk"
                    ? "(Стрілки - навігація, ПРОБІЛ - обрати, ENTER - підтвердити)"
                    : "(Arrows to move, SPACE to check/uncheck, ENTER to confirm)");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=======================================\n");
                Console.ResetColor();

                for (int i = 0; i < options.Count; i++)
                {
                    bool isChecked = checkedIndexes.Contains(i);
                    string checkboxText = isChecked ? "[X]" : "[ ]";

                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($" > {checkboxText} ");
                        Console.WriteLine($"{options[i]} ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = isChecked ? ConsoleColor.Green : ConsoleColor.Gray;
                        Console.Write($"   {checkboxText} ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"{options[i]} ");
                        Console.ResetColor();
                    }
                }

                // ВИПРАВЛЕННЯ: Очищуємо буфер тут також
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0) selectedIndex = options.Count - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= options.Count) selectedIndex = 0;
                }
                else if (key == ConsoleKey.Spacebar)
                {
                    if (checkedIndexes.Contains(selectedIndex))
                        checkedIndexes.Remove(selectedIndex);
                    else
                        checkedIndexes.Add(selectedIndex);
                }
                else if (key == ConsoleKey.Enter)
                {
                    if (checkedIndexes.Count == 0) continue;

                    Console.CursorVisible = true;
                    Console.Clear();
                    System.Threading.Thread.Sleep(100); // Мікропауза
                    return new List<int>(checkedIndexes);
                }
            }
        }

        // НОВИЙ МЕТОД: Анімація виходу
        public static void ShowExitAnimation(string message)
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Cyan; // Робимо текст красивого кольору
            Console.WriteLine("\n\n");
            Console.Write("     ");

            // Ефект друкарської машинки (по одному символу)
            foreach (char c in message)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(40); // Затримка 40 мілісекунд між літерами
            }

            // Анімація крапок (ніби йде завантаження)
            for (int i = 0; i < 3; i++)
            {
                Console.Write(" .");
                System.Threading.Thread.Sleep(500); // Затримка півсекунди
            }

            Console.ResetColor();
            Console.WriteLine("\n\n");
            System.Threading.Thread.Sleep(500); // Фінальна пауза перед закриттям вікна
        }
    }
}