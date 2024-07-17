using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Annotation
{
    public class AnnotationRepository : DbMapper, IAnnotationRepository
    {
        public void Insert(Annotation annotation)
        {
            ExecuteSql("Insert_Annotation", new List<DbParameter>() {
                new DbParameter("param_collaboratorIds", ParameterDirection.Input, annotation.Collaborators),
                new DbParameter("param_annotationTypeId", ParameterDirection.Input, annotation.AnnotationTypeId),
                new DbParameter("param_userAccountId", ParameterDirection.Input, annotation.UserId),
                new DbParameter("param_attachmentFileData", ParameterDirection.Input,annotation.FileData == null ? DBNull.Value.ToString() : annotation.FileData ),
                new DbParameter("param_attachmentFileName", ParameterDirection.Input, annotation.FileName  == null ? DBNull.Value.ToString() : annotation.FileName),
                new DbParameter("param_attachmentFileType", ParameterDirection.Input, annotation.FileType  == null ? DBNull.Value.ToString() : annotation.FileType),
                new DbParameter("param_noteText", ParameterDirection.Input, annotation.Note),
                new DbParameter("param_annotationDate", ParameterDirection.Input, annotation.AnnotationDate)
            });
        }

        public IEnumerable<IAnnotationByCollaboratorDetails> GetAll(DateTime? minAnnotationDate, DateTime? maxAnnotationDate, string firstNameFilter,
            string lastNameFilter, int operatorNumberFilter)
        {
            return ExecuteListWithParameters<AnnotationByCollaboratorDetails>("Select_All_Annotations", new List<DbParameter>() {
                new DbParameter("param_minAnnotationDate", ParameterDirection.Input, minAnnotationDate == null ? DateTime.MinValue : minAnnotationDate ),
                new DbParameter("param_maxAnnotationDate", ParameterDirection.Input, maxAnnotationDate == null ? DateTime.MaxValue : maxAnnotationDate ),
                new DbParameter("param_firstNameFilter", ParameterDirection.Input,firstNameFilter == null ? DBNull.Value.ToString() : firstNameFilter ),
                new DbParameter("param_lastNameFilter", ParameterDirection.Input, lastNameFilter == null ? DBNull.Value.ToString() : lastNameFilter),
                new DbParameter("param_operatorNumberFilter", ParameterDirection.Input, operatorNumberFilter  == 0 ? -1 : operatorNumberFilter)
            });
        }

        public IAnnotationByCollaboratorDetails GetAnnotationById(int annotationId)
        {
            return ExecuteSingle<AnnotationByCollaboratorDetails>("Select_Annotation_By_Id", new List<DbParameter>() {
                new DbParameter("param_annotationId", ParameterDirection.Input, annotationId)
            });
        }
    }
}
