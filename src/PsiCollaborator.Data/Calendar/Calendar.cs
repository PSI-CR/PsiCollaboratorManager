using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Calendar
{
    public class Calendar: ICalendar
    {
        public int Event_Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }     
        public bool AllDay { get; set; }
        //public List<int> CollaboratorId { get; set; }
        public string Color { get; set; }
    }
}
