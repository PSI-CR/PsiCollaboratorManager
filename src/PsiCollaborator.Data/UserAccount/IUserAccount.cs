using System;

namespace PsiCollaborator.Data.UserAccount
{
    public interface IUserAccount
    {
        string Email { get; set; }
        string FirstName { get; set; }
        bool IsActive { get; set; }
        bool IsLockedOut { get; set; }
        string LastName { get; set; }
        DateTime? LockOutEndTime { get; set; }
        bool NeedPasswordChange { get; set; }
        string Telephone1 { get; set; }
        int UserAccountId { get; set; }
        string UserName { get; set; }
    }
}