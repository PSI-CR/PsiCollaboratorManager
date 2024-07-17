using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.PasswordPolicy
{
    public class PasswordPolicyRepository : DbMapper, IPasswordPolicyRepository
    {
        public IPasswordPolicy GetById(int passwordPolicyId)
        {
            return ExecuteSingle<PasswordPolicy>("Select_Password_Policy_by_Id", new List<DbParameter>() { new DbParameter("param_passwordpolicyid", ParameterDirection.Input, passwordPolicyId) });
        }
    }
}
