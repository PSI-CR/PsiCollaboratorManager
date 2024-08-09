using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Attendance
{
    public class AttendModel
    {
        public int AttendId { get; set; }
        public int CollaboratorId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int CheckInStatus { get; set; }
        public int CheckOutStatus { get; set; }
        public string CommentCheckIn { get; set; }
        public bool IsOpenCheckIn { get; set; }
        public string IpAddress { get; set; }
        public string PhysicalAddressEquipment { get; set; }
        public string CheckInStatusWork { get; set; }
        public string CheckOutStatusWork { get; set; }
    }
}