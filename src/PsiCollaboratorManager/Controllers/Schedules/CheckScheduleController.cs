using PsiCollaborator.Data.Schedule.ScheduleDaily;
using PsiCollaborator.Data.Schedule.WorkingDay;
using System.Web.Mvc;

namespace PsiCollaboratorManager.Controllers.Schedules
{
    public class CheckScheduleController : Controller
    {
        private IWorkingDayRepository _workingDayRepository;
        private IScheduleDailyRepository _scheduleDailyRepository;

        public CheckScheduleController() 
        {
            _workingDayRepository = new WorkingDayRepository();   
            _scheduleDailyRepository = new ScheduleDailyRepository();   
        }

        // GET: CheckSchedule
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetWorkingDay(int collaboratorId)
        {
            var workingDay = _workingDayRepository.GetByCollaboratorId(collaboratorId);
            return Json(new { success = true, data = workingDay });
        }

        [HttpPost]
        public JsonResult GetScheduleDaily(int collaboratorId)
        {
            var scheduleDailys = _scheduleDailyRepository.GetByCollaboratorId(collaboratorId);
            return Json(new { success = true, data = scheduleDailys});
        }

        [HttpPost]
        public JsonResult CalculateSchedule(int totalMinutes, int dayId, int collaboratorId)
        {
            var result = _scheduleDailyRepository.GetByScheduleCalculate(dayId, totalMinutes, collaboratorId);
            return Json(new { success = true, data = result });
        }
    }
}