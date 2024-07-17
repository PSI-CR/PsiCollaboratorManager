using System;

namespace PsiCollaborator.Data.Annotation
{
    public interface IAnnotationByCollaboratorDetails
    {
        DateTime? AnnotationDate { get; set; }
        int AnnotationId { get; set; }
        int AnnotationTypeId { get; set; }
        int AttachedFileId { get; set; }
        int CollaboratorId { get; set; }
        int CollaboratorPictureId { get; set; }
        string DNICollaborator { get; set; }
        string Email { get; set; }
        string FileData { get; set; }
        string FileName { get; set; }
        string FileType { get; set; }
        string FirstName { get; set; }
        bool IsActive { get; set; }
        string LastName { get; set; }
        string Note { get; set; }
        int OperatorNumber { get; set; }
        double Percentage { get; set; }
        string Picture { get; set; }
        string TypeName { get; set; }
        int UserId { get; set; }
        bool ValueInScore { get; set; }
        bool VisibleToCollaborator { get; set; }
    }
}