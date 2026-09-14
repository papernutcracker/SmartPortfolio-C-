using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartDividendTracker.Services
{
    public static class GeminiService
    {
        public static async Task<string> CallGeminiApiAsync(string apiKey, string prompt)
        {
            // Очищаємо ключ від випадкових пробілів
            apiKey = apiKey?.Trim();

            if (string.IsNullOrEmpty(apiKey))
                return "Помилка: API ключ порожній.";

            // Використовуємо надійну версію моделі
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key={apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            string jsonPayload = JsonSerializer.Serialize(requestBody);
            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                // Детальний вивід помилки, якщо ключ невірний або сервер не відповідає
                if (!response.IsSuccessStatusCode)
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    return $"API Error: {response.StatusCode}\nДеталі: {errorDetails}";
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                using (var doc = JsonDocument.Parse(jsonResponse))
                {
                    var root = doc.RootElement;
                    var text = root.GetProperty("candidates")[0]
                                   .GetProperty("content")
                                   .GetProperty("parts")[0]
                                   .GetProperty("text")
                                   .GetString();

                    return text ?? "Не вдалося отримати відповідь від ШІ.";
                }
            }
        }
    }
}