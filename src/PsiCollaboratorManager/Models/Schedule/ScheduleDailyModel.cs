using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Schedule
{
    public class ScheduleDailyModel
    {
        public int ScheduleDailyId { get; set; }
        public int DayId { get; set; }
        public string DayName { get; set; }
        public TimeModel BeginTime { get; set; }
        public TimeModel EndTime { get; set; }
    }
}