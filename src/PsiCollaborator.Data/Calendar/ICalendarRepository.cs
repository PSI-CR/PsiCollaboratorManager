using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Calendar
{
    public interface ICalendarRepository
    {
        void Save(Calendar calendarEvent, List<int> collaboratorId);
        Calendar GetById(int id);
        List<Calendar> GetAll();
        void Delete(int id);
        void Update(Calendar calendarEvent);
        List<CollaboratorByEvent> GetCollaboratorByEventId(int id);
    }
}
