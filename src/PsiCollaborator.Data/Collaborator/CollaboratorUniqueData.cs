using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Collaborator
{
    public class CollaboratorUniqueData : ICollaboratorUniqueData
    {
        public int CollaboratorId { get; set; }
        public string Email { get; set; }
        public string Telephone1 { get; set; }
        public int OperatorNumber { get; set; }
        public string DNICollaborator { get; set; }
    }
}
