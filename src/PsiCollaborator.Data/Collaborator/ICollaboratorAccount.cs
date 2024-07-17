using System;

namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorAccount
    {
        int CollaboratorId { get; set; }
        bool IsActive { get; set; }
        bool IsLockedOut { get; set; }
        DateTime LockOutEndTime { get; set; }
        bool NeedPasswordChange { get; set; }
        int OperatorNumber { get; set; }
        string Password { get; set; }
        DateTime PasswordChangeDate { get; set; }
    }
}