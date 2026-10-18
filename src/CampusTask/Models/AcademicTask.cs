using SQLite;

namespace CampusTask.Models
{
    /// <summary>
    /// Représente une tâche académique appartenant à un utilisateur.
    /// </summary>
    [Table("AcademicTasks")]
    public class AcademicTask
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Identifiant du propriétaire (clé étrangère logique vers Users.Id).
        /// </summary>
        [Indexed]
        public int UserId { get; set; }

        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [MaxLength(120)]
        public string Subject { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date limite (traitée comme date calendaire, sans heure).
        /// </summary>
        public DateTime DueDate { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Indique si la tâche est en retard.
        /// Calculé dynamiquement : non terminée et date limite dépassée (jour suivant).
        /// </summary>
        [Ignore]
        public bool IsOverdue
        {
            get
            {
                if (Status == TaskStatus.Completed)
                    return false;

                // Règle : en retard à partir du jour suivant la date limite
                return DateTime.Today > DueDate.Date;
            }
        }
    }
}
