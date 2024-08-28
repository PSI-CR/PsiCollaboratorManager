using PsiCollaboratorManager.Models.Attendance;
using System.Collections.Generic;

namespace PsiCollaboratorManager.Models.Collaborator
{
    public class CollaboratorPictureModel
    {
        public int CollaboratorId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Picture { get; set; }
        public List<AttendModel> AttendModels { get; set; }
    }
}