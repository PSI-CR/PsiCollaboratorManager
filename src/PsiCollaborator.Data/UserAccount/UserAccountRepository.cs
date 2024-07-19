using PsiCollaborator.Data.PasswordUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.UserAccount
{
    public class UserAccountRepository : DbMapper, IUserAccountRepository
    {
        public IUserAccountFull GetByUserName(string userName)
        {
            var userAccount = ExecuteSingle<UserAccountFull>("Select_User_Account_By_UserName", new List<DbParameter>() { new DbParameter("param_userName", ParameterDirection.Input, userName) });
            return userAccount;
        }
        public void Insert(IUserAccountInsert userAccount)
        {
            ExecuteSqlMapObject("Insert_User_Account", userAccount);
        }
        public void Update(IUserAccount userAccount)
        {
            ExecuteSqlMapObject("Update_User_Account", userAccount);
        }
        public void UpdatePassword(int userAccountId, string newPassword, bool needPasswordChange)
        {
            ExecuteSql("Update_User_PasswordHistory", new List<DbParameter>() {
                new DbParameter("param_UserAccountId", ParameterDirection.Input, userAccountId),
                new DbParameter("param_PasswordHash", ParameterDirection.Input, PBKDF2Converter.GetHashPassword(newPassword)),
                new DbParameter("param_needPasswordChange", ParameterDirection.Input, needPasswordChange)
            });
        }
        public IUserAccount GetById(int userAccountId)
        {
            return ExecuteSingle<UserAccount>("Select_User_Account_By_Id", new List<DbParameter>() {
                new DbParameter("param_UserAccountId", ParameterDirection.Input, userAccountId)
            });
        }
        public IEnumerable<IUserAccount> GetAll()
        {
            return ExecuteList<UserAccount>("Select_All_User_Accounts");
        }
        public bool CheckIfPasswordIsRecent(int userAccountId, string password)
        {
            var lastPasswords = ExecuteListWithParameters<Password>("Select_User_Last_Passwords", new List<DbParameter>() { new DbParameter("param_useraccountid", ParameterDirection.Input, userAccountId) });
            foreach (var userPassword in lastPasswords)
            {
                if (PBKDF2Converter.IsValidPassword(password, userPassword.PasswordHash))
                    return true;
            }
            return false;
        }
        public ILockOut VerifyAccountIsLockedOut(int userAccountId, int maxInvalidAttempts, TimeSpan invalidAttemptsTime, TimeSpan loginLockOutTime)
        {
            return ExecuteSingle<LockOut>("Verify_User_Unlock", new List<DbParameter>() {
                new DbParameter("param_useraccountid", ParameterDirection.Input, userAccountId),
                new DbParameter("param_maxinvalidattempts", ParameterDirection.Input, maxInvalidAttempts),
                new DbParameter("param_invalidattemptstime", ParameterDirection.Input, invalidAttemptsTime),
                new DbParameter("param_loginlockouttime", ParameterDirection.Input, loginLockOutTime)
            });
        }
    }
}
