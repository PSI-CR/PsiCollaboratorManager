using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.PasswordUtilities
{
    public class PasswordValidator
    {
        public int MinLength { private set; get; } = 8;
        public bool UseUpperCase { private set; get; } = true;
        public bool UseLowerCase { private set; get; } = true;
        public bool UseNumbers { private set; get; } = true;
        public bool UseSymbols { private set; get; } = true;

        public PasswordValidator() { }

        public PasswordValidator(int minLength, bool useLowerCase, bool useUpperCase, bool useNumbers, bool useSymbols)
        {
            MinLength = minLength;
            UseLowerCase = useLowerCase;
            UseUpperCase = useUpperCase;
            UseNumbers = useNumbers;
            UseSymbols = useSymbols;
        }
        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password is empty or null", "password");
            var passwordContent = CheckPasswordPolicy(password);
            return isValidPassword(passwordContent);
        }

        private bool isValidPassword(PasswordContent passwordContent)
        {
            if (!passwordContent.HasMinLength)
                return false;

            if (UseLowerCase && !passwordContent.ContainsLowerCase)
                return false;

            if (UseUpperCase && !passwordContent.ContainsUpperCase)
                return false;

            if (UseNumbers && !passwordContent.CointainsNumber)
                return false;

            if (UseSymbols && !passwordContent.CointainsSymbol)
                return false;

            return true;
        }
        public PasswordContent CheckPasswordPolicy(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("password is empty or null", "password");

            var lowerCaseRegex = "(?=.*[a-z])";
            var upperCaseRegex = "(?=.*[A-Z])";
            var numericRegex = "(?=.*[0-9])";
            var symbolsRegex = "(?=.*[!@#$%&*-+.?:;_])";

            var passwordContent = new PasswordContent();

            if (password.Length >= MinLength)
                passwordContent.HasMinLength = true;

            if (UseLowerCase && new Regex(@"^" + lowerCaseRegex + "").IsMatch(password))
                passwordContent.ContainsLowerCase = true;

            if (UseUpperCase && new Regex(@"^" + upperCaseRegex + "").IsMatch(password))
                passwordContent.ContainsUpperCase = true;

            if (UseNumbers && new Regex(@"^" + numericRegex + "").IsMatch(password))
                passwordContent.CointainsNumber = true;

            if (UseSymbols && new Regex(@"^" + symbolsRegex + "").IsMatch(password))
                passwordContent.CointainsSymbol = true;

            passwordContent.IsValidPassword = isValidPassword(passwordContent);

            return passwordContent;
        }
    }
    public class PasswordContent
    {
        public bool IsValidPassword { get; set; }
        public bool HasMinLength { get; set; }
        public bool ContainsLowerCase { get; set; }
        public bool ContainsUpperCase { get; set; }
        public bool CointainsNumber { get; set; }
        public bool CointainsSymbol { get; set; }
    }
}
