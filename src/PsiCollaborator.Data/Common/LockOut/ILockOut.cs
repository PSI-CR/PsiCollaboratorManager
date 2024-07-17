using System;

namespace PsiCollaborator.Data
{
    public interface ILockOut
    {
        bool IsLockedOut { get; set; }
        DateTime? LockOutEndTime { get; set; }
    }
}