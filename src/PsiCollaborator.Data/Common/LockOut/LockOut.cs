using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data
{
    public class LockOut : ILockOut
    {
        public bool IsLockedOut { get; set; }
        public DateTime? LockOutEndTime { get; set; }
    }
}
