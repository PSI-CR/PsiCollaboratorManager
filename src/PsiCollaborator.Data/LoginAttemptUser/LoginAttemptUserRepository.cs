using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.LoginAttemptUser
{
    public class LoginAttemptUserRepository : DbMapper, ILoginAttemptUserRepository
    {
        public int Insert(ILoginAttemptUser loginUserAttempt)
        {
            return ExecuteSqlMapObject("Insert_Login_Attempt_User", loginUserAttempt);
        }
    }
}
