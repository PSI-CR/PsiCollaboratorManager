using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Attendance
{
    public class CheckInStatusModel
    {
        public int CheckInStatusId {get; set; }
        public int TypeCheckInStatus {get; set; }
        public int LabelCheckInStatus {get; set; }
        public int Description {get; set; }
    }
}