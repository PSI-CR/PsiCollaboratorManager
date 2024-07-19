using AutoMapper;
using PsiCollaborator.Data.AnnouncementArt;
using PsiCollaborator.Data.Collaborator;
using PsiCollaboratorManager.Mapping;
using PsiCollaboratorManager.Models.Collaborator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace PsiCollaboratorManager.Controllers
{
    public class CollaboratorController : Controller
    {
        private ICollaboratorRepository _collaboratorRepository;
        private CollaboratorMapper _collaboratorMapper;
        public CollaboratorController()
        {
            _collaboratorRepository = new CollaboratorRepository();
            _collaboratorMapper = new CollaboratorMapper();
        }
        public ActionResult GetDetails(int collaboratorId){
            CollaboratorDetails collaboratorDetails = _collaboratorRepository.GetDetailsById(collaboratorId);
            CollaboratorDetailsModel collaboratorModel = _collaboratorMapper.Map(collaboratorDetails);
            return Json(collaboratorModel, JsonRequestBehavior.AllowGet);
        }
    }
}