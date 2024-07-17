using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Collaborator
{
    public class CollaboratorAccount : ICollaboratorAccount
    {
        public int CollaboratorId { get; set; }
        public int OperatorNumber { get; set; }
        public bool NeedPasswordChange { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime LockOutEndTime { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public DateTime PasswordChangeDate { get; set; }
    }
}
