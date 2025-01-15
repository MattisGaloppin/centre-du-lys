using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Serveur.Model.Data
{
    /// <summary>
    ///enregistre les parametres de la bd dans le program et l'appsetting
    /// </summary>
    public class DatabaseSettings
    {
        private string defaultConnection;

        /// <summary>
        /// String de connexion à la bdd, initialisé dans Program.cs
        /// </summary>
        public string DefaultConnection { get => defaultConnection; set => defaultConnection = value; }
    }
}
