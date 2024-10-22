using System;
using System.Collections.Generic;

namespace PsiCollaborator.Data.Annotation
{
    public interface IAnnotationRepository
    {
        IEnumerable<IAnnotationOverview> GetAll(DateTime? minAnnotationDate, DateTime? maxAnnotationDate);
        IAnnotationDetails GetAnnotationById(int annotationId);
        void Insert(Annotation annotation);
    }
}