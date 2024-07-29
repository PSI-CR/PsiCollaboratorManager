using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Annotation
{
    public class Annotation : IAnnotation
    {
        public int AnnotationId { get; set; }
        public int[] CollaboratorIds { get; set; }
        public int AnnotationTypeId { get; set; }
        public int UserId { get; set; }
        public string Note { get; set; }
        public DateTime? Date { get; set; }
        public string FileData { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}
