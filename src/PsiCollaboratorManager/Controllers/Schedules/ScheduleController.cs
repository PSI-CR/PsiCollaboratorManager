using AutoMapper;
using PsiCollaborator.Data.BankAccount;
using PsiCollaborator.Data.Schedule;
using PsiCollaborator.Data.Schedule.ScheduleDay;
using PsiCollaborator.Data.Schedule.WorkingDay;
using PsiCollaboratorManager.Models.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PsiCollaboratorManager.Controllers
{
    public class ScheduleController : Controller
    {
        private IScheduleRepository _scheduleRepository;
        private IWorkingDayRepository _workingDayRepository;
        private IScheduleDayRepository _scheduleDayRepository;
        private IMapper _mapper;
        
        public ScheduleController() 
        {          
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ScheduleData, ScheduleModel>();
                cfg.CreateMap<ScheduleModel, ScheduleData>();
            });

            _scheduleRepository = new ScheduleRepository();
            _scheduleDayRepository = new ScheduleDayRepository();
            _mapper = configuration.CreateMapper();
        }

        public ActionResult Index()
        {
            ViewBag.BasicTitle = "Horarios";
            return View();
        }

        public ActionResult GetAllSchedules()
        {
            try
            {
                List<ScheduleData> schedules = _scheduleRepository.GetAllScheduleWorking();
                return Json(schedules, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "Error interno del servidor");
            }
        }

        public ActionResult Edit(int scheduleId)
        {
            List<ScheduleData> datas = _scheduleRepository.GetAllScheduleWorking();
            SetDays(1);

            //List<WorkingDay> workingDays = _workingDayRepository.ListWorkingDay();
            //List<ScheduleDay> days = _scheduleDaysRepository.ListScheduleDays();

            //List<SelectListItem> workingDaysSelect = new List<SelectListItem>();
            //List<SelectListItem> daysSelect = new List<SelectListItem>();

            //foreach (var workingDay in workingDays)
            //{
            //    workingDaysSelect.Add(new SelectListItem { Text = workingDay.workingdayname, Value = workingDay.workingdayid.ToString() });
            //}

            //foreach (var day in days)
            //{
            //    daysSelect.Add(new SelectListItem { Text = day.dayname, Value = day.dayid.ToString() });
            //}

            //ViewBag.WorkingDay = workingDaysSelect;
            //ViewBag.Days = daysSelect;

            return View();
        }

        public void SetDays(int daysId)
        {
            List<ScheduleDay> days = _scheduleDayRepository.GetAll();
            List<SelectListItem> daysSelect = days.Select(x => new SelectListItem() { Value = x.ScheduleDayId.ToString(), Text = x.Name, Selected = x.ScheduleDayId == daysId }).ToList();
            ViewData["Days"] = daysSelect;
        }
    }
}