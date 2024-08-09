using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Schedule
{
    public class WorkingDayDisplayModel
    {     
        public int WorkingDayId { get; set; }       
        public string Name { get; set; }     
        public string Description { get; set; }       
        public int MaxDays { get; set; }       
        public int MaxHours { get; set; }        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }     
        public bool Accumulative { get; set; }
        public string Assigned { get; set; }
        public DateTime RecordTime { get; set; }    
    }
}