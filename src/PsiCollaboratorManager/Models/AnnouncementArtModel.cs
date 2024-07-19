using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models
{
    public class AnnouncementArtModel
    {
        public int AnnouncementArtId { get; set; }
        [Display(Name = "Imagen:")]
        public string Image { get; set; }
        [Display(Name = "Fecha inicial:")]
        public DateTime BeginDatePublication { get; set; }
        [Display(Name = "Fecha final:")]
        public DateTime EndDatePublication { get; set; }
        [Display(Name = "Descripción:")]
        public string Description { get; set; }
    }
}