using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        [HttpGet]
        public IActionResult GetUsers()
        {
            IActionResult result = BadRequest(new { Message = "Impossible de récupérer les utilisateurs" });

            try
            {
                // Récupère tous les utilisateurs via le manager
                var users = userManager.GetAll();

                result = Ok(new { Users = users });
                logger.LogInformation("Liste des utilisateurs récupérée avec succès.");

            }
            catch (Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });
                logger.LogError("Erreur lors de la récupération des utilisateurs : " + ex.Message);
            }

            return result;
        }


    }
}
