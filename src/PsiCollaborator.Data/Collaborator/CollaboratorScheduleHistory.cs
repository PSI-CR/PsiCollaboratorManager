using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Collaborator
{
    public class CollaboratorScheduleHistory : ICollaboratorScheduleHistory
    {
        public int CollaboratorScheduleHistoryId { get; set; }
        public int CollaboratorId { get; set; }
        public int ScheduleId { get; set; }
        public bool Active { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime DismissDate { get; set; }
    }
}
