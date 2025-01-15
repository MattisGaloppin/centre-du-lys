namespace Serveur.Model.Exceptions
{
    public class UserDoesNotExistsException : Exception
    {
        public UserDoesNotExistsException(): base("L'utilisateur n'existe pas.")
        {
        }
    }
}
