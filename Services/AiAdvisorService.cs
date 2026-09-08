using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SmartDividendTracker.Data;
using SmartDividendTracker.Models;

namespace SmartDividendTracker.Services
{
    public class AiAdvisorService
    {
        private readonly AppDbContext _context;
        private static readonly HttpClient _httpClient = new HttpClient();

        public AiAdvisorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetPortfolioAdviceAsync(UserProfile profile)
        {
            // 1. Отримуємо активи користувача з бази даних
            var stocks = _context.Stocks.Where(s => s.UserProfileId == profile.Id).ToList();
            decimal totalValue = stocks.Sum(s => s.TotalValue);

            // 2. Формуємо контекст для ШІ на основі даних профілю та портфеля
            var sb = new StringBuilder();
            sb.AppendLine($"Ти фінансовий радник. Проаналізуй інвестиційний портфель користувача.");
            sb.AppendLine($"Горизонт інвестування: {profile.Horizon} (переважно від 3 років).");
            sb.AppendLine($"Головна мета: {string.Join(", ", profile.Goals)}.");
            sb.AppendLine($"Загальна вартість портфеля: ${totalValue:F2}.");
            sb.AppendLine("Активи у портфелі:");

            if (stocks.Count == 0)
            {
                sb.AppendLine("Портфель наразі порожній.");
            }
            else
            {
                foreach (var stock in stocks)
                {
                    sb.AppendLine($"- Тікер: {stock.Ticker}, Сектор: {stock.Sector}, Сума: ${stock.TotalValue:F2}, Дивіденди: {stock.DividendYield}%, P/E: {stock.PeRatio}");
                }
            }

            sb.AppendLine("\nДай стислу, професійну пораду українською мовою: чи збалансований портфель для довгострокового інвестування (3+ роки), чи є ризики концентрації в одному секторі, і що варто покращити.");

            string prompt = sb.ToString();

            // 3. Відправляємо запит до локальної Ollama
            return await CallOllamaAsync(prompt);
        }

        private async Task<string> CallOllamaAsync(string prompt)
        {
            var url = "http://localhost:11434/api/generate";

            var requestData = new
            {
                model = "llama3", // модель, яку ти завантажила через термінал
                prompt = prompt,
                stream = false
            };

            var jsonContent = JsonSerializer.Serialize(requestData);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                // Встановлюємо таймаут (локальні моделі можуть думати кілька секунд)
                _httpClient.Timeout = TimeSpan.FromSeconds(60);

                var response = await _httpClient.PostAsync(url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    // Парсимо JSON-відповідь від Ollama
                    using var doc = JsonDocument.Parse(responseString);
                    if (doc.RootElement.TryGetProperty("response", out var aiResponse))
                    {
                        return aiResponse.GetString() ?? "Отримано порожню відповідь від ШІ.";
                    }
                }
                return "Помилка при зверненні до локальної нейромережі.";
            }
            catch (Exception ex)
            {
                return $"Не вдалося підключитися до Ollama. Переконайся, що додаток Ollama запущений на ПК.\nТехнічна помилка: {ex.Message}";
            }
        }
    }
}