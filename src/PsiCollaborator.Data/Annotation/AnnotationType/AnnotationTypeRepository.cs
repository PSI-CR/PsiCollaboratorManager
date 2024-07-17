using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Annotation.AnnotationType
{
    public class AnnotationTypeRepository : DbMapper, IAnnotationTypeRepository
    {
        public List<AnnotationType> GetAll()
        {
            return ExecuteList<AnnotationType>("GetAllAnnotationTypes").ToList();
        }
        public void Save(AnnotationType annotationType)
        {
            ExecuteSqlMapObject("SaveAnnotationType", annotationType);
        }
        public AnnotationType GetById(int annotationTypeId)
        {
            var result = ExecuteSingle<AnnotationType>("GetAnnotationTypeById", new List<DbParameter>() {
                new DbParameter("param_annotationTypeId", ParameterDirection.Input, annotationTypeId)
            });
            return result;
        }
    }
}
