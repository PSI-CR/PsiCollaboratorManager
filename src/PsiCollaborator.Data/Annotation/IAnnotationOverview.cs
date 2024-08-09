using System;

namespace PsiCollaborator.Data.Annotation
{
    public interface IAnnotationOverview
    {
        DateTime? Date { get; set; }
        int AnnotationId { get; set; }
        string AnnotationTypeName { get; set; }
        string CollaboratorDNICollaborator { get; set; }
        string CollaboratorEmail { get; set; }
        string CollaboratorFirstName { get; set; }
        string CollaboratorLastName { get; set; }
        int CollaboratorOperatorNumber { get; set; }
    }
}