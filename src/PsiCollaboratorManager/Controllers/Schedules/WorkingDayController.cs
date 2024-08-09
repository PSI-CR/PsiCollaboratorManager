using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using PsiCollaborator.Data.Schedule.WorkingDay;
using PsiCollaboratorManager.Mapping;
using PsiCollaboratorManager.Models.Schedule;

namespace PsiCollaboratorManager.Controllers
{
    public class WorkingDayController : Controller
    {
        private IWorkingDayRepository _workingDayRepository;
        private ScheduleMapper _scheduleMapper;
        private IMapper _mapper;

        public WorkingDayController()
        {      
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkingDay, WorkingDayModel>();
                cfg.CreateMap<WorkingDayModel, WorkingDay>();
            });

            _workingDayRepository = new WorkingDayRepository();
            _scheduleMapper = new ScheduleMapper();
            _mapper = configuration.CreateMapper();
        }

        public ActionResult Index()
        {
            ViewBag.BasicTitle = "Jornada Laboral";
            return View();
        }

        public ActionResult GetAllWorkingDay()
        {
            List<WorkingDay> workingDays = _workingDayRepository.GetAll();
            List<WorkingDayDisplayModel> models = workingDays.Select(workingDay => _scheduleMapper.MapDisplay(workingDay)).ToList();
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.BasicTitle = "Crear Jornada Laboral";
            return View();
        }

        [HttpPost]
        public ActionResult Create(WorkingDayModel workingDayModel)
        {
            WorkingDay workingDayData = _scheduleMapper.Map(workingDayModel);
            _workingDayRepository.Insert(workingDayData);
            return RedirectToAction("../WorkingDay/Index");
        }

        [HttpPost]
        public JsonResult Delete(int workingDayId)
        {
            try
            {
                _workingDayRepository.Delete(workingDayId);
                return Json(new { success = true, message = "Los datos se han eliminado exitosamente." });
            }
            catch
            {
                return Json(new { success = false, message = "Esta jornada esta asignado algun horario." });
            }
        }
    }
}

