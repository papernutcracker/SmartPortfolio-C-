using System;
using System.Collections.Generic;

namespace SmartDividendTracker.Services
{
    public static class ConsoleHelper
    {
        // 🔥 ФІКС: Класову статичну змінну _selectedIndex ПОВНІСТЮ ВИДАЛЕНО,
        // щоб виключити будь-яке перетікання індексів між різними меню!

        // Головний метод інтерактивного меню з локальним станом
        public static int SelectOption(string prompt, List<string> options, int defaultIndex = 0)
        {
            // Створюємо локальну змінну на стеку — вона живе лише поки працює це конкретне меню
            int selectedIndex = defaultIndex;

            // Захист від некоректного дефолтного індексу
            if (selectedIndex >= options.Count || selectedIndex < 0)
            {
                selectedIndex = 0;
            }

            ConsoleKey keyPressed;
            do
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=========================================================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(prompt);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=========================================================");
                Console.ResetColor();

                for (int i = 0; i < options.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($" > {options[i]} ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"   {options[i]} ");
                    }
                    Console.ResetColor();
                }

                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0) selectedIndex = options.Count - 1;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= options.Count) selectedIndex = 0;
                }

            } while (keyPressed != ConsoleKey.Enter);

            Console.ResetColor();
            return selectedIndex;
        }

        public static List<int> SelectMultipleOptions(string prompt, List<string> options)
        {
            var selectedIndices = new List<int>();
            int currentHover = 0;
            ConsoleKey keyPressed;

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=========================================================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(prompt);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=========================================================");
                Console.ResetColor();

                for (int i = 0; i < options.Count; i++)
                {
                    string checkbox = selectedIndices.Contains(i) ? "[X]" : "[ ]";

                    if (i == currentHover)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($" > {checkbox} {options[i]} ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"   {checkbox} {options[i]} ");
                    }
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n [Space] - Обрати/Зняти | [Enter] - Підтвердити вибір");
                Console.ResetColor();

                while (Console.KeyAvailable) Console.ReadKey(true); 

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    currentHover--;
                    if (currentHover < 0) currentHover = options.Count - 1;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    currentHover++;
                    if (currentHover >= options.Count) currentHover = 0;
                }
                else if (keyPressed == ConsoleKey.Spacebar)
                {
                    if (selectedIndices.Contains(currentHover))
                        selectedIndices.Remove(currentHover);
                    else
                        selectedIndices.Add(currentHover);
                }

            } while (keyPressed != ConsoleKey.Enter);

            return selectedIndices;
        }

        public static void ShowSpinner(string message, int iterations = 15)
        {
            Console.CursorVisible = false;
            string[] spinner = { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };

            Console.Write($"   {message}  ");

            for (int i = 0; i < iterations; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(spinner[i % spinner.Length]);
                System.Threading.Thread.Sleep(80);
                Console.Write("\b");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✔");
            Console.ResetColor();
            Console.CursorVisible = true;
            System.Threading.Thread.Sleep(400);
        }

        public static void ShowExitAnimation(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            foreach (char c in message)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(30);
            }

            Console.WriteLine("\n");
            Console.ResetColor();
            System.Threading.Thread.Sleep(500);
        }
    }
}