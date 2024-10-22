using PsiCollaborator.Data.UserAccount;
using PsiCollaboratorManager.Models.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Mapping
{
    public class UserAccountMapper
    {
        public IUserAccount Map(IUserAccountFull source)
        {
            if (source == null) return null;

            return new UserAccount
            {
                UserAccountId = source.UserAccountId,
                UserName = source.UserName,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                Telephone1 = source.Telephone1,
                NeedPasswordChange = source.NeedPasswordChange,
                IsLockedOut = source.IsLockedOut,
                LockOutEndTime = source.LockOutEndTime,
                IsActive = source.IsActive
            };
        }
        public IUserAccountInsert Fill(UserAccountModel userAccountModel, string userName, string password)
        {
            IUserAccountInsert userAccountFull = new UserAccountInsert();
            userAccountFull.UserName = userName;
            userAccountFull.FirstName = userAccountModel.FirstName;
            userAccountFull.LastName = userAccountModel.LastName;
            userAccountFull.Email = userAccountModel.Email;
            userAccountFull.Telephone1 = "0";
            userAccountFull.NeedPasswordChange = true;
            userAccountFull.IsLockedOut = false;
            userAccountFull.LockOutEndTime = new DateTime();
            userAccountFull.IsActive = true;

            userAccountFull.PasswordHash = password;

            return userAccountFull;

        }
    }
}