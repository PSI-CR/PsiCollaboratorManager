using System;

namespace PsiCollaborator.Data.Attend
{
    public interface IAttend
    {
        int AttendanceId { get; set; }
        int CollaboratorId { get; set; }
        DateTime CheckIn { get; set; }
        int CheckInStatus { get; set; }
        DateTime? CheckOut { get; set; }
        int CheckOutStatus { get; set; }
        string CommentCheckIn { get; set; }
        string IpAddress { get; set; }
        bool IsOpenCheckIn { get; set; }
        string PhysicalAddressEquipment { get; set; }
        string LabelCheckInStatus { get; set; }
        string LabelCheckOutStatus { get; set; }
    }
}