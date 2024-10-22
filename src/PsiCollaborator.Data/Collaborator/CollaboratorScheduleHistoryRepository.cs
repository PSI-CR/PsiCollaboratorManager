using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Collaborator
{
    public class CollaboratorScheduleHistoryRepository : DbMapper, ICollaboratorScheduleHistoryRepository
    {
        public List<CollaboratorScheduleHistory> GetScheduleHistoryByCollaboratorId(int collaboratorId)
        {
            return ExecuteListWithParameters<CollaboratorScheduleHistory>("select_schedule_history_by_collaborator_id",
            new List<DbParameter>() { new DbParameter("param_collaboratorid", ParameterDirection.Input, collaboratorId) }).ToList();
        }
    }
}
