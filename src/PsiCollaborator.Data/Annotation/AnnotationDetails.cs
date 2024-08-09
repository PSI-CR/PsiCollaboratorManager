using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Annotation
{
    public class AnnotationDetails : IAnnotationDetails
    {
        public int AnnotationId { get; set; }
        public DateTime? Date { get; set; }
        public string Note { get; set; }
        public string AnnotationTypeName { get; set; }
        public string CollaboratorFirstName { get; set; }
        public string CollaboratorLastName { get; set; }
        public string CollaboratorDNICollaborator { get; set; }
        public int CollaboratorOperatorNumber { get; set; }
        public string CollaboratorPicture { get; set; }
    }
}
