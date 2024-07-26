using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Schedule
{
    public class ScheduleDataModel
    {
        public int ScheduleId { get; set; }
        public string Name { get; set; }
        public string WorkingDayName { get; set; }
        public string WorkingDayDescription { get; set; }
        public string Assigned { get; set; }
    }
}