using System;

namespace PsiCollaborator.Data.Annotation
{
    public interface IAnnotationDetails
    {
        int AnnotationId { get; set; }
        string AnnotationTypeName { get; set; }
        string CollaboratorDNICollaborator { get; set; }
        string CollaboratorFirstName { get; set; }
        string CollaboratorLastName { get; set; }
        int CollaboratorOperatorNumber { get; set; }
        string CollaboratorPicture { get; set; }
        DateTime? Date { get; set; }
        string Note { get; set; }
    }
}