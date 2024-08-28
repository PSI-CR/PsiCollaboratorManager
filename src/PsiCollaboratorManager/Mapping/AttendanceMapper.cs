using PsiCollaborator.Data.Attend;
using PsiCollaborator.Data.Collaborator;
using PsiCollaboratorManager.Models.Attendance;
using PsiCollaboratorManager.Models.Collaborator;
using System.Collections.Generic;
using System.Linq;

namespace PsiCollaboratorManager.Mapping
{
    public class AttendanceMapper
    { 
        public CollaboratorPictureModel MapToCollaboratorPictureModel(CollaboratorPicture collaborator, List<Attend> attendance)
        {
            return new CollaboratorPictureModel
            {
                CollaboratorId = collaborator.CollaboratorId,
                Firstname = collaborator.Firstname,
                Lastname = collaborator.Lastname,
                Picture = "data:image/png;base64," + collaborator.Picture,
                AttendModels = attendance.Select(a => new AttendModel
                {
                    AttendId = a.AttendanceId,
                    CheckIn = a.CheckIn.ToString("yyyy/MM/dd HH:mm:ss"),
                    CheckOut = a.CheckOut?.ToString("yyyy/MM/dd HH:mm:ss"),
                    CheckInStatus = a.CheckInStatus,
                    CheckOutStatus = a.CheckOutStatus,
                    CommentCheckIn = a.CommentCheckIn,
                    IsOpenCheckIn = a.IsOpenCheckIn,
                    IpAddress = a.IpAddress,
                    PhysicalAddressEquipment = a.PhysicalAddressEquipment,
                    CheckInStatusWork = a.LabelCheckInStatus,
                    CheckOutStatusWork = a.LabelCheckOutStatus
                }).ToList()
            };
        }

    }
}