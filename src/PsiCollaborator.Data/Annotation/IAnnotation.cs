using System;

namespace PsiCollaborator.Data.Annotation
{
    public interface IAnnotation
    {
        DateTime? AnnotationDate { get; set; }
        int AnnotationId { get; set; }
        int AnnotationTypeId { get; set; }
        int[] Collaborators { get; set; }
        string FileData { get; set; }
        string FileName { get; set; }
        string FileType { get; set; }
        string Note { get; set; }
        int UserId { get; set; }
    }
}