using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.LoginAttemptCollaborator
{
    public class LoginAttemptCollaboratorRepository : DbMapper, ILoginAttemptCollaboratorRepository
    {
        public int Save(ILoginAttemptCollaborator loginCollaboratorAttempt)
        {
            return ExecuteSqlMapObject("Insert_Login_Attempt_Collaborator", loginCollaboratorAttempt);
        }
    }
}
