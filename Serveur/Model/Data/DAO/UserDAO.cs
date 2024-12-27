using System.Collections.Generic;
using System.Data;
using Serveur.Model.DTO;
using SQLitePCL;

namespace Serveur.Model.Data
{
    /// <summary>
    /// Classe de gestion des opérations sur la table 'users' dans la base de données.
    /// </summary>
    public class UserDAO
    {
        private readonly IDatabase database;

        /// <summary>
        /// Constructeur de la classe UserDAO.
        /// </summary>
        /// <param name="database">Instance de l'interface IDatabase pour interagir avec la base de données.</param>
        public UserDAO(IDatabase database)
        {
            this.database = database;
            Batteries_V2.Init();
        }

        /// <summary>
        /// Récupère les informations de tous les utilisateurs dans la table 'users'.
        /// </summary>
        /// <returns>Une liste d'objets UserDTO représentant les utilisateurs.</returns>
        public List<UserDTO> GetAllUsers()
        {
            const string query = "SELECT Id, Email, HashPassword FROM Users";

            //connexion a la bd
            database.Connect();

            //execution de la requete
            DataTable result = database.ExecuteQuery(query);

            // Convertir le DataTable en une liste d'objets UserDTO.
            var users = new List<UserDTO>();

            foreach (DataRow row in result.Rows)
            {
                var user = new UserDTO
                {
                    Email = row.Field<string>("Email"),
                    HashPassword = row.Field<string>("HashPassword")
                };

                users.Add(user);
            }
            database.Disconnect();
            return users;
        }
    }
}
