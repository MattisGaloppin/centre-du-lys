using Microsoft.Data.Sqlite;
using Serveur.Model.Data;
using System.Data;

/// <summary>
/// Service pour interagir avec une base de données SQLite.
/// Implémente l'interface IDatabase pour exécuter des requêtes et des commandes SQL.
/// </summary>
public class SQLiteService : IDatabase
{
    private readonly string _connectionString;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="SQLiteService"/>.
    /// </summary>
    /// <param name="connectionString">La chaîne de connexion à la base de données SQLite.</param>
    public SQLiteService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DataTable ExecuteQuery(string query, SqliteParameter[] parameters = null)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var reader = command.ExecuteReader())
                {
                    DataTable result = new DataTable();
                    result.Load(reader); // Charge les données dans le DataTable
                    return result;
                }
            }
        }
    }


    public int ExecuteNonQuery(string commandText, SqliteParameter[] parameters = null)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(commandText, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                return command.ExecuteNonQuery(); // Retourne le nombre de lignes affectées
            }
        }
    }
}
