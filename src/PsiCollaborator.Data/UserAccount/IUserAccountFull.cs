using System;

namespace PsiCollaborator.Data.UserAccount
{
    public interface IUserAccountFull
    {
        string ApplicationLastLogin { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        bool IsActive { get; set; }
        bool IsActual { get; set; }
        bool IsLockedOut { get; set; }
        DateTime? LastLoginDate { get; set; }
        string LastName { get; set; }
        DateTime? LockOutEndTime { get; set; }
        bool NeedPasswordChange { get; set; }
        DateTime PasswordChangedDate { get; set; }
        string PasswordHash { get; set; }
        string Telephone1 { get; set; }
        int UserAccountId { get; set; }
        string UserName { get; set; }
    }
}