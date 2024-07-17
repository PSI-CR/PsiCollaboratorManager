using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data
{
    public class Password : IPassword
    {
        public string PasswordHash { get; set; }

        public Password() { }
        public Password(string password)
        {
            PasswordHash = password;
        }
    }
}
