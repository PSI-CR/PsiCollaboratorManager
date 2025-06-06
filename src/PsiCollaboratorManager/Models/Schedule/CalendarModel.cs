using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Schedule
{
    public class CalendarModel
    {
        public int Event_Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsAllDay { get; set; }
        public List<int> CollaboratorId { get; set; }
        public string Color { get; set; }
    }
}