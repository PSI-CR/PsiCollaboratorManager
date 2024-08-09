using PsiCollaboratorManager.Models.Collaborator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Annotation
{
    public class AnnotationModel
    {
        public int AnnotationId { get; set; }
        public int UserId { get; set; }
        [Display(Name = "Tipo de Anotación")]
        public AnnotationTypeModel AnnotationTypeModel { get; set; }
        [Display(Name = "Nota")]
        public string Note { get; set; }
        [Display(Name = "Fecha")]
        public DateTime? AnnotationDate { get; set; }
        [Display(Name = "Archivo")]
        public string FileData { get; set; }
        [Display(Name = "Nombre del Archivo")]
        public string FileName { get; set; }
        [Display(Name = "Tipo de Archivo")]
        public string FileType { get; set; }
        [Display(Name = "Collaboradores")]
        public List<CollaboratorDetailsModel> Collaborators { get; set; }
    }
}