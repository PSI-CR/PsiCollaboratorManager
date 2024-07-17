using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Schedule
{
    public class ScheduleBasic : IScheduleBasic
    {
        public int ScheduleId { get; set; }
        public string Name { get; set; }
    }
}
