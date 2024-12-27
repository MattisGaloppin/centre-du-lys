using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace Serveur.Model
{
    public static class SecurePasswordHasher
    {

        private const int saltSize = 16;
        private const int iteration = 9874;
        private const string format = "$MYHASH$V1$"; //format du hashage
        private const int hashSize = 20;

        /// <summary>
        /// Creer un pwd hashé à partir d'un pwd.
        /// </summary>
        /// <param name="password">Le password.</param>
        /// <returns>The hash.</returns>
        public static string Hash(string password, int iterations)
        {
            // Create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[saltSize]);

            // Create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(hashSize);

            // Combine salt and hash
            var hashBytes = new byte[saltSize + hashSize];
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, hashSize);

            // Convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            // Format hash with extra information
            return string.Format(format+"{0}${1}", iterations, base64Hash);
        }

        /// <summary>
        /// Creer un hash pwd avec un nb d' iterations
        /// </summary>
        /// <param name="password">le pwd.</param>
        /// <returns>le hash.</returns>
        public static string Hash(string password)
        {
            return Hash(password, iteration);
        }

        /// <summary>
        /// Verifie si le hash est supporté
        /// </summary>
        /// <param name="hashString">le hash.</param>
        /// <returns>si il est supporté</returns>
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains(format);
        }

        /// <summary>
        /// Verifies a password against a hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="hashedPassword">The hash.</param>
        /// <returns>Could be verified?</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            // verifie le format
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            // extrait les itération en base 64
            var splittedHashString = hashedPassword.Replace(format, "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            // conversion en bytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            // Obtient le sel
            var salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);

            // Creer le hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(hashSize);

            // Obtient le resultat
            for (var i = 0; i < hashSize; i++)
            {
                if (hashBytes[i + saltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
