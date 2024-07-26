using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using PsiCollaborator.Data.Schedule.WorkingDay;
using PsiCollaboratorManager.Models.Schedule;

namespace PsiCollaboratorManager.Controllers
{
    public class WorkingDayController : Controller
    {
        private IWorkingDayRepository _workingDayRepository;
        private IMapper _mapper;

        public WorkingDayController()
        {      
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkingDay, WorkingDayModel>();
                cfg.CreateMap<WorkingDayModel, WorkingDay>();
            });

            _workingDayRepository = new WorkingDayRepository();
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
            var gridData = workingDays.Select(workingDay => new
            {
                workingDay.WorkingDayId,
                workingDay.Name,
                workingDay.Description,
                workingDay.MaxDays,
                workingDay.MaxHours,
                workingDay.StartTime,
                workingDay.EndTime,
                workingDay.RecordTime,
                Accumulative = workingDay.Accumulative ? "Si" : "No"
            });
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.BasicTitle = "Crear Jornada Laboral";
            return View();
        }

        [HttpPost]
        public ActionResult Create(WorkingDayModel workingDayModel)
        {
            var workingDayData = new WorkingDay()
            {
                WorkingDayId = workingDayModel.WorkingDayId,
                Name = workingDayModel.Name,
                Description = workingDayModel.Description,
                MaxDays = workingDayModel.MaxDays,
                MaxHours = workingDayModel.MaxHours,
                StartTime = workingDayModel.StartTime,
                EndTime = workingDayModel.EndTime,
                RecordTime = DateTime.Now,
                Accumulative = workingDayModel.Accumulative
            };
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

