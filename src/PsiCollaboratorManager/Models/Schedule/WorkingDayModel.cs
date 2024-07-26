using PsiCollaborator.Data.Schedule.WorkingDay;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Schedule
{
    public class WorkingDayModel
    {
        [Key]
        public int WorkingDayId { get; set; }
        [Required]
        [Display(Name = "Nombre jornada:")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Descripción:")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Máximo Días:")]
        public int MaxDays { get; set; }

        [Required]
        [Display(Name = "Máximo Horas:")]
        public int MaxHours { get; set; }

        [Required]
        [Display(Name = "Hora Inicial:")]
        public DateTime StartTime { get; set; }
        [Required]

        [Display(Name = "Hora Final:")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Jornada Acumulativa:")]
        public bool Accumulative { get; set; }

        public bool Assigned { get; set; }

        public DateTime RecordTime { get; set; }
    }
}

