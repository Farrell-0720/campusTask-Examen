namespace CampusTask.Models
{
    /// <summary>
    /// Statut d'une tâche académique.
    /// Note : le statut "En retard" n'est pas stocké, il est calculé dynamiquement.
    /// </summary>
    public enum TaskStatus
    {
        Todo = 0,
        Completed = 1
    }
}
