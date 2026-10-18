using CampusTask.Models;
using SQLite;

namespace CampusTask.Data
{
    /// <summary>
    /// Service central d'accès à la base SQLite locale.
    /// Initialise la base au premier lancement et fournit la connexion.
    /// </summary>
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _connection;
        private readonly string _dbPath;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public DatabaseService()
        {
            // Emplacement privé de l'application (recommandé pour MAUI)
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "campustask.db3");
        }

        /// <summary>
        /// Retourne la connexion asynchrone, en l'initialisant si nécessaire.
        /// </summary>
        public async Task<SQLiteAsyncConnection> GetConnectionAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_connection != null)
                    return _connection;

                _connection = new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

                // Active les clés étrangères
                await _connection.ExecuteAsync("PRAGMA foreign_keys = ON;");

                // Crée les tables si elles n'existent pas
                await _connection.CreateTableAsync<User>();
                await _connection.CreateTableAsync<AcademicTask>();

                return _connection;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Chemin complet du fichier de base de données (utile pour diagnostic).
        /// </summary>
        public string DatabasePath => _dbPath;
    }
}
