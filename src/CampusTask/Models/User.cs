using SQLite;

namespace CampusTask.Models
{
    /// <summary>
    /// Représente un utilisateur local de l'application.
    /// </summary>
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(120)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(180), Unique]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash BCrypt du mot de passe (contient déjà le sel).
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
