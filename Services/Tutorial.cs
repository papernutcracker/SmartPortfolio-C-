using System;
using System.Threading;

namespace SmartDividendTracker.Services
{
    public static class TutorialService
    {
        public static void RunTutorial()
        {
            Console.Clear();
            TypewriterEffect(LocalizationManager.Get("TutWelcome"));
            WaitForEnter();

            Console.Clear();
            TypewriterEffect(LocalizationManager.Get("TutStep1"));
            WaitForEnter();

            Console.Clear();
            TypewriterEffect(LocalizationManager.Get("TutStep2"));
            WaitForEnter();

            // Викликаємо нашу шпаргалку як частину обов'язкового навчання!
            CheatSheetService.Show();

            Console.Clear();
            TypewriterEffect(LocalizationManager.Get("TutReady"));
            WaitForEnter();
        }

        // Імітація друку тексту (як у відеоіграх)
        private static void TypewriterEffect(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n");
            Console.Write("    "); // Відступ від краю

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(40); // Швидкість друкування
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        // Чекаємо, поки користувач прочитає і натисне Enter
        private static void WaitForEnter()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n\n    [{LocalizationManager.Get("PressEnter")}]");
            Console.ResetColor();

            // Очищуємо буфер клавіатури, щоб уникнути випадкових проклікувань
            while (Console.KeyAvailable) Console.ReadKey(true);

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }
    }
}