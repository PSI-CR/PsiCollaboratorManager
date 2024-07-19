using System;
using System.Collections.Generic;

namespace PsiCollaborator.Data.UserAccount
{
    public interface IUserAccountRepository
    {
        bool CheckIfPasswordIsRecent(int userAccountId, string password);
        IEnumerable<IUserAccount> GetAll();
        IUserAccount GetById(int userAccountId);
        IUserAccountFull GetByUserName(string userName);
        void Insert(IUserAccountInsert userAccount);
        void Update(IUserAccount userAccount);
        void UpdatePassword(int userAccountId, string newPassword, bool needPasswordChange);
        ILockOut VerifyAccountIsLockedOut(int userAccountId, int maxInvalidAttempts, TimeSpan invalidAttemptsTime, TimeSpan loginLockOutTime);
    }
}