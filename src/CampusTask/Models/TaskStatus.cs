namespace CampusTask.Models
{
    /// <summary>
    /// Statut d'une tâche académique.
    /// Le statut "En retard" n'est pas stocké : il est calculé dynamiquement.
    /// </summary>
    public enum AcademicTaskStatus
    {
        Todo = 0,
        Completed = 1
    }
}
