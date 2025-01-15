using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Model.Exceptions;
using Serveur.Model.Data;
using Serveur.Model.DTO;
using Serveur.Model.Exceptions;

namespace Serveur.Model.Managers
{
    /// <summary>
    /// gere la logique de gestion des users
    /// </summary>
    public class UserManager
    {
        private UserDAO userDAO;

        /// <summary>
        /// contructor de UserManager
        /// </summary>
        /// <param name="userDAO">le DAO de user</param>
        public UserManager(UserDAO userDAO)
        {
            this.userDAO = userDAO;
        }

        /// <summary>
        /// renvoie un utilisateur a partir de son mail
        /// </summary>
        /// <returns>l utilisateur a qui appartient l'email</returns>
        public UserDTO GetUserByEmail(string email)
        {
            try
            {
                return this.userDAO.GetUserByEmail(email);
            }
            catch
            {
                throw new UserDoesNotExistsException();
;           }
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur
        /// </summary>
        /// <param name="userDTO">l'utilisateur à enregistrer</param>
        /// <returns>true si l'utilisateur a bien été inséré</returns>
        /// <exception cref="UserAlreadyExistsException">Se leve si l'utilisateur existe deja</exception>
        public bool Register(UserDTO userDTO)
        {
            bool res = false;
            //si l'utilisateur existe deja 
            if (this.userDAO.EmailExists(userDTO.Email))
            {
                throw new UserAlreadyExistsException();
            }
            try
            {
                //insere l'utilisateur
                this.userDAO.Register(userDTO);
                res = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return res;
        }
        /// <summary>
        /// Connecte un utilisateur au site a partir d'un login et d'un pwd 
        /// </summary>
        /// <param name="userDTO">Le user a connecter</param>
        /// <returns>un token</returns>
        public TokenDTO Login(UserDTO userDTO)
        {
            //on recupere le user en base
            var user = this.userDAO.Login(userDTO.Email);

            TokenDTO token = new TokenDTO();

            //on verifie son pwd
           if (SecurePasswordHasher.Verify(userDTO.HashPassword, user.HashPassword))
            {
                //generer token
                token.Token = "connexion reussi";
            }
           else
            {
                throw new UserWrongPassword();
            }

            return token;

        }
    }
}
