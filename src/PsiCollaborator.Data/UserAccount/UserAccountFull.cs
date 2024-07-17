using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.UserAccount
{
    public class UserAccountFull : IUserAccountFull
    {
        public int UserAccountId { get; set; }
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
        public DateTime PasswordChangedDate { get; set; }
        public bool IsActual { get; set; }

        public DateTime? LastLoginDate { get; set; }
        public string ApplicationLastLogin { get; set; }
    }
}
