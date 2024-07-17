using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.PasswordUtilities
{
    public class PBKDF2Converter
    {
        private const int SALT_SIZE = 24; // size in bytes, recommended 64 bits
        private const int HASH_SIZE = 24; // size in bytes
        private const int ITERATIONS = 10000; // number of pbkdf2 iterations, recommended 1000

        private static byte[] createHash(string password, byte[] salt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("password is empty or null", "password");

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            return pbkdf2.GetBytes(HASH_SIZE);
        }

        public static string GetHashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException(nameof(password) + "is empty or null", nameof(password));
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SALT_SIZE]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            byte[] hash = pbkdf2.GetBytes(HASH_SIZE);

            byte[] hashBytes = new byte[SALT_SIZE + HASH_SIZE];
            Array.Copy(salt, 0, hashBytes, 0, SALT_SIZE);
            Array.Copy(hash, 0, hashBytes, SALT_SIZE, HASH_SIZE);

            string hashPass = Convert.ToBase64String(hashBytes);

            return hashPass;
        }

        public static bool IsValidPassword(string password, string hashPass)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException(nameof(password) + "is empty or null", nameof(password));
            if (string.IsNullOrEmpty(hashPass))
                throw new ArgumentException(nameof(hashPass) + "is empty or null", nameof(hashPass));

            byte[] hashBytes = Convert.FromBase64String(hashPass);
            byte[] salt = new byte[SALT_SIZE];
            Array.Copy(hashBytes, 0, salt, 0, SALT_SIZE);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            byte[] hash = pbkdf2.GetBytes(HASH_SIZE);
            for (int i = 0; i < HASH_SIZE; i++)
            {
                if (hashBytes[i + SALT_SIZE] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
