using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serveur.Model;
using Serveur.Model.Data;
using Serveur.Model.DTO;
using Serveur.Model.Managers;


namespace Serveur.Controller
{
    /// <summary>
    /// Controlleur lié aux utilisateurs
    /// </summary>
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private UserManager userManager;
        private readonly ILogger<UserController> logger;
        public UserController(UserManager userManager, ILogger<UserController> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        /// <summary>
        /// Récupère tous les utilisateurs.
        /// </summary>
        /// <returns>Liste des utilisateurs</returns>
        [HttpGet("GetUserByEmail")]
        public IActionResult GetUserByEmail(string Email)
        {
            IActionResult result = BadRequest(new { Message = "Impossible de récupérer l'utilisateur" });

            try
            {
                // Récupère tous les utilisateurs via le manager
                var users = userManager.GetUserByEmail(Email);

                result = Ok(new { Users = users });
                logger.LogInformation("L' utilisateur a été récupéré avec succès.");

            }
            catch (Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });
                logger.LogError("Erreur lors de la récupération des utilisateurs : " + ex.Message);
            }

            return result;
        }
        /// <summary>
        /// Enregistre un nouvel utilisateur
        /// </summary>
        /// <param name="userDTO">l'utilisateur a enregistrer</param>
        /// <returns>L'état de l'inscription</returns>
        [HttpPost("Register")]
        public IActionResult Register(UserDTO userToRegister)
        {
            IActionResult result = BadRequest(new { Message = "Impossible d'insérer cet utilisateur" });

            //hashage du pwd
            userToRegister.HashPassword = SecurePasswordHasher.Hash(userToRegister.HashPassword);
            try
            {
                userManager.Register(userToRegister);
                result = Ok(new { Message = "Inscription réussi" });
            }
            catch (Exception ex) 
            {
                result = BadRequest(new { Message = ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Connecte un utilisateur
        /// </summary>
        /// <param name="userToConnect">l'utilisateur a connecter</param>
        /// <returns>le token de sa connexion</returns>
        [HttpPost("Login")]
        public IActionResult Login(UserDTO userToConnect) 
        {
            IActionResult result = BadRequest(new { Message = "Impossible de se connecter" });

            try
            {
                userManager.Login(userToConnect);
                result = Ok(new { Message = "connexion réussi" });
            }
            catch (Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });
            }
            return result;
        }
    }
}
