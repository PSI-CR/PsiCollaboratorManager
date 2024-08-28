using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Attend
{
    public class Attendance : IAttendance
    {
        public int AttendanceId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int CheckInStatus { get; set; }
        public int CheckOutStatus { get; set; }
        public string CommentCheckIn { get; set; }
        public bool IsOpenCheckIn { get; set; }
    }
}
