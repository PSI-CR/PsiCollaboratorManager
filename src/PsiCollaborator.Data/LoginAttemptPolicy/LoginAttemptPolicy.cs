using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.LoginAttemptPolicy
{
    public class LoginAttemptPolicy : ILoginAttemptPolicy
    {
        public int LoginAttemptPolicyId { get; set; }
        public bool IsActive { get; set; }
        public int MaxInvalidAttempts { get; set; }
        public TimeSpan InvalidAttemptsTime { get; set; }
        public TimeSpan LoginLockoutTime { get; set; }
    }
}
