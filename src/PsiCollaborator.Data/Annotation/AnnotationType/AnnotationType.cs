using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Annotation.AnnotationType
{
    public class AnnotationType : IAnnotationType
    {
        public int AnnotationTypeId { get; set; }
        public string TypeName { get; set; }
        public bool ValueInScore { get; set; }
        public bool VisibleToCollaborator { get; set; }
        public double Percentage { get; set; }
    }
}
