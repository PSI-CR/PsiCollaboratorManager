using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Annotation
{
    public class AnnotationTypeModel
    {
        public int AnnotationTypeId { get; set; }
        [Display(Name = "Nombre")]
        public string TypeName { get; set; }
        [Display(Name = "Valor en la Nota")]
        public bool ValueInScore { get; set; }
        [Display(Name = "Visible para el colaborador")]
        public bool VisibleToCollaborator { get; set; }
        [Display(Name = "Porcentaje")]
        public double Percentage { get; set; }
    }
}