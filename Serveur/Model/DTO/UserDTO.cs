namespace Serveur.Model.DTO
{
    /// <summary>
    /// Represente la table user de la BD
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Permet d'obtenir un id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Permet d'obtenir ou de définir une Email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Permet d'obtenir ou de définir un pwd
        /// </summary>
        public string HashPassword { get; set; }
    }
}
