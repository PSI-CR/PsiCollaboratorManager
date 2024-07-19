using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.UserAccount
{
    public class UserAccountModel
    {
        public int UserAccountId { get; set; }
        [Display(Name ="Usuario")]
        public string UserName { get; set; }
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }
        [Display(Name = "Apellido")]
        public string LastName { get; set; }
        [Display(Name = "Correo")]
        public string Email { get; set; }
        public string Telephone1 { get; set; }
        public bool NeedPasswordChange { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime? LockOutEndTime { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Restaurar contraseña? (Por defecto: psi123)")]
        public bool RestorePassword { get; set; } = false;
    }
}