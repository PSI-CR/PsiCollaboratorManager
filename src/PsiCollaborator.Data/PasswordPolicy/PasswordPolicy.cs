using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.PasswordPolicy
{
    public class PasswordPolicy : IPasswordPolicy
    {
        public int PasswordPolicyId { get; set; }
        public int MinLength { get; set; }
        public int PasswordDuration { get; set; }

        public bool UseLowerCase { get; set; }
        public bool UseUpperCase { get; set; }
        public bool UseNumbers { get; set; }
        public bool UseSymbols { get; set; }

        public bool CheckIfIsCommonlyUsed { get; set; }
        public bool CheckIfIsRecent { get; set; }
        public bool CheckIfContainsUserInfo { get; set; }
    }
}
