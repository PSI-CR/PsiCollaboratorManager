using System;

namespace PsiCollaborator.Data.Attend
{
    public interface IAttendance
    {
        int AttendanceId { get; set; }
        DateTime CheckIn { get; set; }
        int CheckInStatus { get; set; }
        DateTime? CheckOut { get; set; }
        int CheckOutStatus { get; set; }
        string CommentCheckIn { get; set; }
        bool IsOpenCheckIn { get; set; }
    }
}