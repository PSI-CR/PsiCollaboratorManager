using PasswordTester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.PasswordUtilities
{
    public class CommonPasswordChecker
    {
        public static bool CheckPasswordIsCommonlyUsed(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password is empty or null", "password");

            try
            {
                var result = PasswordLookup.LookupAsync(password).GetAwaiter().GetResult();
                if (result)
                {
                    return result.HasHit;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
