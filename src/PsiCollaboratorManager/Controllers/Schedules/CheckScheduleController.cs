using AutoMapper;
using PsiCollaborator.Data.Schedule.WorkingDay;
using PsiCollaboratorManager.Mapping;
using PsiCollaboratorManager.Models.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PsiCollaboratorManager.Controllers.Schedules
{
    public class CheckScheduleController : Controller
    {
        private IWorkingDayRepository _workingDayRepository;

        public CheckScheduleController()
        {
            _workingDayRepository = new WorkingDayRepository();
        }

        public ActionResult GetWorkingDay(int collaboratorId) 
        {
            WorkingDay workingDayData = _workingDayRepository.GetByCollaboratorId(collaboratorId);
            return Json(new { success = true, data = workingDayData }, JsonRequestBehavior.AllowGet);
        }
    }
}