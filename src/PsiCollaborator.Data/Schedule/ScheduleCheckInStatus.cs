using PsiCollaborator.Data.Attend;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Schedule
{
    public class ScheduleCheckInStatus : IScheduleCheckInStatus
    {
        public int checkinstatusid { get; set; }
        public string typecheckinstatus { get; set; }
        public string labelcheckinstatus { get; set; }
        public string description { get; set; }
    }
}
