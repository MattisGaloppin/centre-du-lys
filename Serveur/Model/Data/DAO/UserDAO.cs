using System.Collections.Generic;
using System.Data;
using Serveur.Model.DTO;

namespace Serveur.Model.Data
{
    /// <summary>
    /// Classe de gestion des opérations sur la table 'users' dans la base de données.
    /// </summary>
    public class UserDAO
    {
        private readonly IDatabase _database;

        /// <summary>
        /// Constructeur de la classe UserDAO.
        /// </summary>
        /// <param name="database">Instance de l'interface IDatabase pour interagir avec la base de données.</param>
        public UserDAO(IDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Récupère les informations de tous les utilisateurs dans la table 'users'.
        /// </summary>
        /// <returns>Une liste d'objets UserDTO représentant les utilisateurs.</returns>
        public List<UserDTO> GetAllUsers()
        {
            const string query = "SELECT id, email, password FROM users";
            DataTable result = _database.ExecuteQuery(query);

            // Convertir le DataTable en une liste d'objets UserDTO.
            var users = new List<UserDTO>();

            foreach (DataRow row in result.Rows)
            {
                var user = new UserDTO
                {
                    Id = row.Field<int>("id"),
                    Email = row.Field<string>("email"),
                    HashPassword = row.Field<string>("password")
                };

                users.Add(user);
            }

            return users;
        }
    }
}
