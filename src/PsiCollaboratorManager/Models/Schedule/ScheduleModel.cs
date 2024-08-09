using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PsiCollaboratorManager.Models.Schedule
{
    public class ScheduleModel
    {
        public int ScheduleId { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Jornada")]
        public int WorkingDayId { get; set; }
        public List<ScheduleDailyModel> ScheduleDailys { get; set; }
    }
}