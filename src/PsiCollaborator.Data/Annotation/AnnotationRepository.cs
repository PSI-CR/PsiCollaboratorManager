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
            ExecuteSqlMapObject("Insert_Annotation", annotation);
        }

        public IEnumerable<IAnnotationOverview> GetAll(DateTime? minAnnotationDate, DateTime? maxAnnotationDate, string firstNameFilter,
            string lastNameFilter, int operatorNumberFilter)
        {
            return ExecuteListWithParameters<AnnotationOverview>("Select_All_Annotations", new List<DbParameter>() {
                new DbParameter("param_minAnnotationDate", ParameterDirection.Input, minAnnotationDate == null ? DateTime.MinValue : minAnnotationDate ),
                new DbParameter("param_maxAnnotationDate", ParameterDirection.Input, maxAnnotationDate == null ? DateTime.MaxValue : maxAnnotationDate ),
                new DbParameter("param_firstNameFilter", ParameterDirection.Input,firstNameFilter == null ? DBNull.Value.ToString() : firstNameFilter ),
                new DbParameter("param_lastNameFilter", ParameterDirection.Input, lastNameFilter == null ? DBNull.Value.ToString() : lastNameFilter),
                new DbParameter("param_operatorNumberFilter", ParameterDirection.Input, operatorNumberFilter  == 0 ? -1 : operatorNumberFilter)
            });
        }

        public IAnnotationDetails GetAnnotationById(int annotationId)
        {
            return ExecuteSingle<AnnotationDetails>("Select_Annotation_By_Id", new List<DbParameter>() {
                new DbParameter("param_annotationId", ParameterDirection.Input, annotationId)
            });
        }
    }
}
