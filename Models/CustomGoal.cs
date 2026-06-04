namespace SmartDividendTracker.Models
{
    public class CustomGoal
    {
        public string Name { get; set; } = "Ціль";
        public decimal CurrentPrice { get; set; }
        public int Years { get; set; }
        public decimal AnnualReturn { get; set; }
        public double FuturePrice { get; set; }
        public double MonthlyContribution { get; set; }
    }
}