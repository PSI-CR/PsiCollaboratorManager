using System;
using System.Collections.Generic;

namespace PsiCollaborator.Data.Annotation
{
    public interface IAnnotationRepository
    {
        IEnumerable<IAnnotationByCollaboratorDetails> GetAll(DateTime? minAnnotationDate, DateTime? maxAnnotationDate, string firstNameFilter, string lastNameFilter, int operatorNumberFilter);
        IAnnotationByCollaboratorDetails GetAnnotationById(int annotationId);
        void Insert(Annotation annotation);
    }
}