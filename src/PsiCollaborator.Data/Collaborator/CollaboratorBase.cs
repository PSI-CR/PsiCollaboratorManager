using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Collaborator
{
    public class CollaboratorBase : ICollaboratorBase
    {
        public int CollaboratorId { get; set; }
        public int OperatorNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DNICollaborator { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public bool Parent { get; set; }
        public int? MaritalStatusId { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string ProvinceName { get; set; }
        public string CantonName { get; set; }
        public string DistrictName { get; set; }
        public string AddressLine { get; set; }
    }
}
