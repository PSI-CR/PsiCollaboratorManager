using AutoMapper;
using PsiCollaborator.Data;
using PsiCollaborator.Data.LoginAttemptPolicy;
using PsiCollaborator.Data.LoginAttemptUser;
using PsiCollaborator.Data.PasswordPolicy;
using PsiCollaborator.Data.PasswordUtilities;
using PsiCollaborator.Data.UserAccount;
using PsiCollaboratorManager.Mapping;
using PsiCollaboratorManager.Models;
using PsiCollaboratorManager.Models.UserAccount;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PsiCollaboratorManager.Controllers
{
    public class UserAccountController : Controller
    {
        private IPasswordPolicyRepository _passwordPolicyRepository;
        private IUserAccountRepository _userAccountRepository;
        private ILoginAttemptUserRepository _loginAttemptUserRepository;
        private ILoginAttemptPolicyRepository _loginAttemptPolicyRepository;
        private UserAccountMapper _userAccountMapper;
        private IMapper _mapper;
        private const string DEFAULT_PASSWORD = "psi123";
        public UserAccountController()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IUserAccount, UserAccountModel>();
                cfg.CreateMap<UserAccountModel, IUserAccount>();
            });
            _passwordPolicyRepository = new PasswordPolicyRepository();
            _loginAttemptPolicyRepository = new LoginAttemptPolicyRepository();
            _userAccountRepository = new UserAccountRepository();
            _loginAttemptUserRepository = new LoginAttemptUserRepository();
            _userAccountMapper = new UserAccountMapper();
            _mapper = configuration.CreateMapper();
        }
        public ActionResult Index()
        {
            ViewBag.BasicTitle = "Usuarios";
            return View();
        }
        public ActionResult GetAll()
        {
            IEnumerable<IUserAccount> userAccounts = _userAccountRepository.GetAll();
            List<UserAccountModel> userAccountModels = _mapper.Map<List<UserAccountModel>>(userAccounts);
            return Json(new { rows = userAccountModels }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int userAccountId)
        {
            ViewBag.BasicTitle = "Detalles Usuario";
            IUserAccount userAccount = _userAccountRepository.GetById(userAccountId);
            UserAccountModel userAccountModel = _mapper.Map<UserAccountModel>(userAccount);
            return View(userAccountModel);
        }
        public ActionResult Create()
        {
            ViewBag.BasicTitle = "Agregar Usuario";
            return View();
        }
        [HttpPost]
        public ActionResult Create(UserAccountModel userAccountModel)
        {
            try
            {
                string userName = getUserName(userAccountModel.FirstName, userAccountModel.LastName);
                IUserAccountInsert userAccount = _userAccountMapper.Fill(userAccountModel, userName, DEFAULT_PASSWORD);
                _userAccountRepository.Insert(userAccount);
            }
            catch (Exception ex)
            {
                TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-danger", Title = "Error!", Message = ex.Message };
                return RedirectToAction("Create");
            }
            return RedirectToAction("Index");
        }
        private string getUserName(string firstName, string lastName)
        {
            string fullLastName = lastName;
            string[] names = fullLastName.Split(' ');
            string firstLastName = names[0];
            string word = firstName.ToLower();
            char firstLetter = word[0];
            var userN = firstLetter + firstLastName.ToLower();
            IEnumerable<IUserAccount> users = _userAccountRepository.GetAll();

            IEnumerable<IUserAccount> repeatedUsers = users.Where(x => x.UserName.ToLower().Contains(userN.ToLower()));
            if(repeatedUsers.Count() == 0) return userN; //Es unico
            userN += repeatedUsers.Count(); //agrega un contador para hacerlo unico
            return userN;
        }

        public ActionResult Edit(int userAccountId)
        {
            ViewBag.BasicTitle = "Detalles Usuario";
            IUserAccount userAccount = _userAccountRepository.GetById(userAccountId);
            UserAccountModel userAccountModel = _mapper.Map<UserAccountModel>(userAccount);
            return View(userAccountModel);
        }
        [HttpPost]
        public ActionResult Edit(UserAccountModel userAccountModel)
        {
            if (userAccountModel.RestorePassword)
            {
                userAccountModel.NeedPasswordChange = true;
                _userAccountRepository.UpdatePassword(userAccountModel.UserAccountId, DEFAULT_PASSWORD, true);
            }
            IUserAccount userAccount = _mapper.Map<IUserAccount>(userAccountModel);
            _userAccountRepository.Update(userAccount);
            return RedirectToAction("Index");
        }
        #region Login

        public ActionResult Login()
        {
            ViewData["AppVersion"] = getVersion();
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyUserAccount(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IUserAccountFull userAccount = _userAccountRepository.GetByUserName(loginModel.UserName);
                    if (userAccount != null && !string.IsNullOrEmpty(userAccount.PasswordHash) && PBKDF2Converter.IsValidPassword(loginModel.Password, userAccount.PasswordHash))
                    {
                        if (userAccount.IsActive)
                        {
                            DateTime dateTimeNow = DateTime.UtcNow.AddHours(-6);
                            DateTime lockOutEndTime = userAccount.LockOutEndTime.HasValue ? (DateTime)userAccount.LockOutEndTime : new DateTime();
                            if (userAccount.IsLockedOut && dateTimeNow < lockOutEndTime)
                            {
                                TimeSpan timeleft = lockOutEndTime.Subtract(dateTimeNow);
                                TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-danger", Title = "Error", Message = "Su cuenta fue bloqueada. Espere a por " + (int)Math.Ceiling(timeleft.TotalMinutes) + "minutos e inténtelo de nuevo o póngase en contacto con el administrador de su cuenta. " };
                            }
                            else
                            {
                                if (userAccount.IsLockedOut)
                                {
                                    userAccount.IsLockedOut = false;
                                    _userAccountRepository.Update(_userAccountMapper.Map(userAccount));
                                }
                                IPAddress ipAddress = Request.ServerVariables["REMOTE_ADDR"] != null ? IPAddress.Parse(Request.ServerVariables["REMOTE_ADDR"]) : null;
                                _loginAttemptUserRepository.Insert(new LoginAttemptUser { UserAccountId = userAccount.UserAccountId, IsSuccess = true, IpAddress = ipAddress, ApplicationName = "PSI Collaborator Manager." });
                                IPasswordPolicy passwordPolicy = getPasswordPolicy();
                                TimeSpan timeLastChangePassword = DateTime.UtcNow.AddHours(-6).Subtract(userAccount.PasswordChangedDate);
                                if (userAccount.NeedPasswordChange || (passwordPolicy.PasswordDuration > 0 && timeLastChangePassword.TotalDays >= passwordPolicy.PasswordDuration)) return Redirect("/UserAccount/ChangePassword?userName=" + userAccount.UserName);
                                Session["UserAccount"] = userAccount.UserAccountId;
                                Session["FullName"] = userAccount.FirstName + " " + userAccount.LastName;
                                Session["AppVersion"] = getVersion();
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-danger", Title = "Error!", Message = "Su cuenta ha sido desactivada. Póngase en contacto con el administrador del sistema." };
                        }
                    }
                    else
                    {
                        if (userAccount != null)
                        {
                            ILoginAttemptPolicy loginAttemptPolicy = getLoginAttemptPolicy();
                            ILockOut lockOutUser = _userAccountRepository.VerifyAccountIsLockedOut(userAccount.UserAccountId, loginAttemptPolicy.MaxInvalidAttempts - 1, loginAttemptPolicy.InvalidAttemptsTime, loginAttemptPolicy.LoginLockoutTime);
                            if (lockOutUser != null)
                            {
                                var dateTimeNow = DateTime.UtcNow.AddHours(-6);
                                var lockOutEndTime = lockOutUser.LockOutEndTime.HasValue ? (DateTime)
                                    lockOutUser.LockOutEndTime : new DateTime();
                                if (lockOutUser.IsLockedOut && dateTimeNow < lockOutEndTime)
                                {
                                    var timeLeft = lockOutEndTime.Subtract(dateTimeNow);
                                    TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-danger", Title = "Error!", Message = "Su cuenta fue bloqueada. Espere a por " + (int)Math.Ceiling(timeLeft.TotalMinutes) + "minutos e inténtelo de nuevo o póngase en contacto con el administrador de su cuenta. " };
                                }
                                else
                                {
                                    var ipAddress = Request.ServerVariables["REMOTE_ADDR"] != null ? IPAddress.Parse(Request.ServerVariables["REMOTE_ADDR"]) : null;
                                    _loginAttemptUserRepository.Insert(new LoginAttemptUser { UserAccountId = userAccount.UserAccountId, IsSuccess = false, IpAddress = ipAddress, ApplicationName = "PSI Collaborator Manager." });
                                    TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-danger", Title = "Error!", Message = "La contraseña no coincide con el nombre de usuario." };
                                }
                            }
                        }
                        else
                            TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-danger", Title = "Error!", Message = "No se ha encontrado el nombre de usuario o el número de operador." };
                    }
                }
                catch (Exception ex)
                {
                    TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-danger", Title = "Error!", Message = "No se ha podido iniciar sesión. " + ex.Message };
                }
            }
            return View("../UserAccount/Login");
        }
        public ActionResult ChangePassword(string userName)
        {
            try
            {
                IPasswordPolicy passwordPolicy = getPasswordPolicy();
                Session["MinLength"] = passwordPolicy.MinLength;
                Session["PasswordDuration"] = passwordPolicy.PasswordDuration;
                Session["UseLowerCase"] = passwordPolicy.UseLowerCase;
                Session["UseUpperCase"] = passwordPolicy.UseUpperCase;
                Session["UseNumbers"] = passwordPolicy.UseNumbers;
                Session["UseSymbols"] = passwordPolicy.UseSymbols;
            }
            catch (Exception ex)
            {
                TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-danger", Title = "Error!", Message = "No se ha podido cargar la política de contraseñas." + ex.Message };
            }
            return View(new ChangePasswordModel() { UserName = userName, OldPassword = "", Password = "", ConfirmPassword = ""});
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (!ModelState.IsValid)
                return View(changePasswordModel);
            try
            {
                IUserAccountFull userAccount = _userAccountRepository.GetByUserName(changePasswordModel.UserName);
                if (userAccount != null)
                {
                    if (!PBKDF2Converter.IsValidPassword(changePasswordModel.OldPassword, userAccount.PasswordHash))
                            TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-danger", Title = "Error!", Message = "La contraseña actual no es válida." };
                    else
                        return validatePasswordAndUpdate(changePasswordModel.Password, userAccount, "ChangePassword");
                }
                else
                    TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-danger", Title = "Error!", Message = "Could not find your user account. Please contact the system administrator." };
            }
            catch (Exception ex)
            {
                TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-danger", Title = "Error!", Message = "No se ha podido cambiar la contraseña. " + ex.Message };
            }
            return View(changePasswordModel);
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return RedirectToAction("Login", "UserAccount");
        }

        private ActionResult validatePasswordAndUpdate(string newPassword, IUserAccountFull userAccount, string viewName)
        {
            string messageError = null;
            var passwordPolicy = getPasswordPolicy();
            var passwordValidator = getPasswordValidator();
            var passwordContent = passwordValidator.CheckPasswordPolicy(newPassword);
            if (passwordPolicy.UseUpperCase && !passwordContent.ContainsUpperCase)
                messageError = "La nueva contraseña requiere al menos una letra mayúscula";
            else if (passwordPolicy.UseLowerCase && !passwordContent.ContainsLowerCase)
                messageError = "La nueva contraseña requiere al menos una letra minúscula.";
            else if (passwordPolicy.UseNumbers && !passwordContent.CointainsNumber)
                messageError = "La nueva contraseña requiere al menos un número.";
            else if (passwordPolicy.UseSymbols && !passwordContent.CointainsSymbol)
                messageError = "La nueva contraseña requiere al menos un carácter especial.";
            else if (passwordPolicy.CheckIfIsRecent && _userAccountRepository.CheckIfPasswordIsRecent(userAccount.UserAccountId, newPassword))
                messageError = "La nueva contraseña no puede ser la misma que una de tus 10 contraseñas más recientes.";
            else if (passwordPolicy.CheckIfIsCommonlyUsed && CommonPasswordChecker.CheckPasswordIsCommonlyUsed(newPassword))
                messageError = "La nueva contraseña está en una lista de contraseñas utilizadas habitualmente en otros sitios web.";
            if (string.IsNullOrEmpty(messageError))
            {
                _userAccountRepository.UpdatePassword(userAccount.UserAccountId, newPassword, false);
                TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-success", Title = "Éxito!", Message = "Su contraseña se ha actualizado correctamente." };
                return RedirectToAction("Login", "UserAccount");
            }
            else
            {
                TempData["UserMessage"] = new MessageModel() { CssClassName = "alert-danger", Title = "Error!", Message = messageError };
                return RedirectToAction("Login", viewName);
            }
        }
        private string getVersion()
        {
            return typeof(MvcApplication).Assembly.GetName().Version.ToString(3);
        }
        private IPasswordPolicy getPasswordPolicy()
        {
            return _passwordPolicyRepository.GetById(int.Parse(ConfigurationManager.AppSettings["passwordPolicy"]));
        }
        private ILoginAttemptPolicy getLoginAttemptPolicy()
        {
            return _loginAttemptPolicyRepository.GetById(int.Parse(ConfigurationManager.AppSettings["loginAttemptPolicy"]));
        }
        private PasswordValidator getPasswordValidator()
        {
            var passwordPolicy = getPasswordPolicy();
            var passwordValidator = new PasswordValidator(passwordPolicy.MinLength, passwordPolicy.UseLowerCase, passwordPolicy.UseUpperCase, passwordPolicy.UseNumbers, passwordPolicy.UseSymbols);
            return passwordValidator;
        } 
        #endregion
    }
}