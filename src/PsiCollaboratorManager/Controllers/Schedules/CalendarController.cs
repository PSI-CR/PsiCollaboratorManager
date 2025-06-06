using AutoMapper;
using PsiCollaborator.Data.Calendar;
using PsiCollaborator.Data.Collaborator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PsiCollaboratorManager.Models.Schedule
{
    public class CalendarController : Controller
    {
        private ICalendarRepository _calendarRepository;
        private IMapper _mapper;

        public CalendarController()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Calendar, CalendarModel>();
                cfg.CreateMap<CalendarModel, Calendar>();
            });

            _calendarRepository = new CalendarRepository();
            _mapper = configuration.CreateMapper();
        }

        // GET: Calendar
        public ActionResult Index()
        {
            ViewBag.BasicTitle = "Horarios";
            List<Calendar> events = _calendarRepository.GetAll();
            return View();
        }

        public JsonResult GetEvents()
        {
            var events = _calendarRepository.GetAll();

            var result = events.Select(e => new {
                id = e.Event_Id,
                title = e.Title,
                start = e.Start.ToString("s"),
                end = e.End.ToString("s"),
                allDay = e.AllDay,
                color = e.Color,
                collaboratorIds = _calendarRepository
                .GetCollaboratorByEventId(e.Event_Id)
                .Select(c => c.Collaborator_Id)
                .ToList()
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEventById(int id)
        {
            var calendarEvent = _calendarRepository.GetById(id);
            if (calendarEvent == null)
            {
                return Json(new { success = false, message = "Evento no encontrado." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    id = calendarEvent.Event_Id,
                    title = calendarEvent.Title,
                    start = calendarEvent.Start.ToString("s"),
                    end = calendarEvent.End.ToString("s"),
                    description = calendarEvent.Description,
                    allDay = calendarEvent.AllDay,
                    color = calendarEvent.Color,
                }
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Details(int id)
        {
            Calendar calendarEvent = _calendarRepository.GetById(id);
            if (calendarEvent == null)
            {
                return HttpNotFound();
            }
            return View(calendarEvent);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Calendar calendarEvent)
        {
            if (ModelState.IsValid)
            {
                //_calendarRepository.Save(calendarEvent);
                return RedirectToAction("Index");
            }

            return View(calendarEvent);
        }

        public ActionResult Edit(int id)
        {
            Calendar calendarEvent = _calendarRepository.GetById(id);
            if (calendarEvent == null)
            {
                return HttpNotFound();
            }
            return View(calendarEvent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Calendar calendarEvent)
        {
            if (id != calendarEvent.Event_Id)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                _calendarRepository.Update(calendarEvent);
                return RedirectToAction("Index");
            }
            return View(calendarEvent);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                _calendarRepository.Delete(id);

                return Json(new { success = true, message = "Evento eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Calendar/SaveEvent
        [HttpPost]
        public ActionResult SaveEvent(CalendarModel model)
        {
            if (ModelState.IsValid)
            {
                Calendar calendarEvent = new Calendar
                {
                    Title = model.Title,
                    Description = model.Description,
                    Start = model.StartDate,
                    End = model.EndDate,
                    AllDay = model.IsAllDay,
                    Color = model.Color
                };

                _calendarRepository.Save(calendarEvent, model.CollaboratorId);

                return Json(new { success = true, message = "Evento guardado exitosamente" });
            }

            return Json(new { success = false, message = "Ocurrió un error al guardar el evento" });
        }

        [HttpPost]
        public JsonResult UpdateEvent(CalendarModel model)
        {

            if (ModelState.IsValid)
            {
                Calendar calendarEvent = new Calendar
                {
                    Event_Id = model.Event_Id,
                    Title = model.Title,
                    Description = model.Description,
                    Start = model.StartDate,
                    End = model.EndDate,
                    AllDay = model.IsAllDay,
                    Color = model.Color
                };

                _calendarRepository.Update(calendarEvent);

                return Json(new { success = true, message = "Evento actualizado exitosamente" });
            }

            return Json(new { success = false, message = "Ocurrió un error al actualizar el evento" });
        }
    }
}