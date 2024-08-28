using AutoMapper;
using PsiCollaborator.Data.Attend;
using PsiCollaborator.Data.Collaborator;
using PsiCollaborator.Data.Schedule;
using PsiCollaborator.Data.Schedule.ScheduleDay;
using PsiCollaborator.Data.Schedule.WorkingDay;
using PsiCollaboratorManager.Mapping;
using PsiCollaboratorManager.Models.Schedule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PsiCollaboratorManager.Controllers
{
    public class ScheduleController : Controller
    {
        private IScheduleRepository _scheduleRepository;
        private IWorkingDayRepository _workingDayRepository;
        private IScheduleDayRepository _scheduleDayRepository;
        private ICollaboratorRepository _collaboratorRepository;
        private IAttendRepository _attendRepository;
        private IMapper _mapper;
        private ScheduleMapper _scheduleMapper;
        private AttendanceMapper _attendMapper;
        
        public ScheduleController() 
        {          
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ScheduleData, ScheduleDataModel>();
                cfg.CreateMap<ScheduleDataModel, ScheduleData>();
            });

            _scheduleRepository = new ScheduleRepository();
            _scheduleDayRepository = new ScheduleDayRepository();
            _scheduleMapper = new ScheduleMapper();
            _workingDayRepository = new WorkingDayRepository();
            _collaboratorRepository = new CollaboratorRepository();
            _attendRepository = new AttendRepository();
            _attendMapper = new AttendanceMapper();
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
                List<ScheduleDataModel> models = schedules.Select(schedule => _scheduleMapper.Map(schedule)).ToList();
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "Error interno del servidor");
            }
        }

        public ActionResult Create() 
        {
            ViewBag.BasicTitle = "Crear Horario";
            List<ScheduleData> datas = _scheduleRepository.GetAllScheduleWorking();
            SetDays(2);
            SetWorkingDay();
            return View();
        }

        [HttpPost]
        public JsonResult Save(string schedule)
        {
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                ScheduleModel scheduleModel = js.Deserialize<ScheduleModel>(schedule);
                Schedule scheduleData = _scheduleMapper.MapScheduleModelToSchedule(scheduleModel);
                _scheduleRepository.Save(scheduleData);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al guardar el horario: " + ex.Message });
            }
        }

        public ActionResult Edit(int scheduleId)
        {
            ViewBag.BasicTitle = "Editar Horario";
            var schedule = _scheduleRepository.GetById(scheduleId);
            var workingDay = _workingDayRepository.GetById(schedule.WorkingdayId);
            var scheduleModel = _scheduleMapper.MapScheduleToModel(schedule, workingDay);
            SetDays(2);
            SetWorkingDay();
            return View(scheduleModel);
        }

        public void SetDays(int daysId)
        {
            List<ScheduleDay> days = _scheduleDayRepository.GetAll();
            List<SelectListItem> daysSelect = days.Select(x => new SelectListItem() { Value = x.DayId.ToString(), Text = x.DayName, Selected = x.DayId == daysId }).ToList();
            ViewData["Days"] = daysSelect;
        }

        public void SetWorkingDay()
        {
            List<WorkingDay> workingDays = _workingDayRepository.GetAll();
            List<SelectListItem> workingDaySelect = workingDays.Select(workingDay => new SelectListItem { Text = workingDay.Name, Value = workingDay.WorkingDayId.ToString() }).ToList();
            ViewData["Working"] = workingDaySelect;
        }

        [HttpPost]
        public JsonResult GetWorkingDayData(int workingDayId)
        {
            WorkingDay workingDay = _workingDayRepository.GetById(workingDayId);
            WorkingDayModel workingDayModel = _scheduleMapper.Map(workingDay);
            return Json(new { success = true, data = workingDayModel });
        }

        public ActionResult GetScheduleData() 
        {
            return View();       
        }

        public ActionResult CheckSchedule()
        {
            ViewBag.BasicTitle = "Revisión Horarios";
            return View();
        }

        //CheckAttendance
        public ActionResult GetCollaborator()
        {
            List<CollaboratorOperator> collaborators = _collaboratorRepository.GetAllOperator();  
            return Json(new { rows = collaborators }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAssistanceByCollaborator(int collaboratorId)
        {
            CollaboratorPicture collaboratorPictureData = _collaboratorRepository.GetCollaboratorPictureById(collaboratorId);
            List<Attend> attendance = _attendRepository.GetAttendByCollaboratorId(collaboratorId);
            var result = _attendMapper.MapToCollaboratorPictureModel(collaboratorPictureData, attendance);
            return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditAssistance(Attendance attendData) 
        {
            try
            {
                _attendRepository.Update(attendData);
                return Json(new { success = true, message = "Datos guardados exitosamente" });
            }catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al guardar los datos", error = ex.Message });
            }
           
        }    
    }
}