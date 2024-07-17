namespace PsiCollaborator.Data.Annotation.AnnotationType
{
    public interface IAnnotationType
    {
        int AnnotationTypeId { get; set; }
        double Percentage { get; set; }
        string TypeName { get; set; }
        bool ValueInScore { get; set; }
        bool VisibleToCollaborator { get; set; }
    }
}