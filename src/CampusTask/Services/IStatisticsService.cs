namespace CampusTask.Services
{
    public class StatisticsResult
    {
        public int Total { get; set; }
        public int Todo { get; set; }
        public int Completed { get; set; }
        public int Overdue { get; set; }
        public double CompletionPercentage { get; set; }
        public int LowPriority { get; set; }
        public int MediumPriority { get; set; }
        public int HighPriority { get; set; }
    }

    public interface IStatisticsService
    {
        Task<StatisticsResult> GetStatisticsAsync();
    }
}
