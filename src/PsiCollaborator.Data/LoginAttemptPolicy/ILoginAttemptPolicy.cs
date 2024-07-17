using System;

namespace PsiCollaborator.Data.LoginAttemptPolicy
{
    public interface ILoginAttemptPolicy
    {
        TimeSpan InvalidAttemptsTime { get; set; }
        bool IsActive { get; set; }
        int LoginAttemptPolicyId { get; set; }
        TimeSpan LoginLockoutTime { get; set; }
        int MaxInvalidAttempts { get; set; }
    }
}