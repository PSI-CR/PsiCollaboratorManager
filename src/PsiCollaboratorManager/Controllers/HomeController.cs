using AutoMapper;
using PsiCollaborator.Data.AnnouncementArt;
using PsiCollaborator.Data.Collaborator;
using PsiCollaboratorManager.Models.Collaborator;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PsiCollaboratorManager.Controllers
{
    public class HomeController : Controller
    {
        private ICollaboratorRepository _collaboratorRepository;
        private IAnnouncementArtRepository _announcementArtRepository;
        private IMapper _mapper;
        public HomeController() {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CollaboratorBirthday, CollaboratorBirthdayModel>();
                cfg.CreateMap<CollaboratorBirthdayModel, CollaboratorBirthday>();
            });
            _collaboratorRepository = new CollaboratorRepository();
            _announcementArtRepository = new AnnouncementArtRepository();
            _mapper = configuration.CreateMapper();
        }
        public ActionResult Index()
        {
            ViewBag.DoubleTitleLeft = "Información";
            ViewBag.DoubleTitleRight = "Cumpleaños";
            return View();
        }
        public ActionResult GetBirthdayLastWeek()
        {
            List<CollaboratorBirthday> collaborators = _collaboratorRepository.GetLastWeekBirthday();
            List<CollaboratorBirthdayModel> collaboratorModels = _mapper.Map<List<CollaboratorBirthdayModel>>(collaborators);
            return Json(new { rows = collaboratorModels }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBirthdayToday()
        {
            List<CollaboratorBirthday> collaborators = _collaboratorRepository.GetTodayBirthday();
            List<CollaboratorBirthdayModel> collaboratorModels = _mapper.Map<List<CollaboratorBirthdayModel>>(collaborators);
            return Json(new { rows = collaboratorModels }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBirthdayNextWeek()
        {
            List<CollaboratorBirthday> collaborators = _collaboratorRepository.GetNextWeekBirthday();
            List<CollaboratorBirthdayModel> collaboratorModels = _mapper.Map<List<CollaboratorBirthdayModel>>(collaborators);
            return Json(new { rows = collaboratorModels }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCarouselImages()
        {
            List<AnnouncementArt> announcementArts = _announcementArtRepository.GetByDate();
            List<string> base64Images = announcementArts.Select(announcementArt => announcementArt.Image).ToList();
            return Json(base64Images, JsonRequestBehavior.AllowGet);
        }
    }
}