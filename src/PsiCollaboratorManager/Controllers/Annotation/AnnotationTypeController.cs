using AutoMapper;
using PsiCollaborator.Data.Annotation.AnnotationType;
using PsiCollaboratorManager.Models.Annotation;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PsiCollaboratorManager.Controllers
{
    public class AnnotationTypeController : Controller
    {
        private IAnnotationTypeRepository _annotationTypeRepository;
        private IMapper _mapper;
        public AnnotationTypeController()
        {
            _annotationTypeRepository = new AnnotationTypeRepository();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnnotationType, AnnotationTypeModel>();
                cfg.CreateMap<AnnotationTypeModel, AnnotationType>();
            });
            _mapper = configuration.CreateMapper();
        }
        public ActionResult Index()
        {
            ViewBag.BasicTitle = "Tipos de Anotación";
            return View();
        }
        public ActionResult GetAll()
        {
            List<AnnotationType> annotationTypes = _annotationTypeRepository.GetAll();
            List<AnnotationTypeModel> annotationTypesModels = _mapper.Map<List<AnnotationTypeModel>>(annotationTypes);
            return Json(new { rows = annotationTypesModels }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            ViewBag.BasicTitle = "Crear Tipo de Anotación";    
            return View();
        }
        [HttpPost]
        public ActionResult Save(AnnotationTypeModel annotationTypeModel)
        {
            try
            {
                AnnotationType annotationType = _mapper.Map<AnnotationType>(annotationTypeModel);
                _annotationTypeRepository.Save(annotationType);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int annotationTypeId)
        {
            ViewBag.BasicTitle = "Editar Tipo de Anotación";
            AnnotationType annotationType = _annotationTypeRepository.GetById(annotationTypeId);
            AnnotationTypeModel annotationTypeModel = _mapper.Map<AnnotationTypeModel>(annotationType);
            return View(annotationTypeModel);
        }
    }
}
