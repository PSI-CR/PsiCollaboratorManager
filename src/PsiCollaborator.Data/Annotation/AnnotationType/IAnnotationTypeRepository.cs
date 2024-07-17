using System.Collections.Generic;

namespace PsiCollaborator.Data.Annotation.AnnotationType
{
    public interface IAnnotationTypeRepository
    {
        List<AnnotationType> GetAll();
        AnnotationType GetById(int annotationTypeId);
        void Save(AnnotationType annotationType);
    }
}