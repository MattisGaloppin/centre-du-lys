using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Serveur.Model.DTO;
using Serveur.Model.Exceptions;
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
        /// Récupère les informations d' un user dans la table 'users'.
        /// </summary>
        /// <returns>Une liste d'objets UserDTO représentant les utilisateurs.</returns>
        public UserDTO GetUserByEmail(string email)
        {
            const string query = "SELECT Id, Email, HashPassword FROM Users WHERE Email = @Email";

            //connexion a la bd
            database.Connect();

            var parameters = new SqliteParameter[]
            {
                new SqliteParameter("@Email", email),
            };

            //execution de la requete
            var result =  database.ExecuteQuery(query, parameters);

            // Convertir le DataTable en une liste d'objets UserDTO.
            var userFound = new UserDTO
            {
                Email = result.Rows[0].Field<string>("Email"),
                HashPassword = result.Rows[0].Field<string>("HashPassword")
            };

            database.Disconnect();
            return userFound;
        }
        /// <summary>
        /// Insere un nouvel utilisateur en base
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Register(UserDTO user)
        {
            bool res = false;
            database.Connect();

            // insere des users
            string query = "INSERT INTO Users (Email, HashPassword) VALUES (@Email, @HashPassword)";

            var parameters = new SqliteParameter[]
            {
                new SqliteParameter("@Email", user.Email),
                new SqliteParameter("@HashPassword", user.HashPassword)
            };

            // Exécution de la requête
            database.ExecuteNonQuery(query, parameters);
            res = true;

            database.Disconnect();
            return res;
        }
        /// <summary>
        /// Vérifie si une adresse email existe déjà dans la base de données.
        /// </summary>
        /// <param name="email">L'adresse email à vérifier.</param>
        /// <returns>Un booléen indiquant si l'email existe ou non.</returns>
        public bool EmailExists(string email)
        {
            bool res = false;

            const string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            var parameter = new SqliteParameter[]
            {
                new SqliteParameter("Email", email)
            };

            database.Connect();
            var result = database.ExecuteQuery(query, parameter);

            // Retourne vrai si au moins une correspondance est trouvée
            if (result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0][0]) > 0)
            {
                res = true;
            }
            database.Disconnect();

            return res; 
        }
        /// <summary>
        /// Connecte un user au site a partir d'un login et d'un pwd
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns>Le token généré de l'utilisateur</returns>
        public UserDTO Login(string email) 
        {
            var user = new UserDTO();
            try
            {
                user = this.GetUserByEmail(email);
            }
            catch
            {
                throw new UserDoesNotExistsException();
            }
            return user;
        }


    }
}
