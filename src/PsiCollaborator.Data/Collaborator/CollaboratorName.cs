using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Collaborator
{
    public class CollaboratorName : ICollaboratorName
    {
        public int CollaboratorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
