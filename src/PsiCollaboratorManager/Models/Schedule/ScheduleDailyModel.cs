using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Schedule
{
    public class ScheduleDailyModel
    {
        public int ScheduleDailyId { get; set; }
        public string ScheduleDayName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}