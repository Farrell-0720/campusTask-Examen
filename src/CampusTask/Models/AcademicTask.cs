using SQLite;

namespace CampusTask.Models
{
    [Table("AcademicTasks")]
    public class AcademicTask
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int UserId { get; set; }

        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [MaxLength(120)]
        public string Subject { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime DueDate { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public AcademicTaskStatus Status { get; set; } = AcademicTaskStatus.Todo;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        [Ignore]
        public bool IsOverdue
        {
            get
            {
                if (Status == AcademicTaskStatus.Completed)
                    return false;
                return DateTime.Today > DueDate.Date;
            }
        }
    }
}
