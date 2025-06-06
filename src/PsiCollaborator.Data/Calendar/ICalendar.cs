using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Calendar
{
    public interface ICalendar
    {
        int Event_Id { get; set; }
        string Title { get; set; }
        DateTime Start { get; set; }
        DateTime End { get; set; }
        string Description { get; set; }
        bool AllDay { get; set; }
        //List<int> CollaboratorId { get; set; }
        string Color { get; set; }
    }
}