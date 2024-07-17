using System;

namespace PsiCollaborator.Data.Attend
{
    public interface IAttend
    {
        int AttendId { get; set; }
        DateTime CheckIn { get; set; }
        int CheckInStatus { get; set; }
        string CheckInStatusWork { get; set; }
        DateTime? CheckOut { get; set; }
        int CheckOutStatus { get; set; }
        string CheckOutStatusWork { get; set; }
        int CollaboratorId { get; set; }
        string CommentCheckIn { get; set; }
        string IpAddress { get; set; }
        bool IsOpenCheckIn { get; set; }
        string PhysicalAddressEquipment { get; set; }
    }
}