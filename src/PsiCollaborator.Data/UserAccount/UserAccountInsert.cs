using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.UserAccount
{
    public class UserAccountInsert : IUserAccountInsert
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone1 { get; set; }
        public bool NeedPasswordChange { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime? LockOutEndTime { get; set; }
        public bool IsActive { get; set; }
        public string PasswordHash { get; set; }
    }
}
