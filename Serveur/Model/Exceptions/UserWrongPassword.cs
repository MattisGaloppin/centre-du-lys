namespace Serveur.Model.Exceptions
{
    public class UserWrongPassword : Exception
    {
        public UserWrongPassword(): base("Le mot de passe ne correspond pas"){}
    }
}
