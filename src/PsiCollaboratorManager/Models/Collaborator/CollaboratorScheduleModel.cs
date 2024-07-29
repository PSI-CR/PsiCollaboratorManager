using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Collaborator
{
    public class CollaboratorScheduleModel
    {
        public int CollaboratorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DNICollaborator { get; set; }
        public int OperatorNumber { get; set; }
        public string Email { get; set; }
        public int CollaboratorScheduleHistoryId { get; set; }
        public DateTime AssignDate { get; set; }
        public string ScheduleName { get; set; }
    }
}