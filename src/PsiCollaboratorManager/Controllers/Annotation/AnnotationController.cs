using AutoMapper;
using Newtonsoft.Json;
using PsiCollaborator.Data.Address;
using PsiCollaborator.Data.Annotation;
using PsiCollaborator.Data.Annotation.AnnotationType;
using PsiCollaborator.Data.Collaborator;
using PsiCollaboratorManager.Mapping;
using PsiCollaboratorManager.Models.Annotation;
using PsiCollaboratorManager.Models.Collaborator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PsiCollaboratorManager.Controllers.Annotation
{
    public class AnnotationController : Controller
    {
        private IAnnotationRepository _annotationRepository;
        private IAnnotationTypeRepository _annotationTypeRepository;
        private ICollaboratorRepository _collaboratorRepository;
        private IMapper _mapper;
        private CollaboratorMapper _collaboratorMapper;
        private AnnotationMapper _annotationMapper;
        public AnnotationController() {
            _annotationRepository = new AnnotationRepository();
            _annotationTypeRepository = new AnnotationTypeRepository();
            _collaboratorRepository = new CollaboratorRepository();

            _collaboratorMapper = new CollaboratorMapper();
            _annotationMapper = new AnnotationMapper();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnnotationOverview, AnnotationOverviewModel>();
                cfg.CreateMap<AnnotationOverviewModel, AnnotationOverview>();

                cfg.CreateMap<AnnotationDetails, AnnotationDetailsModel>();
                cfg.CreateMap<AnnotationDetailsModel, AnnotationDetails>();

                cfg.CreateMap<CollaboratorOperator, CollaboratorOperatorModel>();
                cfg.CreateMap<CollaboratorOperatorModel, CollaboratorOperator>();
            });
            _mapper = configuration.CreateMapper();
        }
        public ActionResult Index()
        {
            ViewBag.BasicTitle = "Anotaciones";
            return View();
        }
        public ActionResult GetAll()
        {
            IEnumerable<IAnnotationOverview> annotations = _annotationRepository.GetAll(null, null, "","",0);
            List<AnnotationOverviewModel> annotationModels = _mapper.Map<List<AnnotationOverviewModel>>(annotations);
            return Json(new {rows = annotationModels}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchAnnotations(string name, string lastName, int opNumber, DateTime startDate, DateTime endDate)
        {
            try
            {
                IEnumerable<IAnnotationOverview> annotations = _annotationRepository.GetAll(startDate, endDate, name, lastName, opNumber);
                List<AnnotationOverviewModel> annotationModels = _mapper.Map<List<AnnotationOverviewModel>>(annotations);
                return Json(new { rows = annotationModels }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetAnnotationDetails(int annotationId)
        {
            IAnnotationDetails annotationDetails = _annotationRepository.GetAnnotationById(annotationId);
            AnnotationDetailsModel annotationDetailsModel = _mapper.Map<AnnotationDetailsModel>(annotationDetails);
            return Json(annotationDetailsModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewAnnotation()
        {
            ViewBag.BasicTitle = "Nueva Anotación";
            return View();
        }
        public ActionResult GetCollaborators()
        {
            IEnumerable<CollaboratorOperator> collaborators = _collaboratorRepository.GetAllOperator();
            List<CollaboratorOperatorModel> collaboratorOperatorModels = _mapper.Map<List<CollaboratorOperatorModel>>(collaborators);
            return Json(new { rows = collaboratorOperatorModels }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateAnnotation(string collaboratorsDictJson)
        {
            ViewBag.BasicTitle = "Asignar Anotaciones";
            List<string> collaboratorsDict = JsonConvert.DeserializeObject<List<string>>(collaboratorsDictJson);

            List<CollaboratorDetailsModel> collaborators = collaboratorsDict.Select(x => _collaboratorMapper.Map(_collaboratorRepository.GetDetailsById(Convert.ToInt32(x)))).ToList();

            List<AnnotationType> annotationTypes = _annotationTypeRepository.GetAll();
            ViewBag.AnnotationTypes = new SelectList(annotationTypes, "AnnotationTypeId", "TypeName");
            string userId = Session["UserAccount"].ToString();

            AnnotationModel annotationModel = new AnnotationModel { Collaborators = collaborators, UserId = 1, AnnotationDate = DateTime.Now };
            return View(annotationModel);
        }
        public JsonResult Create(AnnotationModel annotationModel, HttpPostedFileBase attachmentFile)
        {
            string errorMessage = "";
            if (annotationModel.Collaborators == null)
            {
                errorMessage += "Debe haber al menos un collaborador";
                return Json(errorMessage, JsonRequestBehavior.AllowGet);
            }
            if (attachmentFile != null && attachmentFile.ContentLength > 0)
            {
                using (var reader = new BinaryReader(attachmentFile.InputStream))
                {
                    annotationModel.FileData = Convert.ToBase64String(reader.ReadBytes(attachmentFile.ContentLength));
                    annotationModel.FileName = attachmentFile.FileName;
                    annotationModel.FileType = attachmentFile.ContentType;
                }
            }
            PsiCollaborator.Data.Annotation.Annotation annotation = _annotationMapper.Map(annotationModel);
            _annotationRepository.Insert(annotation);
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
    }
}