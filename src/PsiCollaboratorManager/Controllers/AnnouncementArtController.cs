using AutoMapper;
using PsiCollaborator.Data.AnnouncementArt;
using PsiCollaborator.Data.Collaborator;
using PsiCollaboratorManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PsiCollaboratorManager.Controllers
{
    public class AnnouncementArtController : Controller
    {
        private IAnnouncementArtRepository _announcementArtRepository;
        private IMapper _mapper;

        public AnnouncementArtController()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnnouncementArt, AnnouncementArtModel>();
                cfg.CreateMap<AnnouncementArtModel, AnnouncementArt>();
            });
            _announcementArtRepository = new AnnouncementArtRepository(); 
            _mapper = configuration.CreateMapper();
        }

        public ActionResult Index()
        {
            ViewBag.BasicTitle = "Anuncios";
            IEnumerable<IAnnouncementArt> announcementArts = _announcementArtRepository.GetAll();
            List<AnnouncementArtModel> announcementArtModel = _mapper.Map<List<AnnouncementArtModel>>(announcementArts);
            return View(announcementArtModel);
        }

        public ActionResult Create()
        {
            ViewBag.BasicTitle = "Cargar Anuncio";
            return View();
        }

        [HttpPost]
        public ActionResult Create(AnnouncementArtModel announcementArtModel)
        {
            try
            {
                AnnouncementArt announcementArt = _mapper.Map<AnnouncementArt>(announcementArtModel);
                _announcementArtRepository.Save(announcementArt);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int announcementArtId)
        {
            ViewBag.BasicTitle = "Editar Anuncio";
            IAnnouncementArt announcementArt = _announcementArtRepository.GetById(announcementArtId);
            AnnouncementArtModel announcementArtModel = _mapper.Map<AnnouncementArtModel>(announcementArt);
            return View(announcementArtModel);
        }

        [HttpPost]
        public ActionResult Edit(AnnouncementArtModel announcementArtModel)
        {
            try
            {
                AnnouncementArt announcementArt = _mapper.Map<AnnouncementArt>(announcementArtModel);
                _announcementArtRepository.Save(announcementArt);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int announcementArtId)
        {
            try
            {
                _announcementArtRepository.Delete(announcementArtId);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}