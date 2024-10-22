using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorScheduleHistoryRepository
    {
        List<CollaboratorScheduleHistory> GetScheduleHistoryByCollaboratorId(int collaboratorId);
    }
}
