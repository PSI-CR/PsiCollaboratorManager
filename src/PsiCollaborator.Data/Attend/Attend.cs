using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Attend
{
    public class Attend : IAttend
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
