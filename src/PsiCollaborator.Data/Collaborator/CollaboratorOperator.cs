using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Collaborator
{
    public class CollaboratorOperator : ICollaboratorOperator
    {
        public int CollaboratorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OperatorNumber { get; set; }
    }
}
