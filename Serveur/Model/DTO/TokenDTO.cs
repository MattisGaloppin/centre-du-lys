namespace Serveur.Model.DTO
{
    /// <summary>
    /// Représente la table Tokens de la bd
    /// </summary>
    public class TokenDTO
    {
        /// <summary>
        /// Permet d'obtenir l'id du token
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Permet d'obtenir ou de définir l'id du user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Permet d'obtenir ou de définir un pwd
        /// </summary>
        public string Token  { get; set; }
    }
}
}
