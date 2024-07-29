using PsiCollaborator.Data.Annotation;
using PsiCollaboratorManager.Models.Annotation;
using System.Linq;

namespace PsiCollaboratorManager.Mapping
{
    public class AnnotationMapper
    {
        public Annotation Map(AnnotationModel annotationModel)
        {
            return new Annotation() {
                AnnotationId = annotationModel.AnnotationId,
                CollaboratorIds = annotationModel.Collaborators.Select(x => x.CollaboratorId).ToArray(),
                AnnotationTypeId = annotationModel.AnnotationTypeModel.AnnotationTypeId,
                UserId = annotationModel.UserId,
                Note = annotationModel.Note,
                Date = annotationModel.AnnotationDate,
                FileData = annotationModel.FileData,
                FileName = annotationModel.FileName,
                FileType = annotationModel.FileType
            };
        }
    }
}