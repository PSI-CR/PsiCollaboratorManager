using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.LoginAttemptPolicy
{
    public class LoginAttemptPolicyRepository : DbMapper, ILoginAttemptPolicyRepository
    {
        public ILoginAttemptPolicy GetById(int logginAttemptPolicyId)
        {
            return ExecuteSingle<LoginAttemptPolicy>("Select_Connection_Policy_By_Id", new List<DbParameter>() { new DbParameter("param_loginattemptpolicyid", ParameterDirection.Input, logginAttemptPolicyId) });
        }
    }
}
