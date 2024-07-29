using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Annotation
{
    public class AnnotationOverviewModel
    {
        public int AnnotationId { get; set; }
        public DateTime? Date { get; set; }
        public string AnnotationTypeName { get; set; }
        public string CollaboratorFirstName { get; set; }
        public string CollaboratorLastName { get; set; }
        public string CollaboratorDNICollaborator { get; set; }
        public int CollaboratorOperatorNumber { get; set; }
        public string CollaboratorEmail { get; set; }
    }
}