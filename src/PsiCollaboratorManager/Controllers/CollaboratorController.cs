using AutoMapper;
using PsiCollaborator.Data.Address;
using PsiCollaborator.Data.BankAccount;
using PsiCollaborator.Data.Collaborator;
using PsiCollaborator.Data.PasswordUtilities;
using PsiCollaboratorManager.Annotations;
using PsiCollaboratorManager.Mapping;
using PsiCollaboratorManager.Models.Collaborator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PsiCollaboratorManager.Controllers
{
    [CheckSessionTimeOut]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class CollaboratorController : Controller
    {
        private ICollaboratorRepository _collaboratorRepository;
        private IBankAccountRepository _bankAccountRepository;
        private IAddressRepository _addressRepository;
        private CollaboratorMapper _collaboratorMapper;
        private IMapper _mapper;
        public CollaboratorController()
        {
            _collaboratorRepository = new CollaboratorRepository();
            _bankAccountRepository = new BankAccountRepository();
            _addressRepository = new AddressRepository();
            _collaboratorMapper = new CollaboratorMapper();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CollaboratorBase, CollaboratorBaseModel>();
                cfg.CreateMap<CollaboratorBaseModel, CollaboratorBase>();

                cfg.CreateMap<Canton, CantonModel>();
                cfg.CreateMap<CantonModel, Canton>();

                cfg.CreateMap<District, DistrictModel>();
                cfg.CreateMap<DistrictModel, District>();
            });
            _mapper = configuration.CreateMapper();
        }
        public ActionResult GetDetails(int collaboratorId){
            CollaboratorDetails collaboratorDetails = _collaboratorRepository.GetDetailsById(collaboratorId);
            CollaboratorDetailsModel collaboratorModel = _collaboratorMapper.Map(collaboratorDetails);
            return Json(collaboratorModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            ViewBag.BasicTitle = "Colaboradores Activos";
            return View();
        }
        public ActionResult GetAllActive()
        {
            List<CollaboratorBase> collaborators = _collaboratorRepository.GetByIsActive(true);
            List<CollaboratorBaseModel> collaboratorModels = collaborators.Select(x => _collaboratorMapper.Map(x)).ToList();
            return Json(new { rows = collaboratorModels }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InactiveCollaborators()
        {
            ViewBag.BasicTitle = "Colaboradores Inactivos";
            return View();
        }
        public ActionResult GetAllInactive()
        {
            List<CollaboratorBase> collaborators = _collaboratorRepository.GetByIsActive(false);
            List<CollaboratorBaseModel> collaboratorModels = collaborators.Select(x => _collaboratorMapper.Map(x)).ToList();
            return Json(new { rows = collaboratorModels }, JsonRequestBehavior.AllowGet);
        }

        private void setBanks(int bankId)
        {
            List<Bank> banks = _bankAccountRepository.GetAllBank();
            List<SelectListItem> banksSelect = banks.Select(x => new SelectListItem() { Value = x.BankId.ToString(), Text = x.Acronym, Selected = x.BankId == bankId }).ToList();
            ViewData["Banks"] = banksSelect;
        }
        private void setCurrencyTypes(int currencyTypeId)
        {
            List<CurrencyType> currencyTypes = _bankAccountRepository.GetAllCurrencyType();
            List<SelectListItem> currencyTypesSelect = currencyTypes.Select(x => new SelectListItem() { Value = x.CurrencyTypeId.ToString(), Text = x.Name, Selected = x.CurrencyTypeId == currencyTypeId }).ToList();
            ViewData["CurrencyTypes"] = currencyTypesSelect;
        }
        private void setProvinces(int provinceId)
        {
            List<Province> provinces = _addressRepository.GetAllProvinces();
            List<SelectListItem> provincesSelect = provinces.Select(x => new SelectListItem() { Value = x.ProvinceId.ToString(), Text = x.Name, Selected = x.ProvinceId == provinceId }).ToList();
            ViewData["Provinces"] = provincesSelect;
        }
        private void setCantons(int cantonId, int provinceId = 1)
        {
            List<Canton> cantons = _addressRepository.GetAllCantonsByProvince(provinceId);
            List<SelectListItem> cantonsSelect = cantons.Select(x => new SelectListItem() { Value = x.CantonId.ToString(), Text = x.Name, Selected = x.CantonId == cantonId }).ToList();
            ViewData["Cantons"] = cantonsSelect;
        }
        private void setDistricts(int districtId, int cantonId = 101)
        {
            List<District> districts = _addressRepository.GetAllDistrictsByCanton(cantonId);
            List<SelectListItem> districtsSelect = districts.Select(x => new SelectListItem() { Value = x.DistrictId.ToString(), Text = x.Name, Selected = x.DistrictId == districtId }).ToList();
            ViewData["Districts"] = districtsSelect;
        }
        private void setRelationships()
        {
            List<RelationshipEnum> relationships = Enum.GetValues(typeof(RelationshipEnum)).Cast<RelationshipEnum>().ToList();
            List<SelectListItem> relationshipsSelect = relationships.Select(x => new SelectListItem() { Value = ((int)x).ToString(), Text = x.ToString() }).ToList();
            ViewData["Relationships"] = relationshipsSelect;
        }
        private string generateTemporaryPassword()
        {
            return Guid.NewGuid().ToString("N").ToLower() .Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 5);
        }
        private void setDropdownLists(int bankId, int currencyTypeId, int provinceId, int cantonId, int districtId)
        {
            setBanks(bankId);
            setCurrencyTypes(currencyTypeId);
            setProvinces(provinceId);
            setCantons(cantonId, provinceId);
            setDistricts(districtId, cantonId);
            setRelationships();
        }
        private CollaboratorFullModel setDefaultValues(int highestOperatorNumber)
        {
            CollaboratorFullModel model = new CollaboratorFullModel();
            model.CollaboratorId = 0;
            model.IsActive = true;
            model.OperatorNumber = highestOperatorNumber;
            model.CreateUserAccount = true;
            model.Password = generateTemporaryPassword();
            model.Gender = 2;
            model.MaritalStatus = false;
            model.Parent = false;
            model.DateOfBirth = null;
            return model;
        }
        public JsonResult GetCantonsByProvince(int provinceId)
        {
            List<Canton> cantons = _addressRepository.GetAllCantonsByProvince(provinceId);
            List<CantonModel> cantonModels = _mapper.Map<List<CantonModel>>(cantons);
            return Json(cantonModels, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDistrictsByCanton(int cantonId)
        {
            List<District> districts = _addressRepository.GetAllDistrictsByCanton(cantonId);
            List<DistrictModel> districtModels = _mapper.Map<List<DistrictModel>>(districts);
            return Json(districtModels, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            ViewBag.BigTitle = "Agregar Colaborador";
            int highestOperatorNumber = _collaboratorRepository.GetHighestOperatorNumber() + 1;
            CollaboratorFullModel model = setDefaultValues(highestOperatorNumber);
            setDropdownLists(1,1,1,101,10101);
            ViewBag.ProfileImage = Url.Content("~/Images/DefaultCollaborator.jpg");
            return View(model);
        }
        public ActionResult Edit(int collaboratorId)
        {
            ViewBag.BigTitle = "Editar Colaborador";
            CollaboratorFull collaborator = _collaboratorRepository.GetFullById(collaboratorId);
            CollaboratorFullModel collaboratorModel = _collaboratorMapper.Map(collaborator);
            ViewBag.ProfileImage = collaboratorModel.Picture.StartsWith("data:image/jpeg;base64,") ? collaboratorModel.Picture : "data:image/jpeg;base64," + collaboratorModel.Picture; 
            setDropdownLists(collaboratorModel.BankId, collaboratorModel.CurrencyTypeId, collaboratorModel.ProvinceId, collaboratorModel.CantonId, collaboratorModel.DistrictId);
            collaboratorModel.Password = generateTemporaryPassword();
            return View(collaboratorModel);
        }
        public JsonResult Save(string collaborator)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string errorMessage = "";
            try
            {
                CollaboratorFullModel collaboratorModel = js.Deserialize<CollaboratorFullModel>(collaborator);
                errorMessage += checkUniqueness(collaboratorModel);
                if(errorMessage != "") return Json(errorMessage, JsonRequestBehavior.AllowGet);

                if (collaboratorModel.CreateUserAccount && collaboratorModel.Password != null)
                {
                    collaboratorModel.NeedPasswordChange = true;
                    collaboratorModel.Password = PBKDF2Converter.GetHashPassword(collaboratorModel.Password);
                }
                else
                {
                    collaboratorModel.CreateUserAccount = false;
                    collaboratorModel.Password = "";
                }
                CollaboratorFull dbCollaborator = _collaboratorMapper.Map(collaboratorModel);
                _collaboratorRepository.Save(dbCollaborator);
            }
            catch (Exception ex)
            {
                errorMessage += ex.Message;
            }

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
        private string checkUniqueness(CollaboratorFullModel collaboratorModel)
        {
            string errors = "";
            List<CollaboratorUniqueData> uniqueData = _collaboratorRepository.GetCollaboratorUniqueData();
            var (dniFound, telephoneFound, emailFound, operatorNumberFound) = (false, false, false, false);
            foreach (CollaboratorUniqueData collaboratorUniqueData in uniqueData)
            {
                if ((collaboratorModel.DNICollaborator == collaboratorUniqueData.DNICollaborator) && (collaboratorModel.CollaboratorId != collaboratorUniqueData.CollaboratorId) && !dniFound)
                {
                    dniFound = true;
                    errors += "La cédula ya está asignada a otro colaborador.\n";
                }
                if ((collaboratorModel.Telephone1 == collaboratorUniqueData.Telephone1) && (collaboratorModel.CollaboratorId != collaboratorUniqueData.CollaboratorId) && !telephoneFound)
                {
                    telephoneFound = true;
                    errors += "El telefono ya está asignado a otro colaborador.\n";
                }
                if ((collaboratorModel.Email == collaboratorUniqueData.Email) && (collaboratorModel.CollaboratorId != collaboratorUniqueData.CollaboratorId) && !emailFound)
                {
                    emailFound = true;
                    errors += "El correo ya está asignado a otro colaborador.\n";
                }
                if ((collaboratorModel.OperatorNumber == collaboratorUniqueData.OperatorNumber) && (collaboratorModel.CollaboratorId != collaboratorUniqueData.CollaboratorId) && !operatorNumberFound)
                {
                    dniFound = true;
                    errors += "El numero de operador ya está asignado a otro colaborador.\n";
                }
            }
            return errors;
        }
        public JsonResult Delete(int collaboratorId)
        {
            try {
                _collaboratorRepository.Delete(collaboratorId);
                return Json(new { success = true });
            }
            catch {
                return Json(new { success = false });
            }
        }
        public ActionResult AssignSchedule()
        {
            ViewBag.BasicTitle = "Asignar Horario";
            return View();
        }
    }
}