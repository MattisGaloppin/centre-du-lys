using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serveur.Model.Data;
using Serveur.Model.DTO;

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
    /// renvoie la liste des users
    /// </summary>
    /// <returns>les users du site</returns>
    public List<UserDTO> GetAll()
    {
        return this.userDAO.GetAllUsers();
    }
      

    }
}
