using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Schedule
{
    public class ScheduleWorkingDayModel
    {
        public int ScheduleId { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Jornada")]
        public WorkingDayModel WorkingDay { get; set; }
        public List<ScheduleDailyModel> ScheduleDailys { get; set; }
    }
}