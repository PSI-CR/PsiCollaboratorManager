using PsiCollaborator.Data.Attend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Schedule
{
    public class ScheduleCheckOutStatus : IScheduleCheckOutStatus
    {
        public int checkoutstatusid { get; set; }
        public string typecheckoutstatus { get; set; }
        public string labelcheckoutstatus { get; set; }
        public string description { get; set; }
    }
}
