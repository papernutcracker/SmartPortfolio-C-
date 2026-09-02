namespace SmartDividendTracker.Models
{
    public class CustomGoal
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Ціль";
        public decimal CurrentPrice { get; set; }
        public int Years { get; set; }
        public decimal AnnualReturn { get; set; }
        public double FuturePrice { get; set; }
        public double MonthlyContribution { get; set; }

        // НОВЕ ПОЛЕ: Дата останнього оновлення
        public System.DateTime DateUpdated { get; set; } = System.DateTime.Now;

        public double ProgressPercentage
        {
            get
            {
                if (FuturePrice == 0) return 0;

                // Просто рахуємо реальний відсоток без обмеження в 100%
                return (double)CurrentPrice / FuturePrice * 100.0;
            }
        }
    }
}