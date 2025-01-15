using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace Serveur.Model
{
    /// <summary>
    /// Permet de sécuriser le pwd
    /// </summary>
    public static class SecurePasswordHasher
    {
        private const int saltSize = 16;
        private const int iteration = 9874;
        private const string format = "$MYHASH$V1$"; // Format du hashage
        private const int hashSize = 20;

        // Salt fixe
        private static readonly byte[] fixedSalt = new byte[saltSize] {
            0x24, 0xF6, 0xB2, 0x85, 0x1A, 0x43, 0xA0, 0xD2, 0xF1, 0x7A, 0x65, 0x78,
            0x34, 0xC2, 0xFB, 0x5D
        };

        /// <summary>
        /// Crée un mot de passe hashé à partir d'un mot de passe.
        /// </summary>
        /// <param name="password">Le mot de passe.</param>
        /// <returns>Le hash.</returns>
        public static string Hash(string password, int iterations)
        {
            // Utilise un salt fixe
            byte[] salt = fixedSalt;

            // Crée le hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(hashSize);

            // Combine salt et hash
            var hashBytes = new byte[saltSize + hashSize];
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, hashSize);

            // Convertit en base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            // Format du hash avec les informations supplémentaires
            return string.Format(format + "{0}${1}", iterations, base64Hash);
        }

        /// <summary>
        /// Crée un hash du mot de passe avec un nombre d'itérations par défaut.
        /// </summary>
        /// <param name="password">Le mot de passe.</param>
        /// <returns>Le hash.</returns>
        public static string Hash(string password)
        {
            return Hash(password, iteration);
        }

        /// <summary>
        /// Vérifie si le hash est supporté.
        /// </summary>
        /// <param name="hashString">Le hash.</param>
        /// <returns>Si le hash est supporté.</returns>
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains(format);
        }

        /// <summary>
        /// Vérifie si un mot de passe correspond à un hash.
        /// </summary>
        /// <param name="password">Le mot de passe.</param>
        /// <param name="hashedPassword">Le hash.</param>
        /// <returns>Vrai si le mot de passe peut être vérifié.</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            // Vérifie le format
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("Le type de hash n'est pas supporté");
            }

            // Extrait les itérations et le hash de la chaîne
            var splittedHashString = hashedPassword.Replace(format, "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            // Conversion en bytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            // Récupère le salt
            var salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);

            // Crée le hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(hashSize);

            // Vérifie le résultat
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
